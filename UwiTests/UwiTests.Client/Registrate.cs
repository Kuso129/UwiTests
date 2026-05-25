using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using UwiTests.Services;

namespace UwiTests
{
    public partial class Registrate : Form
    {
        private readonly MainForm _mainForm;
        private readonly AuthService _authService;

        public Registrate(MainForm mainForm, AuthService authService)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _authService = authService;

            // обработчики событий
            this.btnRegistrate.Click += new EventHandler(this.btnRegistrate_Click);
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
        }

        private async void btnRegistrate_Click(object sender, EventArgs e)
        {
            string login = this.txtLogin.Text;
            string password = this.txtPassword.Text;

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLogin.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            btnRegistrate.Enabled = false;
            btnRegistrate.Text = "Регистрация...";

            try
            {
                bool success = await Task.Run(() => _authService.Register(login, password));

                if (success)
                {
                    MessageBox.Show("Регистрация успешна! Теперь вы можете войти.", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Очистка полей
                    txtLogin.Clear();
                    txtPassword.Clear();

                    // Переход на форму входа
                    _mainForm.ShowLoginForm();
                }
                else
                {
                    MessageBox.Show("Пользователь с таким логином уже существует", "Ошибка",
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
                btnRegistrate.Enabled = true;
                btnRegistrate.Text = "Зарегистрироваться";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Очистка полей
            txtLogin.Clear();
            txtPassword.Clear();

            // Возврат на стартовую форму
            _mainForm.ShowStartForm();
        }
    }
}
