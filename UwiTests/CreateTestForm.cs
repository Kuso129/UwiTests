using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UwiTests.Model;
using UwiTests.Services;

namespace UwiTests
{
    public partial class CreateTestForm : Form
    {
        private readonly MainForm _mainForm;
        private readonly AuthService _authService;

        private TextBox _txtTestName;
        private Button _btnAddQuestion;
        private Button _btnSaveTest;
        private Button _btnBack;
        private FlowLayoutPanel _questionsPanel;

        private List<Question> _questions = new List<Question>();
        private int _questionCounter = 0;

        public CreateTestForm(MainForm mainForm, AuthService authService)
        {
            InitializeComponent();
            _mainForm = mainForm;
            _authService = authService;
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = Color.FromArgb(163, 205, 233);
            this.AutoScroll = true;

            var title = new Label
            {
                Text = "Создание нового теста",
                Font = new Font("Mistral", 32),
                ForeColor = Color.FromArgb(225, 91, 149),
                Location = new Point(30, 20),
                AutoSize = true
            };

            var lblTestName = new Label
            {
                Text = "Название теста:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(30, 90),
                AutoSize = true
            };

            _txtTestName = new TextBox
            {
                Location = new Point(30, 115),
                Size = new Size(300, 25),
                Font = new Font("Arial", 11)
            };

            _btnAddQuestion = new Button
            {
                Text = "+ Добавить вопрос",
                BackColor = Color.FromArgb(100, 200, 200),
                Size = new Size(150, 35),
                Location = new Point(30, 160),
                FlatStyle = FlatStyle.Flat
            };
            _btnAddQuestion.Click += BtnAddQuestion_Click;

            _questionsPanel = new FlowLayoutPanel
            {
                Location = new Point(30, 210),
                Size = new Size(this.Width - 60, 300),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            _btnSaveTest = new Button
            {
                Text = "Сохранить тест",
                BackColor = Color.FromArgb(100, 200, 100),
                Size = new Size(150, 40),
                Location = new Point(30, this.Height - 80),
                FlatStyle = FlatStyle.Flat
            };
            _btnSaveTest.Click += BtnSaveTest_Click;

            _btnBack = new Button
            {
                Text = "Отмена",
                BackColor = Color.FromArgb(255, 150, 150),
                Size = new Size(120, 40),
                Location = new Point(200, this.Height - 80),
                FlatStyle = FlatStyle.Flat
            };
            _btnBack.Click += (s, e) => _mainForm.ShowTestsMainForm();

            this.Controls.Add(title);
            this.Controls.Add(lblTestName);
            this.Controls.Add(_txtTestName);
            this.Controls.Add(_btnAddQuestion);
            this.Controls.Add(_questionsPanel);
            this.Controls.Add(_btnSaveTest);
            this.Controls.Add(_btnBack);

            this.Resize += (s, e) =>
            {
                _questionsPanel.Size = new Size(this.Width - 60, this.Height - 310);
                _btnSaveTest.Location = new Point(30, this.Height - 80);
                _btnBack.Location = new Point(200, this.Height - 80);
            };
        }

        private void BtnAddQuestion_Click(object sender, EventArgs e)
        {
            var questionPanel = CreateQuestionEditor(_questionCounter++);
            _questionsPanel.Controls.Add(questionPanel);
        }

        private Panel CreateQuestionEditor(int index)
        {
            var panel = new Panel
            {
                Width = _questionsPanel.Width - 25,
                Height = 180,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5),
                Tag = index
            };

            var lblQ = new Label
            {
                Text = $"Вопрос {index + 1}:",
                Location = new Point(5, 5),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            var txtQuestion = new TextBox
            {
                Location = new Point(5, 25),
                Width = panel.Width - 120,
                Height = 50,
                Multiline = true
            };

            var lblAnswers = new Label
            {
                Text = "Варианты ответов (каждый с новой строки):",
                Location = new Point(5, 85),
                AutoSize = true
            };

            var txtAnswers = new TextBox
            {
                Location = new Point(5, 105),
                Width = panel.Width - 120,
                Height = 60,
                Multiline = true
            };

            var lblCorrect = new Label
            {
                Text = "Номер правильного ответа (1-4):",
                Location = new Point(5, 170),
                AutoSize = true
            };

            var numCorrect = new NumericUpDown
            {
                Location = new Point(180, 168),
                Minimum = 1,
                Maximum = 4,
                Width = 50
            };

            var btnRemove = new Button
            {
                Text = "X",
                BackColor = Color.LightCoral,
                Size = new Size(30, 30),
                Location = new Point(panel.Width - 40, 5),
                FlatStyle = FlatStyle.Flat
            };
            btnRemove.Click += (s, e) =>
            {
                _questionsPanel.Controls.Remove(panel);
                _questions.RemoveAll(q => q.QuestionText == txtQuestion.Text);
            };

            panel.Controls.Add(lblQ);
            panel.Controls.Add(txtQuestion);
            panel.Controls.Add(lblAnswers);
            panel.Controls.Add(txtAnswers);
            panel.Controls.Add(lblCorrect);
            panel.Controls.Add(numCorrect);
            panel.Controls.Add(btnRemove);

            panel.Resize += (sender, args) =>
            {
                txtQuestion.Width = panel.Width - 120;
                txtAnswers.Width = panel.Width - 120;
                btnRemove.Location = new Point(panel.Width - 40, 5);
            };

            // Сохранение вопроса
            panel.Tag = new { txtQuestion, txtAnswers, numCorrect };

            return panel;
        }

        private async void BtnSaveTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTestName.Text))
            {
                MessageBox.Show("Введите название теста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_questionsPanel.Controls.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один вопрос", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Собираем вопросы
            var questions = new List<Question>();
            foreach (Panel panel in _questionsPanel.Controls)
            {
                var txtQuestion = panel.Controls[1] as TextBox;
                var txtAnswers = panel.Controls[3] as TextBox;
                var numCorrect = panel.Controls[5] as NumericUpDown;

                if (string.IsNullOrWhiteSpace(txtQuestion?.Text))
                {
                    MessageBox.Show("Заполните текст всех вопросов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var answersList = txtAnswers?.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (answersList == null || answersList.Length < 2)
                {
                    MessageBox.Show("У каждого вопроса должно быть минимум 2 варианта ответа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var question = new Question
                {
                    QuestionText = txtQuestion.Text,
                    Answers = new List<string>(answersList),
                    CorrectAnswerID = (int)numCorrect.Value - 1
                };
                questions.Add(question);
            }

            // Сохранение теста
            _btnSaveTest.Enabled = false;
            _btnSaveTest.Text = "Сохранение...";

            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    MessageBox.Show("Пользователь не авторизован", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var testData = new TestData
                {
                    TestId = 0,
                    TestName = _txtTestName.Text,
                    CreatorId = currentUser.UserId
                };

                var createdTest = await _authService.CreateTest(testData);
                if (createdTest != null)
                {
                    foreach (var q in questions)
                    {
                        q.TestId = createdTest.TestId;
                        await _authService.AddQuestion(q);
                    }

                    MessageBox.Show($"Тест \"{_txtTestName.Text}\" успешно создан с {questions.Count} вопросами!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _mainForm.ShowTestsMainForm();
                    // Обновим список тестов в главной форме
                    if (_mainForm.Controls.Find("TestsMainForm", true)[0] is TestsMainForm testsForm)
                        testsForm.RefreshTests();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _btnSaveTest.Enabled = true;
                _btnSaveTest.Text = "Сохранить тест";
            }
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Text = "Создание теста";
        }
    }
}
