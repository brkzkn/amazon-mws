using System;
using System.Linq;
using System.Windows.Forms;

namespace ReadersHub.ProductAnalyzer
{
    public partial class CriteriaSettings : Form
    {
        private readonly Properties.Settings Settings;
        public CriteriaSettings()
        {
            InitializeComponent();
            Settings = Properties.Settings.Default;
            InitializeValues();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var checkedItems = clb_subCondition.CheckedItems;
            Settings.SUB_CONDITION.Clear();
            foreach (var item in checkedItems)
            {
                Settings.SUB_CONDITION.Add((string)item);
            }
            Settings.FEEDBACK_COUNT = (int)nud_feedbackCount.Value;
            Settings.FEEDBACK_RATING = (int)nud_feedbackRating.Value;
            Settings.Save();
            this.Close();
        }

        private void InitializeValues()
        {
            nud_feedbackCount.Value = Settings.FEEDBACK_COUNT;
            nud_feedbackRating.Value = Settings.FEEDBACK_RATING;
            var subConditionList = Settings.SUB_CONDITION.Cast<string>().ToList();
            for (int i = 0; i < clb_subCondition.Items.Count; i++)
            {
                if (subConditionList.Contains(clb_subCondition.Items[i].ToString()))
                {
                    clb_subCondition.SetItemChecked(i, true);
                }
            }
        }
    }
}
