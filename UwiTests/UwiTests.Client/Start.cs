using System;
using System.Windows.Forms;

namespace UwiTests
{
    public partial class Start : Form
    {
        private MainForm _mainForm;

        public Start(MainForm mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Переход на форму регистрации
            _mainForm.ShowRegistrateForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Переход на форму входа
            _mainForm.ShowLoginForm();
        }
    }
}