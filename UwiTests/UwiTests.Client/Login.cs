using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UwiTests.Services;

namespace UwiTests
{
    public partial class Login : Form
    {
        private readonly MainForm _mainForm;
        private readonly AuthService _authService;

        public Login(MainForm mainForm, AuthService authService)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _authService = authService;

            this.button2.Click += new EventHandler(this.btnLogin_Click);
            this.button1.Click += new EventHandler(this.btnBack_Click);
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string login = this.textBox2.Text;  // textBox2 - поле для логина
            string password = this.textBox1.Text; // textBox1 - поле для пароля

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            button2.Enabled = false;
            button2.Text = "Вход...";

            try
            {
                bool success = await Task.Run(() => _authService.Login(login, password));

                if (success)
                {
                    var currentUser = _authService.GetCurrentUser();
                    var userRole = currentUser?.Role ?? "User";

                    MessageBox.Show($"Вход выполнен успешно!\nРоль: {userRole}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Очистка полей
                    textBox2.Clear();
                    textBox1.Clear();

                    // Переход на главную форму с тестами
                    _mainForm.ShowTestsMainForm();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button2.Enabled = true;
                button2.Text = "Войти";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Очистка полей
            textBox2.Clear();
            textBox1.Clear();

            // Возврат на стартовую форму
            _mainForm.ShowStartForm();
        }


    }
}