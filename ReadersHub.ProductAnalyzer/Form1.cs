using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using ReadersHub.ProductAnalyzer.Dto;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ReadersHub.ProductAnalyzer
{
    public partial class Form1 : Form
    {
        private MarketplaceWebServiceProducts.MarketplaceWebServiceProducts Client;
        private readonly Properties.Settings Settings;
        private double _remainingToken;
        private string _fileName;
        private DateTime _requestRestartDate;
        private List<ExcelDto> ProductList;
        private double RemainingToken
        {
            get
            {
                return Settings.REMAINING_TOKEN;
            }
            set
            {
                _remainingToken = value;
                Settings.REMAINING_TOKEN = value;
                Settings.Save();
                tssl_remainingTokenValue.Text = _remainingToken.ToString();
            }
        }
        private DateTime RequestRestartDate
        {
            get
            {
                return Settings.REQUEST_RESTART_DATE;
            }
            set
            {
                _requestRestartDate = value;
                Settings.REQUEST_RESTART_DATE = value;
                Settings.Save();
                tssl_requestRestartDateValue.Text = _requestRestartDate.ToString("dd.MM.yyyy HH:mm");
            }
        }

        public Form1()
        {
            InitializeComponent();
            Settings = Properties.Settings.Default;
            ProductList = new List<Dto.ExcelDto>();
            try
            {
                Client = InitializeClient();
                CheckForIllegalCrossThreadCalls = false;
                tssl_remainingTokenValue.Text = RemainingToken.ToString();
                tssl_requestRestartDateValue.Text = RequestRestartDate.ToString("dd.MM.yyyy HH:mm");
            }
            catch (Exception)
            {
                //MessageBox.Show("Uygulamayı kullanmak için önce ayarları düzenleyin.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Client = null;
            }
        }

        private void btn_calculate_Click(object sender, EventArgs e)
        {
            // ProcessExcel();
            backgroundWorker1.RunWorkerAsync();
            btn_calculate.Enabled = false;
        }

        private void ProcessExcel()
        {
            if (Client == null)
            {
                MessageBox.Show("Uygulamayı kullanmak için önce ayarları düzenleyin.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
                return;
            }

            List<ExcelDto> productList = new List<ExcelDto>();

            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                MessageBox.Show("Lütfen önce dosyayı seçiniz.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
                return;
            }

            using (var doc = new SLDocument(openFileDialog.FileName))
            {
                var settings = doc.GetWorksheetStatistics();

                var rowCount = GetRowCount(doc, settings, 2);

                for (var row = 2; row <= rowCount; row++)
                {
                    var isbn = doc.GetCellValueAsString(row, 1);
                    var asin = doc.GetCellValueAsString(row, 2);
                    productList.Add(new ExcelDto()
                    {
                        ISBN = isbn.PadLeft(10, '0'),
                        ASIN = asin.PadLeft(10, '0')
                    });
                }
            }

            var asinList = productList.Select(x => x.ASIN).Distinct().ToList();
            var isbnList = productList.Select(x => x.ISBN).Distinct().ToList();

            int asinLength = asinList.Count / 20;
            int isbnLength = isbnList.Count / 20;
            int maxStep = asinLength + isbnLength + (asinList.Count % 20 > 0 ? 1 : 0) + (isbnList.Count % 20 > 0 ? 1 : 0);

            for (int i = 0; i < asinLength; i++)
            {
                var list = asinList.Skip(i * 20).Take(20).ToList();
                CalculatePrice(list, productList, true, true);
                CalculatePrice(list, productList, true, false);
                CalculateSalesRank(list, productList, true);
            }

            if (asinList.Count % 20 > 0)
            {
                var list = asinList.Skip(asinLength * 20).Take(20).ToList();
                CalculatePrice(list, productList, true, true);
                CalculatePrice(list, productList, true, false);
                CalculateSalesRank(list, productList, true);
            }

            for (int i = 0; i < isbnLength; i++)
            {
                var list = isbnList.Skip(i * 20).Take(20).ToList();
                CalculatePrice(list, productList, false, true);
                CalculatePrice(list, productList, false, false);
                CalculateSalesRank(list, productList, false);
            }

            if (isbnList.Count % 20 > 0)
            {
                var list = isbnList.Skip(isbnLength * 20).Take(20).ToList();
                CalculatePrice(list, productList, false, true);
                CalculatePrice(list, productList, false, false);
                CalculateSalesRank(list, productList, false);
            }

            SaveFile(productList);


            MessageBox.Show("İşlem tamamlandı.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CalculatePrice(List<string> itemList, List<ExcelDto> excelList, bool isAsin, bool isUsed)
        {
            GetLowestOfferListingsForASINResponse response = null;
            int retry = 0;
            do
            {
                try
                {
                    response = Client.GetLowestOfferListingsForASIN(new GetLowestOfferListingsForASINRequest()
                    {
                        ASINList = new ASINListType()
                        {
                            ASIN = itemList,
                        },
                        SellerId = Settings.SELLER_ID,
                        MarketplaceId = Settings.MARKETPLACE_ID,
                        ExcludeMe = true,
                        ItemCondition = isUsed ? "used" : "new"
                    });
                }
                catch (Exception ex)
                {
                    response = null;
                    Thread.Sleep(1000);
                }
            } while (response == null && retry++ < 3);

            if (response == null)
            {
                /// TODO: itemList'in açıklamasını doldur.
                /// 
                SetErrorMessage(itemList, excelList, isAsin, "Ürün bilgileri alınamadı");
                return;
            }

            RemainingToken = response.ResponseHeaderMetadata.QuotaRemaining ?? 0;
            if (response.ResponseHeaderMetadata.QuotaResetsAt.HasValue)
            {
                RequestRestartDate = response.ResponseHeaderMetadata.QuotaResetsAt.Value;
            }

            if (response.GetLowestOfferListingsForASINResult != null)
            {
                ProcessResponse(excelList, response, isAsin, isUsed);
            }
        }
        private void CalculateSalesRank(List<string> itemList, List<ExcelDto> excelList, bool isAsin)
        {
            GetCompetitivePricingForASINResponse response = null;
            int retry = 0;
            do
            {
                try
                {
                    response = Client.GetCompetitivePricingForASIN(new GetCompetitivePricingForASINRequest()
                    {
                        ASINList = new ASINListType()
                        {
                            ASIN = itemList,
                        },
                        SellerId = Settings.SELLER_ID,
                        MarketplaceId = Settings.MARKETPLACE_ID,
                    });
                }
                catch (Exception ex)
                {
                    response = null;
                    Thread.Sleep(1000);
                }
            } while (response == null && retry++ < 3);

            if (response == null)
            {
                /// TODO: itemList'in açıklamasını doldur.
                /// 
                SetErrorMessage(itemList, excelList, isAsin, "Ürün bilgileri alınamadı");
                return;
            }

            if (response.GetCompetitivePricingForASINResult != null)
            {
                foreach (var item in response.GetCompetitivePricingForASINResult)
                {
                    if (item.Error != null)
                    {
                        if (isAsin)
                        {
                            excelList.Where(x => x.ASIN == item.ASIN).ToList().ForEach(x =>
                            {
                                x.Description.Add(item.Error.Message);
                            });
                        }
                        else
                        {
                            excelList.Where(x => x.ISBN == item.ASIN).ToList().ForEach(x =>
                            {
                                x.Description.Add(item.Error.Message);
                            });
                        }
                    }

                    if (item.Product != null && item.Product.SalesRankings != null && item.Product.SalesRankings.SalesRank != null && item.Product.SalesRankings.SalesRank.Count > 0)
                    {
                        if (isAsin)
                        {
                            excelList.Where(x => x.ASIN == item.ASIN).ToList().ForEach(x =>
                              {
                                  x.ASINSalesRank = item.Product.SalesRankings.SalesRank.Max(y => y.Rank);
                              });
                        }
                        else
                        {
                            excelList.Where(x => x.ISBN == item.ASIN).ToList().ForEach(x =>
                            {
                                x.ISBNSalesRank = item.Product.SalesRankings.SalesRank.Max(y => y.Rank);
                            });
                        }
                    }
                    else
                    {
                        if (isAsin)
                        {
                            excelList.Where(x => x.ASIN == item.ASIN).ToList().ForEach(x =>
                            {
                                x.Description.Add("ASIN Sales rank bulunamadı.");
                            });
                        }
                        else
                        {
                            excelList.Where(x => x.ISBN == item.ASIN).ToList().ForEach(x =>
                            {
                                x.Description.Add("ISBN Sales rank bulunamadı.");
                            });
                        }
                    }
                }
            }
            else
            {
                if (isAsin)
                {
                    excelList.Where(x => itemList.Contains(x.ASIN)).ToList().ForEach(x =>
                    {
                        x.Description.Add("GetCompetitivePricingForASINResult");
                    });
                }
                else
                {
                    excelList.Where(x => itemList.Contains(x.ISBN)).ToList().ForEach(x =>
                    {
                        x.Description.Add("GetCompetitivePricingForASINResult");
                    });
                }
            }
        }
        private void ProcessResponse(List<ExcelDto> processList, GetLowestOfferListingsForASINResponse response, bool isAsin, bool isUsed)
        {
            if (response.GetLowestOfferListingsForASINResult != null)
            {
                foreach (var item in response.GetLowestOfferListingsForASINResult)
                {
                    List<ExcelDto> excelRows = null;
                    string asinInfo = isAsin ? "ASIN" : "ISBN";
                    string usedInfo = isUsed ? "Used" : "New";

                    if (isAsin)
                    {
                        excelRows = processList.Where(x => x.ASIN == item.ASIN).ToList();
                    }
                    else
                    {
                        excelRows = processList.Where(x => x.ISBN == item.ASIN).ToList();
                    }

                    if (item.Product == null)
                    {
                        excelRows.ForEach(x =>
                        {
                            x.Description.Add($"Ürün bulunamadı. ({asinInfo} - {usedInfo})");
                        });
                        continue;
                    }

                    if (item.Product.LowestOfferListings.LowestOfferListing.Count > 0)
                    {
                        decimal price = 0;
                        foreach (var lowestPrice in item.Product.LowestOfferListings.LowestOfferListing)
                        {
                            price = lowestPrice.Price.ListingPrice.Amount;
                            string feedbackRating = "0";
                            if (!string.IsNullOrEmpty(lowestPrice.Qualifiers.SellerPositiveFeedbackRating))
                            {
                                feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(0, 2);
                                if (lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Length == 7)
                                {
                                    feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(3, 3);
                                }
                                else if (lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Length == 6)
                                {
                                    feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(3, 2);
                                }
                            }
                            string feedbackCount = lowestPrice.SellerFeedbackCount.ToString();
                            string subcondition = lowestPrice.Qualifiers.ItemSubcondition;
                            if (lowestPrice.Qualifiers.ItemCondition == "Used")
                            {
                                subcondition = $"Used - {subcondition}";
                            }
                            /// Kriterlere uyan en düşük fiyatlı ürünü bulunca döngüyü sonlandırıyoruz.
                            /// 
                            if (CheckCriteria(feedbackRating, feedbackCount, subcondition))
                            {
                                break;
                            }
                        }
                        /// Kriterlere uyan en düşük fiyatı set et.
                        /// 
                        if (isAsin)
                        {
                            if (isUsed)
                            {
                                excelRows.ForEach(x =>
                                {
                                    x.UsedASINPrice = price;
                                });
                            }
                            else
                            {
                                excelRows.ForEach(x =>
                                {
                                    x.NewASINPrice = price;
                                });
                            }
                        }
                        else
                        {
                            if (isUsed)
                            {
                                excelRows.ForEach(x =>
                                {
                                    x.UsedISBNPrice = price;
                                });
                            }
                            else
                            {
                                excelRows.ForEach(x =>
                                {
                                    x.NewISBNPrice = price;
                                });
                            }
                        }
                    }
                    else
                    {
                        excelRows.ForEach(x =>
                        {
                            x.Description.Add($"Ürün bulunamadı. ({asinInfo} - {usedInfo})");
                        });
                    }
                }
            }
        }
        private bool CheckCriteria(string feedbackRating, string feedbackCount, string subCondition)
        {
            var _subConditions = Settings.SUB_CONDITION.Cast<string>().ToList();

            int _feedbackCount = 0;
            if (int.TryParse(feedbackCount, out _feedbackCount))
            {
                if (_feedbackCount < Settings.FEEDBACK_COUNT)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            int _feedbackRating = 0;
            if (int.TryParse(feedbackRating, out _feedbackRating))
            {
                if (_feedbackRating < Settings.FEEDBACK_RATING)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (!_subConditions.Any(x => x == subCondition))
            {
                return false;
            }

            return true;
        }
        private int GetRowCount(SLDocument doc, SLWorksheetStatistics settings, int columnCount)
        {
            bool search = false;
            int rowCount = settings.EndRowIndex;
            //endrowindex may be greater than exact value. So clear empty rows and calculate true value.
            for (int row = settings.EndRowIndex; row >= settings.StartRowIndex; row--)
            {
                search = false;
                for (int col = settings.StartColumnIndex; col <= columnCount; col++)
                {
                    if (doc.HasCellValue(row, col))
                    {
                        search = true;
                        break;
                    }
                }
                if (search)
                    break;
                else
                    rowCount--;
            }
            return rowCount;
        }
        private void SetErrorMessage(List<string> itemList, List<ExcelDto> excelList, bool isAsin, string message)
        {
            List<ExcelDto> excelRows = null;
            string asinInfo = isAsin ? "ASIN" : "ISBN";
            foreach (var item in itemList)
            {
                if (isAsin)
                {
                    excelRows = excelList.Where(x => x.ASIN == item).ToList();
                }
                else
                {
                    excelRows = excelList.Where(x => x.ISBN == item).ToList();
                }

                excelRows.ForEach(x =>
                {
                    x.Description.Add(message);
                });
            }
        }
        private MarketplaceWebServiceProducts.MarketplaceWebServiceProducts InitializeClient()
        {
            if (Client != null)
            {
                return Client;
            }

            // The client application name
            string appName = "ProductAnalyzer";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = Settings.MWS_DESTINATION;

            // Create a configuration object
            MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
            config.ServiceURL = serviceURL;

            // Set other client connection configurations here if needed
            // Create the client itself
            string MWS_API_ACCESS_KEY_ID = Settings.ACCESS_KEY_ID;
            string MWS_API_SECRET_KEY = Settings.SECRET_KEY;

            return new MarketplaceWebServiceProductsClient(appName, appVersion, MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, config);
        }

        #region FormComponent Events
        private void tsmi_store_Click(object sender, EventArgs e)
        {
            var form = new StoreSettings();
            form.ShowDialog();
            Client = InitializeClient();
        }
        private void tsmi_criteria_Click(object sender, EventArgs e)
        {
            var form = new CriteriaSettings();
            form.ShowDialog();
        }
        private void btn_openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "xlsx dosyası(*.xlsx)|*.xlsx|xls files (*.xls)|*.xls";
            openFileDialog.Title = "Excel dosyasını seçiniz.";

            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_fileName.Text = openFileDialog.SafeFileName;
                _fileName = openFileDialog.SafeFileName;
            }
        }
        #endregion

        private void SaveFile(List<ExcelDto> list)
        {
            string fileName = $"{_fileName}";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            //File.Create(fileName);

            using (var doc = new SLDocument())
            {
                // Header
                doc.SetCellValue(1, 1, "ISBN");
                doc.SetCellValue(1, 2, "ASIN");
                doc.SetCellValue(1, 3, "NewISBNPrice");
                doc.SetCellValue(1, 4, "NewASINPrice");
                doc.SetCellValue(1, 5, "NewDifference");
                doc.SetCellValue(1, 6, "UsedISBNPrice");
                doc.SetCellValue(1, 7, "UsedASINPrice");
                doc.SetCellValue(1, 8, "UsedDifference");
                doc.SetCellValue(1, 9, "ISBNSalesRank");
                doc.SetCellValue(1, 10, "ASINSalesRank");

                for (int i = 0; i < list.Count; i++)
                {
                    int rowIndex = i + 2;
                    doc.SetCellValue(rowIndex, 1, list[i].ISBN);
                    doc.SetCellValue(rowIndex, 2, list[i].ASIN);
                    doc.SetCellValue(rowIndex, 3, list[i].NewISBNPrice);
                    doc.SetCellValue(rowIndex, 4, list[i].NewASINPrice);
                    doc.SetCellValue(rowIndex, 5, list[i].NewDifference);
                    doc.SetCellValue(rowIndex, 6, list[i].UsedISBNPrice);
                    doc.SetCellValue(rowIndex, 7, list[i].UsedASINPrice);
                    doc.SetCellValue(rowIndex, 8, list[i].UsedDifference);
                    doc.SetCellValue(rowIndex, 9, list[i].ISBNSalesRank);
                    doc.SetCellValue(rowIndex, 10, list[i].ASINSalesRank);
                    doc.SetCellValue(rowIndex, 11, string.Join(",", list[i].Description));
                }
                doc.SaveAs(fileName);
            }
        }

        #region BackgroundWorker Functions
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Client == null)
            {
                MessageBox.Show("Uygulamayı kullanmak için önce ayarları düzenleyin.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
                return;
            }


            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                MessageBox.Show("Lütfen önce dosyayı seçiniz.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
                return;
            }

            using (var doc = new SLDocument(openFileDialog.FileName))
            {
                var settings = doc.GetWorksheetStatistics();

                var rowCount = GetRowCount(doc, settings, 2);

                for (var row = 2; row <= rowCount; row++)
                {
                    var isbn = doc.GetCellValueAsString(row, 1);
                    var asin = doc.GetCellValueAsString(row, 2);
                    ProductList.Add(new ExcelDto()
                    {
                        ISBN = string.IsNullOrEmpty(isbn) ? string.Empty : isbn.PadLeft(10, '0'),
                        ASIN = string.IsNullOrEmpty(asin) ? string.Empty : asin.PadLeft(10, '0')
                    });
                }
            }

            var asinList = ProductList.Select(x => x.ASIN).Where(x => x != string.Empty).Distinct().ToList();
            var isbnList = ProductList.Select(x => x.ISBN).Where(x => x != string.Empty).Distinct().ToList();

            int asinLength = asinList.Count / 20;
            int isbnLength = isbnList.Count / 20;
            int maxStep = asinLength + isbnLength + (asinList.Count % 20 > 0 ? 1 : 0) + (isbnList.Count % 20 > 0 ? 1 : 0);
            int currentStep = 1;
            pb_status.Maximum = maxStep;

            for (int i = 0; i < asinLength; i++)
            {
                var list = asinList.Skip(i * 20).Take(20).ToList();
                CalculatePrice(list, ProductList, true, true);
                CalculatePrice(list, ProductList, true, false);
                CalculateSalesRank(list, ProductList, true);
                backgroundWorker1.ReportProgress(currentStep++);
            }

            if (asinList.Count % 20 > 0)
            {
                var list = asinList.Skip(asinLength * 20).Take(20).ToList();
                CalculatePrice(list, ProductList, true, true);
                CalculatePrice(list, ProductList, true, false);
                CalculateSalesRank(list, ProductList, true);
                backgroundWorker1.ReportProgress(currentStep++);
            }

            for (int i = 0; i < isbnLength; i++)
            {
                var list = isbnList.Skip(i * 20).Take(20).ToList();
                CalculatePrice(list, ProductList, false, true);
                CalculatePrice(list, ProductList, false, false);
                CalculateSalesRank(list, ProductList, false);
                backgroundWorker1.ReportProgress(currentStep++);
            }

            if (isbnList.Count % 20 > 0)
            {
                var list = isbnList.Skip(isbnLength * 20).Take(20).ToList();
                CalculatePrice(list, ProductList, true, true);
                CalculatePrice(list, ProductList, true, false);
                CalculateSalesRank(list, ProductList, false);
                backgroundWorker1.ReportProgress(currentStep++);
            }

            SaveFile(ProductList);

            MessageBox.Show("İşlem tamamlandı.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
            backgroundWorker1.CancelAsync();
        }
        // Back on the 'UI' thread so we can update the progress bar
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            pb_status.Value = e.ProgressPercentage;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                backgroundWorker1.CancelAsync();
                throw e.Error;
            }

            btn_calculate.Enabled = true;
        }

        #endregion

        private void tsmi_about_Click(object sender, EventArgs e)
        {
            var form = new AboutBox();
            form.ShowDialog();
        }
    }
}
