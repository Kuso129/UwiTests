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
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

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

            btnLogin.Enabled = false;
            btnLogin.Text = "Вход...";

            try
            {
                bool success = await Task.Run(() => _authService.Login(login, password));

                if (success)
                {
                    var currentUser = _authService.GetCurrentUser();
                    var userRole = currentUser?.Role ?? "User";

                    MessageBox.Show($"Вход выполнен успешно!\nРоль: {userRole}", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Здесь переход на главное окно с тестами
                    // _mainForm.ShowTestsForm();

                    // Очистка полей
                    txtLogin.Clear();
                    txtPassword.Clear();
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
                btnLogin.Enabled = true;
                btnLogin.Text = "Войти";
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _mainForm.ShowStartForm();
        }
    }
}