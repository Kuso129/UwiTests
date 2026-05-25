using System;
using System.Drawing;
using System.Windows.Forms;
using UwiTests.Model;
using UwiTests.Services;

namespace UwiTests
{
    public partial class AccountInfoForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly AuthService _authService;
        private Label _lblLogin;
        private Label _lblTestsCompleted;
        private Label _lblAvgTime;
        private Button _btnBack;

        public AccountInfoForm(MainForm mainForm, AuthService authService)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _authService = authService;
            SetupUI();
            LoadAccountInfo();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(163, 205, 233);

            var title = new Label
            {
                Text = "Информация об аккаунте",
                Font = new Font("Mistral", 36),
                ForeColor = Color.FromArgb(225, 91, 149),
                Location = new Point(50, 30),
                AutoSize = true
            };

            _lblLogin = new Label
            {
                Font = new Font("Arial", 14),
                Location = new Point(50, 120),
                AutoSize = true
            };

            _lblTestsCompleted = new Label
            {
                Font = new Font("Arial", 14),
                Location = new Point(50, 160),
                AutoSize = true
            };

            _lblAvgTime = new Label
            {
                Font = new Font("Arial", 14),
                Location = new Point(50, 200),
                AutoSize = true
            };

            _btnBack = new Button
            {
                Text = "Назад",
                BackColor = Color.FromArgb(198, 242, 216),
                Size = new Size(120, 40),
                Location = new Point(50, 260),
                FlatStyle = FlatStyle.Flat
            };
            _btnBack.Click += (s, e) => _mainForm.ShowTestsMainForm();

            this.Controls.Add(title);
            this.Controls.Add(_lblLogin);
            this.Controls.Add(_lblTestsCompleted);
            this.Controls.Add(_lblAvgTime);
            this.Controls.Add(_btnBack);
        }

        private async void LoadAccountInfo()
        {
            var user = _authService.GetCurrentUser();
            if (user == null)
            {
                _lblLogin.Text = "Не авторизован";
                _lblTestsCompleted.Text = "Количество пройденных тестов: -";
                _lblAvgTime.Text = "Среднее время: -";
                return;
            }

            _lblLogin.Text = $"Логин: {user.Login}";

            try
            {
                var stats = await _authService.GetUserStatistics(user.UserId);
                if (stats != null)
                {
                    _lblTestsCompleted.Text = $"Количество пройденных тестов: {stats.TestAmount}";
                    _lblAvgTime.Text = $"Среднее время прохождения: {stats.AvgCompletionTime:HH:mm:ss}";
                }
                else
                {
                    _lblTestsCompleted.Text = "Количество пройденных тестов: 0";
                    _lblAvgTime.Text = "Среднее время: нет данных";
                }
            }
            catch
            {
                _lblTestsCompleted.Text = "Количество пройденных тестов: ошибка загрузки";
                _lblAvgTime.Text = "Среднее время: ошибка загрузки";
            }
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Информация об аккаунте";
        }
    }
}
