using System;
using System.Windows.Forms;

namespace UwiTests
{
    public partial class Start : Form
    {
        private MainForm mainForm;

        public Start(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.ShowLoginForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainForm.ShowRegistrateForm();
           
        }
    }
}
