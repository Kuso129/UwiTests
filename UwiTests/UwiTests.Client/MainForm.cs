
using System;
using System.Net.Http;
using System.Windows.Forms;
using UwiTests.Repositories;
using UwiTests.Services;

namespace UwiTests
{
    public partial class MainForm : Form
    {
        private Form _currentForm;
        private Start _startForm;
        private Login _loginForm;
        private Registrate _registrateForm;

        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public MainForm()
        {
            InitializeComponent();
            var httpClient = new HttpClient();
            _userRepository = new UserRepository(httpClient);
            _authService = new AuthService(_userRepository);

            InitializeForms();
            ShowStartForm();
        }

        private void InitializeForms()
        {
            _startForm = new Start(this);
            _loginForm = new Login(this, _authService);
            _registrateForm = new Registrate(this, _authService);

            ConfigureForm(_startForm);
            ConfigureForm(_loginForm);
            ConfigureForm(_registrateForm);

            // Добавляем в правильном порядке
            this.Controls.Add(_registrateForm);
            this.Controls.Add(_loginForm);
            this.Controls.Add(_startForm);
        }

        private void ConfigureForm(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Visible = false;
        }

        public void ShowStartForm() => ShowForm(_startForm);
        public void ShowLoginForm() => ShowForm(_loginForm);
        public void ShowRegistrateForm() => ShowForm(_registrateForm);

        private void ShowForm(Form formToShow)
        {
            if (_currentForm != null)
            {
                _currentForm.Hide();
                _currentForm.Visible = false;
            }

            _currentForm = formToShow;
            _currentForm.Visible = true;
            _currentForm.Show();
            _currentForm.BringToFront();
        }

        // Доступ к сервису
        public AuthService GetAuthService() => _authService;
    }
}