using System;
using System.Windows.Forms;

namespace ReadersHub.ProductAnalyzer
{
    public partial class StoreSettings : Form
    {
        private readonly Properties.Settings Settings;

        public StoreSettings()
        {
            InitializeComponent();
            Settings = Properties.Settings.Default;
            tb_accessKeyId.Text = Settings.ACCESS_KEY_ID;
            tb_marketPlaceId.Text = Settings.MARKETPLACE_ID;
            tb_mwsDestination.Text = Settings.MWS_DESTINATION;
            tb_secretKey.Text = Settings.SECRET_KEY;
            tb_sellerId.Text = Settings.SELLER_ID;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Settings.ACCESS_KEY_ID = tb_accessKeyId.Text;
            Settings.MARKETPLACE_ID = tb_marketPlaceId.Text;
            Settings.MWS_DESTINATION = tb_mwsDestination.Text;
            Settings.SECRET_KEY = tb_secretKey.Text;
            Settings.SELLER_ID = tb_sellerId.Text;
            Settings.Save();

            /// TODO: Check API validation key
            /// 

            this.Close();
        }
    }
}
