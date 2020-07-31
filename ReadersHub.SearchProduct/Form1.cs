using _21stSolution.Extensions;
using Nager.AmazonProductAdvertising;
using Nager.AmazonProductAdvertising.Model;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ReadersHub.SearchProduct
{
    public partial class Form1 : Form
    {
        private AmazonWrapper wrapper;
        private string _fileName;
        private List<string> PublisherList;
        private List<string> SeriesList;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = true;
            PublisherList = new List<string>();
            SeriesList = new List<string>();
            var publisher = File.ReadAllText("publisher.txt");
            var series = File.ReadAllText("series.txt");

            var publisherList = publisher.Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in publisherList)
            {
                PublisherList.Add(item.ToLower(CultureInfo.InvariantCulture));
            }

            var seriesList = series.Split(new string[] { ",", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in seriesList)
            {
                SeriesList.Add(item.ToLower(CultureInfo.InvariantCulture));
            }
        }

        private void btn_openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "xlsx dosyası(*.xlsx)|*.xlsx|xls files (*.xls)|*.xls";
            openFileDialog.Title = "Sipariş dosyasını seçiniz.";

            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_fileName.Text = openFileDialog.SafeFileName;
                _fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            }
        }
        private void btn_apply_Click(object sender, EventArgs e)
        {
            rtb_log.Clear();
            var authentication = new AmazonAuthentication();
            string associateTag = string.Empty;
            if (rb_us.Checked)
            {
                authentication.AccessKey = ConfigurationManager.AppSettings["US_AccessKey"];
                authentication.SecretKey = ConfigurationManager.AppSettings["US_SecretKey"];
                associateTag = ConfigurationManager.AppSettings["US_AssociateTag"];
                wrapper = new AmazonWrapper(authentication, AmazonEndpoint.US, associateTag);
            }
            else if (rb_ca.Checked)
            {
                authentication.AccessKey = ConfigurationManager.AppSettings["CA_AccessKey"];
                authentication.SecretKey = ConfigurationManager.AppSettings["CA_SecretKey"];
                associateTag = ConfigurationManager.AppSettings["CA_AssociateTag"];
                wrapper = new AmazonWrapper(authentication, AmazonEndpoint.CA, associateTag);
            }
            else
            {
                authentication.AccessKey = ConfigurationManager.AppSettings["UK_AccessKey"];
                authentication.SecretKey = ConfigurationManager.AppSettings["UK_SecretKey"];
                associateTag = ConfigurationManager.AppSettings["UK_AssociateTag"];
                wrapper = new AmazonWrapper(authentication, AmazonEndpoint.UK, associateTag);
            }

            wrapper.ErrorReceived += (errorResonse) =>
            {
                SetText(errorResonse.Error.Message);
            };

            backgroundWorker1.RunWorkerAsync();
            btn_apply.Enabled = false;
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
        private List<ExcelModel> AmazonApiCall(List<string> isbns)
        {
            ItemLookupResponse result = null;
            int exceptionCount = 1;
            do
            {
                
                var ilo = wrapper.ItemLookupOperation(isbns);
                //ilo.SearchIndex(AmazonSearchIndex.Books);
                //ilo.SearchIndex(AmazonSearchIndex.Toys);
                var xmlResponse = wrapper.Request(ilo);
                result = XmlHelper.ParseXml<ItemLookupResponse>(xmlResponse.Content);
                if (result == null)
                {
                    SetText($"Mesaj okunamadı. {exceptionCount} sn. bekleniyor.");
                    Thread.Sleep(1000 * exceptionCount++);
                }
                else
                {
                    break;
                }
            } while (true);

            Thread.Sleep(1000);

            /*
                    Alınan ürün bilgilerinden title, binding ve power durumlarına göre bulunan ürünlerin 
                    listesi alınır. Bu liste içinden ASIN'ler çekilir.
             */
            List<ExcelModel> excelResult = new List<ExcelModel>();
            List<Item> itemList = new List<Item>();

            if (result.Items.Item == null)
                return excelResult;

            foreach (var isbnItem in result.Items.Item)
            {
                if (isbnItem.ItemAttributes == null)
                    continue;

                string title = isbnItem.ItemAttributes.Title;

                string binding = isbnItem.ItemAttributes.Binding;
                if (binding == "Kindle Edition")
                {
                    continue;
                }

                SetText($"{isbnItem.ItemAttributes.ISBN} için ({isbnItem.ItemAttributes.Binding}) ASIN'ler bulunuyor");

                var asinByQuery = GetAsinByQuery(title, binding);
                if (asinByQuery?.Count == 0)
                {
                    SetText($"{isbnItem.ItemAttributes.ISBN} için ASIN bulunamadı.");
                    continue;
                }
                foreach (var asinItem in asinByQuery)
                {
                    if (asinItem.ItemAttributes.Binding == "Kindle Edition" || asinItem.ASIN[0] != 'B' || asinItem.ItemAttributes.Binding != binding)
                    {
                        continue;
                    }

                    var excelModel = new ExcelModel()
                    {
                        Isbn = isbnItem.ItemAttributes.ISBN,
                        IsbnBinding = isbnItem.ItemAttributes.Binding,
                        IsbnSalesRank = isbnItem.SalesRank,
                        IsbnTitle = isbnItem.ItemAttributes.Title,
                        IsbnPublicationDate = isbnItem.ItemAttributes.PublicationDate,
                        IsbnMinNewPrice = isbnItem.OfferSummary?.LowestNewPrice?.FormattedPrice,
                        IsbnMinUsedPrice = isbnItem.OfferSummary?.LowestUsedPrice?.FormattedPrice,
                        Asin = asinItem.ASIN,
                        AsinSalesRank = asinItem.SalesRank,
                        AsinMinNewPrice = asinItem.OfferSummary?.LowestNewPrice?.FormattedPrice,
                        AsinMinUsedPrice = asinItem.OfferSummary?.LowestUsedPrice?.FormattedPrice,
                        AsinPublicationDate = asinItem.ItemAttributes.PublicationDate,
                        AsinTitle = asinItem.ItemAttributes.Title,
                        AsinBinding = asinItem.ItemAttributes.Binding,
                        AsinProductLink = asinItem.DetailPageURL.Remove(asinItem.DetailPageURL.IndexOf("?")),
                        AsinProductImageLink = asinItem.LargeImage?.URL,
                        IsbnProductImageLink = isbnItem.LargeImage?.URL,
                    };

                    List<string> controlResult = new List<string>();
                    excelModel.IsSuccess = true;
                    if (TitleSimilarityControl(isbnItem, asinItem, out controlResult) == false)
                    {
                        excelModel.IsSuccess = false;
                        excelModel.ControlResult.AddRange(controlResult);
                    }
                    else
                    {
                        excelModel.IsSuccess = (excelModel.IsSuccess && AuthorControl(isbnItem, asinItem, out controlResult));
                        if (controlResult.Count > 0)
                        {
                            excelModel.ControlResult.AddRange(controlResult);
                        }

                        controlResult = new List<string>();
                        excelModel.IsSuccess = (excelModel.IsSuccess && PublisherControl(isbnItem, asinItem, out controlResult));
                        if (controlResult.Count > 0)
                        {
                            excelModel.ControlResult.AddRange(controlResult);
                        }

                        controlResult = new List<string>();
                        excelModel.IsSuccess = (excelModel.IsSuccess && SeriesControl(isbnItem, asinItem, out controlResult));
                        excelModel.IsSeriesSuccess = TitleSeriesCheck(isbnItem, asinItem);
                        if (controlResult.Count > 0)
                        {
                            excelModel.ControlResult.AddRange(controlResult);
                        }
                    }


                    excelResult.Add(excelModel);
                }
                Thread.Sleep(1000);
            }

            return excelResult;
        }
        private List<Item> GetAsinByQuery(string keyword, string binding)
        {
            string power = "binding:" + binding + " and title:" + keyword;
            ItemSearchResponse res = null;
            int exceptionCount = 1;
            List<Item> itemList = new List<Item>();
            int currentPages = 1;
            int totalPages = 1;
            try
            {
                do
                {
                    if (currentPages > 10)
                    {
                        SetText("En fazla 10 sayfa ASIN okunabiliyor. Kalan sayfalar okunamıyor.");
                        break;
                    }
                    var searchOperation = wrapper.ItemSearchOperation(keyword, AmazonSearchIndex.Books, AmazonResponseGroup.Large);
                    searchOperation.Keywords(keyword);
                    searchOperation.ParameterDictionary.Add("power", power);
                    searchOperation.Skip(currentPages);
                    var response = wrapper.Request(searchOperation);

                    res = XmlHelper.ParseXml<ItemSearchResponse>(response.Content);
                    if (res != null)
                    {
                        itemList.AddRange(res.Items.Item.ToList());
                        if (!string.IsNullOrEmpty(res.Items.TotalPages))
                        {
                            totalPages = int.Parse(res.Items.TotalPages);
                            SetText($"{currentPages}. sayfadaki ASIN'ler çekildi. Toplam sayfa: {totalPages}");
                        }
                        currentPages += 1;
                        Thread.Sleep(1100);
                    }
                    else
                    {
                        SetText($"Mesaj okunamadı. {exceptionCount} sn. bekleniyor.");
                        Thread.Sleep(1100 * exceptionCount++);
                    }

                } while (res == null || currentPages != (totalPages + 1));
            }
            catch (Exception)
            {
            }

            return itemList;
        }
        private void SaveFile(List<ExcelModel> list)
        {

            string fileName = $"{_fileName}_{DateTime.Now.ToString("ddMMyy_HHmmss")}.xlsx";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var doc = new SLDocument())
            {
                // Header
                doc.SetCellValue(1, 1, "ISBN");
                doc.SetCellValue(1, 2, "ISBN Rank");
                doc.SetCellValue(1, 3, "ISBN PubDate");
                doc.SetCellValue(1, 4, "ISBN Binding");
                doc.SetCellValue(1, 5, "ISBN NewPrice");
                doc.SetCellValue(1, 6, "ISBN UsedPrice");
                doc.SetCellValue(1, 7, "ASIN");
                doc.SetCellValue(1, 8, "ASIN Rank");
                doc.SetCellValue(1, 9, "ASIN PubDate");
                doc.SetCellValue(1, 10, "ASIN Binding");
                doc.SetCellValue(1, 11, "ASIN NewPrice");
                doc.SetCellValue(1, 12, "ASIN UsedPrice");
                doc.SetCellValue(1, 13, "ISBN Title");
                doc.SetCellValue(1, 14, "ASIN Title");
                doc.SetCellValue(1, 15, "Result");
                doc.SetCellValue(1, 16, "ASINProductLink");

                var defaultStyle = doc.CreateStyle();
                var failStyle = doc.CreateStyle();
                var seriesStyle = doc.CreateStyle();

                failStyle.Fill.SetPatternBackgroundColor(Color.Blue);
                seriesStyle.Fill.SetPatternBackgroundColor(Color.Red);

                Color myRgbColor = new Color();
                myRgbColor = Color.FromArgb(83, 141, 213);

                Color mySeriesRgbColor = new Color();
                mySeriesRgbColor = Color.FromArgb(141, 213, 83);

                failStyle.Fill.SetPattern(DocumentFormat.OpenXml.Spreadsheet.PatternValues.Solid, myRgbColor, Color.AliceBlue);
                seriesStyle.Fill.SetPattern(DocumentFormat.OpenXml.Spreadsheet.PatternValues.Solid, mySeriesRgbColor, Color.Red);

                for (int i = 0; i < list.Count; i++)
                {
                    int rowIndex = i + 2;

                    if (list[i].IsSeriesSuccess)
                    {
                        doc.SetRowStyle(rowIndex, seriesStyle);
                    }
                    else if (list[i].IsSuccess)
                    {
                        doc.SetRowStyle(rowIndex, defaultStyle);
                    }
                    else
                    {
                        doc.SetRowStyle(rowIndex, failStyle);
                    }

                    doc.SetCellValue(rowIndex, 1, list[i].Isbn);
                    doc.SetCellValue(rowIndex, 2, list[i].IsbnSalesRank);
                    doc.SetCellValue(rowIndex, 3, list[i].IsbnPublicationDate);
                    doc.SetCellValue(rowIndex, 4, list[i].IsbnBinding);
                    doc.SetCellValue(rowIndex, 5, list[i].IsbnMinNewPrice);
                    doc.SetCellValue(rowIndex, 6, list[i].IsbnMinUsedPrice);

                    doc.SetCellValue(rowIndex, 7, list[i].Asin);
                    doc.SetCellValue(rowIndex, 8, list[i].AsinSalesRank);
                    doc.SetCellValue(rowIndex, 9, list[i].AsinPublicationDate);
                    doc.SetCellValue(rowIndex, 10, list[i].AsinBinding);
                    doc.SetCellValue(rowIndex, 11, list[i].AsinMinNewPrice);
                    doc.SetCellValue(rowIndex, 12, list[i].AsinMinUsedPrice);

                    doc.SetCellValue(rowIndex, 13, list[i].IsbnTitle);
                    doc.SetCellValue(rowIndex, 14, list[i].AsinTitle);
                    string result = string.Join(",", list[i].ControlResult);
                    doc.SetCellValue(rowIndex, 15, result);
                    doc.SetCellValue(rowIndex, 16, list[i].AsinProductLink);

                }
                doc.SaveAs(fileName);
            }
        }
        private bool TitleSimilarityControl(Item isbnItem, Item asinItem, out List<string> controlResult)
        {
            controlResult = new List<string>();
            bool result = true;
            var similarityResult = CalculateSimilarity(isbnItem.ItemAttributes?.Title, asinItem.ItemAttributes?.Title);
            if (asinItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture).Contains(isbnItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture)))
            {
                controlResult.Add("ISBN kitap ismi ASIN'de geçiyor.");
            }
            else if (similarityResult < 0.21)
            {
                controlResult.Add($"ISBN kitap ismi, ASIN kitabına benzerlik oranı {Convert.ToInt32(similarityResult * 100)}.");
                result = false;
            }

            return result;
        }
        private bool AuthorControl(Item isbnItem, Item asinItem, out List<string> controlResult)
        {
            controlResult = new List<string>();
            bool result = true;
            if (isbnItem.ItemAttributes.Author == null)
            {
                controlResult.Add("ISBN'de yazar bilgisi bulunmuyor");
                result = false;

                return result;
            }

            if (asinItem.ItemAttributes.Author == null)
            {
                controlResult.Add("ASIN'de yazar ismi bulunmuyor.");
                result = true;
            }
            else
            {

                /// ASIN yazar isimleri ISBN yazar isimleri ile karşılaştırılıyor.
                /// 
                for (int i = 0; i < asinItem.ItemAttributes.Author.Length; i++)
                {
                    for (int k = 0; k < isbnItem.ItemAttributes.Author.Length; k++)
                    {
                        var asinAuthorName = asinItem.ItemAttributes.Author[i];
                        var isbnAuthorName = isbnItem.ItemAttributes.Author[k];

                        var asinSplittedAuthorName = asinAuthorName.ToLower(CultureInfo.InvariantCulture).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        var isbnSplittedAuthorName = isbnAuthorName.ToLower(CultureInfo.InvariantCulture).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        var intersect = asinSplittedAuthorName.Intersect(isbnSplittedAuthorName);
                        if (intersect.Count() != asinSplittedAuthorName.Length)
                        {
                            controlResult.Add($"{asinAuthorName} asin yazarı, {isbnAuthorName} isbn ismi ile aynı değil.");
                            result = false;
                        }
                        else
                        {
                            controlResult.Add($"{asinAuthorName} asin yazarı, {isbnAuthorName} isbn ismi ile aynı.");
                            result = true;
                            break;
                        }
                    }
                    if (result)
                    {
                        break;
                    }
                }
            }

            if (result)
            {
                return result;
            }

            /// ISBN'deki yazar isimleri kitap isminde aranıyor.
            /// 
            foreach (var isbnAuthor in isbnItem.ItemAttributes.Author)
            {
                var asinTitle = asinItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture);
                var isbnSplittedAuthorName = isbnAuthor.ToLower(CultureInfo.InvariantCulture).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (isbnSplittedAuthorName.All(s => asinTitle.Contains(s)))
                {
                    result = true;
                    controlResult.Add($"ISBN yazar ismi {isbnAuthor}, ASIN kitabı isminde bulundu");
                    break;
                }
                else
                {
                    controlResult.Add($"ISBN yazar ismi {isbnAuthor}, ASIN kitabı isminde bulunamadı");
                    result = false;
                }
            }

            return result;
        }
        private bool PublisherControl(Item isbnItem, Item asinItem, out List<string> controlResult)
        {
            bool result = true;
            controlResult = new List<string>();
            string isbnPublisher = isbnItem.ItemAttributes.Publisher?.ToLower(CultureInfo.InvariantCulture);
            string asinPublisher = asinItem.ItemAttributes.Publisher?.ToLower(CultureInfo.InvariantCulture);

            if (isbnItem.ItemAttributes.Publisher == null)
            {
                controlResult.Add("ISBN'de yayıncı bilgisi bulunmuyor.");
            }
            else
            {
                if (PublisherList.Contains(isbnItem.ItemAttributes.Publisher))
                {
                    if (asinItem.ItemAttributes.Publisher == null)
                    {
                        controlResult.Add("ASIN'de yayıncı bilgisi bulunmuyor");
                        result = false;
                    }
                    else if (!isbnPublisher.Contains(asinPublisher) && !asinPublisher.Contains(isbnPublisher))
                    {
                        controlResult.Add($"{asinPublisher} ASIN yayıncısı, {isbnPublisher} ISBN yayıncısı ile aynı değil.");
                        result = false;
                    }
                    else if (isbnPublisher.Contains(asinPublisher))
                    {
                        controlResult.Add($"ISBN yayıncısı ({isbnPublisher}) ile ASIN yayıncısı ({asinPublisher}) aynı.");
                    }
                }
                else
                {
                    controlResult.Add($"{isbnPublisher} yayıncı belirtilen listede bulunmuyor.");
                }
            }

            return result;
        }
        private bool SeriesControl(Item isbnItem, Item asinItem, out List<string> controlResult)
        {
            bool result = true;
            controlResult = new List<string>();
            string isbnTitle = isbnItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture);
            string asinTitle = asinItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture);

            foreach (var seriesKeyword in SeriesList)
            {
                if (isbnTitle.Contains(seriesKeyword))
                {
                    if (!asinTitle.Contains(seriesKeyword))
                    {
                        controlResult.Add($"ISBN kitap başlığında olan {seriesKeyword} kelimesi, ASIN'de mevcut değil.");
                        result = false;
                        return result;
                    }
                    else
                    {
                        var removedIsbnTitle = isbnTitle.Remove(0, isbnTitle.IndexOf(seriesKeyword));
                        var removedAsinTitle = asinTitle.Remove(0, asinTitle.IndexOf(seriesKeyword));
                        var isbnString = Regex.Match(removedIsbnTitle, @"\d+").Value;
                        var asinString = Regex.Match(removedAsinTitle, @"\d+").Value;
                        if (isbnString != asinString)
                        {
                            controlResult.Add($"ISBN serisi {isbnString}, ASIN serisi {asinString} ile eşit değil.");
                            result = false;
                            return result;
                        }
                    }
                }
            }
            return result;
        }
        private bool TitleSeriesCheck(Item isbnItem, Item asinItem)
        {
            bool result = true;
            string isbnTitle = isbnItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture);
            string asinTitle = asinItem.ItemAttributes.Title.ToLower(CultureInfo.InvariantCulture);

            foreach (var seriesKeyword in SeriesList)
            {
                if (!isbnTitle.Contains(seriesKeyword) && !asinTitle.Contains(seriesKeyword))
                {
                    result = false;
                }
                else
                {
                    return true;
                }
            }
            return result;
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pb_status.Value = e.ProgressPercentage;
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            List<string> excelISBNList = new List<string>();

            if (openFileDialog.FileName.IsNullOrEmpty())
            {
                MessageBox.Show("Lütfen önce dosyayı seçiniz.", "Amazon Feedback Uygulaması", MessageBoxButtons.OK, MessageBoxIcon.Error);
                backgroundWorker1.CancelAsync();
                return;
            }

            using (var doc = new SLDocument(openFileDialog.FileName))
            {
                SetText("Dosya açıldı.");
                var settings = doc.GetWorksheetStatistics();

                var rowCount = GetRowCount(doc, settings, 1);

                for (var row = 2; row <= rowCount; row++)
                {
                    var orderId = doc.GetCellValueAsString(row, 1);
                    excelISBNList.Add(orderId.Trim().PadLeft(10, '0'));
                }
            }

            //-- Excel dosyasındaki tüm ISBN'ler orderIdList'in içinde
            //
            excelISBNList = excelISBNList.Distinct().ToList();
            SetText($"Toplam {excelISBNList.Count} ISBN bulundu.");

            int isbnLength = excelISBNList.Count / 10;
            int maxStep = isbnLength + (excelISBNList.Count % 10 > 0 ? 1 : 0);
            SetProgressBarMaximumValue(maxStep);

            int currentStatus = 1;
            List<ExcelModel> excelResultList = new List<ExcelModel>();
            for (int i = 0; i < isbnLength; i++)
            {
                var list = excelISBNList.Skip(i * 10).Take(10).ToList();
                var amazonResult = AmazonApiCall(list);
                excelResultList.AddRange(amazonResult);
                Thread.Sleep(200);
                SetText($"{currentStatus * 10} ISBN işlendi.");
                backgroundWorker1.ReportProgress(currentStatus++);
            }

            if (excelISBNList.Count % 10 > 0)
            {
                var list = excelISBNList.Skip(isbnLength * 10).Take(10).ToList();
                var amazonResult = AmazonApiCall(list);
                excelResultList.AddRange(amazonResult);
                backgroundWorker1.ReportProgress(currentStatus++);
            }

            SaveFile(excelResultList);

            MessageBox.Show("İşlem tamamlandı.", "Amazon Feedback Uygulaması", MessageBoxButtons.OK, MessageBoxIcon.Information);
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btn_apply.Enabled = true;
            if (e.Error != null)
            {
                backgroundWorker1.CancelAsync();
                throw e.Error;
            }
        }

        delegate void SetTextCallback(string text);
        delegate void SetProgressBarMaximumValueCallback(int maximum);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (rtb_log.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                rtb_log.AppendText(text + "\r\n");
                rtb_log.ScrollToCaret();
            }
        }
        private void SetProgressBarMaximumValue(int maximum)
        {
            if (pb_status.InvokeRequired)
            {
                SetProgressBarMaximumValueCallback d = new SetProgressBarMaximumValueCallback(SetProgressBarMaximumValue);
                Invoke(d, new object[] { maximum });
            }
            else
            {
                pb_status.Maximum = maximum;
            }
        }
        private int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
        private double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

    }
}
