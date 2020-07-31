using _21stSolution.Extensions;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using RestSharp;
using RestSharp.Authenticators;
using SendGrid;
using SendGrid.Helpers.Mail;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ReadersHub.Mailing
{
    public partial class Form1 : Form
    {
        private string SELLER_ID;
        private string ACCESS_KEY_ID;
        private string SECRET_KEY;
        private string MARKET_PLACE_ID;
        private string MWS_DESTINATION;
        private MarketplaceWebServiceOrdersClient client;
        private int STORE_INDEX = -1;
        public Form1()
        {
            InitializeComponent();
            cb_store.SelectedIndex = 1;
            CheckForIllegalCrossThreadCalls = true;
        }

        private void btn_openFile_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "xlsx dosyası(*.xlsx)|*.xlsx|xls files (*.xls)|*.xls";
            openFileDialog.Title = "Sipariş dosyasını seçiniz.";

            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tb_fileName.Text = openFileDialog.SafeFileName;
            }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            rtb_log.Clear();
            backgroundWorker1.RunWorkerAsync();
            btn_apply.Enabled = false;
        }

        private void ApplyOrder(List<string> orderIds)
        {
            client = GetClient();
            GetOrderResponse response = null;
            int retry = 1;
            do
            {
                try
                {
                    response = client.GetOrder(new GetOrderRequest()
                    {
                        AmazonOrderId = orderIds,
                        SellerId = SELLER_ID
                    });
                }
                catch (Exception ex)
                {
                    response = null;
                    SetText($"Mesaj okunamadı {retry} sn. bekleniyor");
                    Thread.Sleep(1000 * retry);
                }
            } while (response == null && retry++ < 15);

            if (response.GetOrderResult != null && response.GetOrderResult.Orders != null)
            {
                foreach (var item in response.GetOrderResult.Orders)
                {
                    var mailResponse = SendSimpleMessage(item.BuyerName, item.BuyerEmail, item.AmazonOrderId);
                    if (mailResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        SetText($"Mail gönderilemedi. OrderID: \t{item.AmazonOrderId}\t Email:{item.BuyerEmail}\t Name:{item.BuyerName}\t Response: {mailResponse.ErrorMessage}");
                    }
                }
            }
        }

        private MarketplaceWebServiceOrdersClient GetClient()
        {
            // The client application name
            string appName = "ReadersHubMailing";

            // The client application version
            string appVersion = "1.0";

            // Create a configuration object
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = MWS_DESTINATION;
            // Set other client connection configurations here if needed
            // Create the client itself
            return new MarketplaceWebServiceOrdersClient(ACCESS_KEY_ID, SECRET_KEY, appName, appVersion, config);
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

        private IRestResponse SendSimpleMessage(string name, string email, string orderId)
        {
            RestRequest request = new RestRequest();
            var client = GetClient(email, name, orderId, request);
            var response = client.Execute(request);
            return response;
        }

        private void SetConfiguration(int index)
        {
            MWS_DESTINATION = ConfigurationManager.AppSettings["MWS_DESTINATION"];
            if (index == 0)
            {
                //-- Toffee
                SELLER_ID = ConfigurationManager.AppSettings["SELLER_ID_TOFFEE"];
                ACCESS_KEY_ID = ConfigurationManager.AppSettings["ACCESS_KEY_ID_TOFFEE"];
                SECRET_KEY = ConfigurationManager.AppSettings["SECRET_KEY_TOFFEE"];
                MARKET_PLACE_ID = ConfigurationManager.AppSettings["MARKET_PLACE_ID_TOFFEE"];
            }
            else
            {
                //-- Powerseller
                SELLER_ID = ConfigurationManager.AppSettings["SELLER_ID_POWERSELLER"];
                ACCESS_KEY_ID = ConfigurationManager.AppSettings["ACCESS_KEY_ID_POWERSELLER"];
                SECRET_KEY = ConfigurationManager.AppSettings["SECRET_KEY_POWERSELLER"];
                MARKET_PLACE_ID = ConfigurationManager.AppSettings["MARKET_PLACE_ID_POWERSELLER"];
            }
        }

        private RestClient GetClient(string email, string name, string orderId, RestRequest request)
        {
            RestClient client = new RestClient();
            string template = string.Empty;
            string apiKey = string.Empty;
            if (STORE_INDEX == 0)
            {
                apiKey = ConfigurationManager.AppSettings["ApiKey_Toffee"];

                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", apiKey);

                request.AddParameter("domain", "mg.toffeapplebook.com", ParameterType.UrlSegment);
                request.Resource = "mg.toffeapplebook.com/messages";
                request.AddParameter("from", "Toffee Apple Book UK<no-reply@toffeapplebook.com>");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Additional Information Required");
                template = GetTemplate();
            }
            else if (STORE_INDEX == 1)
            {
                apiKey = ConfigurationManager.AppSettings["ApiKey_Powerseller"];

                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", apiKey);

                request.AddParameter("domain", "mg.powersellerbookuk.com", ParameterType.UrlSegment);
                request.Resource = "mg.powersellerbookuk.com/messages";
                request.AddParameter("from", "Pi Store<no-reply@powersellerbookuk.com>");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Additional Information Required");
                template = GetTemplate();
            }

            template = template.Replace("#buyer-name#", name.First().ToString().ToUpper() + name.Substring(1));
            template = template.Replace("#order-id#", orderId);

            request.AddParameter("html", template);
            request.Method = Method.POST;

            return client;

        }

        private string GetTemplate()
        {
            try
            {
                return new StreamReader("template.txt").ReadToEnd();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (cb_store.SelectedIndex < 0)
            {
                MessageBox.Show("Lütfen işlem yapacağınız mağazayı seçiniz!", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            STORE_INDEX = cb_store.SelectedIndex;
            SetConfiguration(cb_store.SelectedIndex);

            List<string> orderIdList = new List<string>();
            if (openFileDialog.FileName.IsNullOrEmpty())
            {
                MessageBox.Show("Lütfen önce dosyayı seçiniz.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var doc = new SLDocument(openFileDialog.FileName))
            {
                //rtb_log.Text += "Dosya açıldı\r\n";
                SetText("Dosya açıldı");

                var settings = doc.GetWorksheetStatistics();

                var rowCount = GetRowCount(doc, settings, 1);
                //rtb_log.Text += $"Toplam {rowCount - 1} sipariş bulundu\r\n";
                SetText($"Toplam {rowCount - 1} sipariş bulundu");

                SetProgressBarMaximumValue(rowCount);
                for (var row = 2; row <= rowCount; row++)
                {
                    var orderId = doc.GetCellValueAsString(row, 1);
                    if (string.IsNullOrEmpty(orderId))
                        continue;
                    orderIdList.Add(orderId.Trim());
                    backgroundWorker1.ReportProgress(row);
                    if (orderIdList.Count == 50)
                    {
                        ApplyOrder(orderIdList);
                        orderIdList = new List<string>();
                        //rtb_log.Text += "50 sipariş işlendi.\r\n";
                        SetText("50 sipariş işlendi. 2 sn. bekliyor");
                        Thread.Sleep(2000);
                    }
                }

                if (orderIdList.Count > 0)
                {
                    ApplyOrder(orderIdList);
                    //rtb_log.Text += $"{orderIdList.Count} sipariş işlendi.\r\n";
                    SetText($"{orderIdList.Count} sipariş işlendi.");
                    orderIdList = new List<string>();
                }
                backgroundWorker1.ReportProgress(rowCount);
            }

            MessageBox.Show("İşlem tamamlandı.", "ReadersHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pb_status.Value = e.ProgressPercentage;
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
    }
}
