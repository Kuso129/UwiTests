using System;
using System.Windows.Forms;
using UwiTests.Services;

namespace UwiTests
{
    public partial class MainForm : Form
    {
        private Form _currentForm;
        private Start _startForm;
        private Login _loginForm;
        private Registrate _registrateForm;
        private TestsMainForm _testsMainForm;
        private AccountInfoForm _accountInfoForm;
        private CreateTestForm _createTestForm;

        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;

        public MainForm()
        {
            InitializeComponent();
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5145");
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
            _testsMainForm = new TestsMainForm(this, _authService);
            _accountInfoForm = new AccountInfoForm(this, _authService);
            _createTestForm = new CreateTestForm(this, _authService);

            ConfigureForm(_startForm);
            ConfigureForm(_loginForm);
            ConfigureForm(_registrateForm);
            ConfigureForm(_testsMainForm);
            ConfigureForm(_accountInfoForm);
            ConfigureForm(_createTestForm);

            this.Controls.Add(_createTestForm);
            this.Controls.Add(_accountInfoForm);
            this.Controls.Add(_testsMainForm);
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
        public void ShowTestsMainForm() => ShowForm(_testsMainForm);
        public void ShowAccountInfoForm() => ShowForm(_accountInfoForm);
        public void ShowCreateTestForm() => ShowForm(_createTestForm);

        private void ShowForm(Form formToShow)
        {
            if (_currentForm != null)
            {
                _currentForm.Hide();
                _currentForm.Visible = false;
            }

            _currentForm = formToShow;
            _currentForm.Visible = true;
            _currentForm.BringToFront();
        }

        public AuthService GetAuthService() => _authService;

        // ★★★★★ ДОБАВЬТЕ ЭТОТ МЕТОД ★★★★★
        /// <summary>
        /// Выход из аккаунта и возврат на стартовую форму
        /// </summary>
        public void LogoutAndShowStart()
        {
            _authService.Logout();
            ShowStartForm();
        }
    }
}