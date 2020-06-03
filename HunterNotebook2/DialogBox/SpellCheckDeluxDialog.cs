using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HunterNotebook2.DialogBox
{
    public partial class SpellCheckDeluxDialog : Form
    {
        public SpellCheckDeluxDialog()
        {
            InitializeComponent();
        }

        AzureBingSpellSheck handler = new AzureBingSpellSheck();
        ProcessedBingSpellcheck SpellcheckResults;
        Timer CheckResults = new Timer();
        Task<String> SpellTask;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SpellCheckDeluxDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckResults.Enabled = true;
            CheckResults.Interval = 10;
            CheckResults.Tick += CheckResults_Tick;

            SpellTask= handler.SpellCheckAsync(this.richTextBox1.Text);
            

        }

        private void CheckResults_Tick(object sender, EventArgs e)
        {
            if (SpellTask.IsCompleted == true)
            {
                CheckResults.Stop();
                MessageBox.Show(SpellTask.Result);
                CheckResults.Tick -= CheckResults_Tick;
            }
            
            
        }

        ProcessedBingSpellcheck result;
           
    }



}



