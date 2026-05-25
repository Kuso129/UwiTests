using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UwiTests.Model;
using UwiTests.Services;

namespace UwiTests
{
    public partial class TestsMainForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly AuthService _authService;
        private FlowLayoutPanel _testsPanel;
        private Button _btnAccountInfo;
        private Button _btnCreateTest;
        private Button _btnLogout;
        private Label _lblWelcome;

        // Конструктор с 2 параметрами
        public TestsMainForm(MainForm mainForm, AuthService authService)
        {
            _mainForm = mainForm;
            _authService = authService;
            InitializeComponent();
            SetupUI();
            LoadTests();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(163, 205, 233);

            _lblWelcome = new Label
            {
                Text = _authService?.GetCurrentUser()?.Login != null
                    ? $"Добро пожаловать, {_authService.GetCurrentUser().Login}!"
                    : "Добро пожаловать!",
                Font = new Font("Mistral", 28),
                ForeColor = Color.FromArgb(225, 91, 149),
                Location = new Point(30, 20),
                AutoSize = true
            };

            _btnAccountInfo = new Button
            {
                Text = "Мой аккаунт",
                BackColor = Color.FromArgb(198, 242, 216),
                Size = new Size(140, 40),
                Location = new Point(30, 80),
                FlatStyle = FlatStyle.Flat
            };
            if (_mainForm != null)
                _btnAccountInfo.Click += (s, e) => _mainForm.ShowAccountInfoForm();

            _btnCreateTest = new Button
            {
                Text = "Создать тест",
                BackColor = Color.FromArgb(198, 242, 216),
                Size = new Size(140, 40),
                Location = new Point(190, 80),
                FlatStyle = FlatStyle.Flat
            };
            if (_mainForm != null)
                _btnCreateTest.Click += (s, e) => _mainForm.ShowCreateTestForm();

            _btnLogout = new Button
            {
                Text = "Выйти",
                BackColor = Color.FromArgb(255, 150, 150),
                Size = new Size(100, 40),
                Location = new Point(this.Width - 130, 20),
                FlatStyle = FlatStyle.Flat
            };
            if (_mainForm != null)
                _btnLogout.Click += (s, e) => _mainForm.LogoutAndShowStart();

            _testsPanel = new FlowLayoutPanel
            {
                Location = new Point(30, 140),
                Size = new Size(this.Width - 60, this.Height - 170),
                AutoScroll = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            this.Controls.Add(_lblWelcome);
            this.Controls.Add(_btnAccountInfo);
            this.Controls.Add(_btnCreateTest);
            this.Controls.Add(_btnLogout);
            this.Controls.Add(_testsPanel);

            this.Resize += (s, e) =>
            {
                _btnLogout.Location = new Point(this.Width - 130, 20);
                _testsPanel.Size = new Size(this.Width - 60, this.Height - 170);
            };
        }

        private async void LoadTests()
        {
            if (_authService == null) return;

            _testsPanel.Controls.Clear();
            var loadingLabel = new Label { Text = "Загрузка тестов...", AutoSize = true };
            _testsPanel.Controls.Add(loadingLabel);

            try
            {
                var tests = await _authService.GetAllTests();
                _testsPanel.Controls.Clear();

                if (tests == null || tests.Count == 0)
                {
                    _testsPanel.Controls.Add(new Label
                    {
                        Text = "Нет доступных тестов",
                        AutoSize = true,
                        Font = new Font("Arial", 12),
                        ForeColor = Color.Gray
                    });
                    return;
                }

                foreach (var test in tests)
                {
                    var testCard = CreateTestCard(test);
                    _testsPanel.Controls.Add(testCard);
                }
            }
            catch (Exception ex)
            {
                _testsPanel.Controls.Clear();
                _testsPanel.Controls.Add(new Label
                {
                    Text = $"Ошибка загрузки: {ex.Message}",
                    ForeColor = Color.Red,
                    AutoSize = true
                });
            }
        }

        private Panel CreateTestCard(TestData test)
        {
            var card = new Panel
            {
                Width = _testsPanel.Width - 25,
                Height = 80,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(240, 248, 255)
            };

            var lblName = new Label
            {
                Text = test.TestName,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var lblCreator = new Label
            {
                Text = $"ID создателя: {test.CreatorId}",
                Location = new Point(10, 35),
                AutoSize = true,
                Font = new Font("Arial", 9),
                ForeColor = Color.Gray
            };

            var btnTake = new Button
            {
                Text = "Пройти",
                BackColor = Color.FromArgb(100, 200, 100),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 40),
                Location = new Point(card.Width - 120, 20)
            };
            btnTake.Click += async (s, e) =>
            {
                MessageBox.Show($"Начинаем тест: {test.TestName}", "Информация");
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblCreator);
            card.Controls.Add(btnTake);

            card.Resize += (sender, args) =>
            {
                btnTake.Location = new Point(card.Width - 120, 20);
                lblName.MaximumSize = new Size(card.Width - 140, 0);
            };

            return card;
        }

        public void RefreshTests() => LoadTests();
    }
}