using System.Collections.Generic;  
using System;                    

namespace UwiTests.Client;

public class TestDtoBase   //DTO с общими полями
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
}

//для создания теста
public class CreateTestDto
{
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public int TimeLimitMinutes { get; set; }
	public bool IsPublic { get; set; }
}

//для создания вопроса
public class CreateQuestionDto
{
	public string QuestionText { get; set; } = string.Empty;
	public List<string> Options { get; set; } = new();
	public int CorrectOptionIndex { get; set; }
	public int Points { get; set; }
}

// DTO для тестов пользователя
public class UserTestDto : TestDtoBase
{
	public string Description { get; set; } = string.Empty;
	public int QuestionsCount { get; set; }
	public int TimesTaken { get; set; }
	public bool IsPublished { get; set; }
}

// DTO для прохождения
public class TestForTakingDto : TestDtoBase
{
	public List<QuestionForTakingDto> Questions { get; set; } = new();
	public int TimeLimitMinutes { get; set; }
}

// DTO для отправки ответов
public class SubmitAnswersDto
{
	public int TestId { get; set; }
	public Dictionary<int, int> Answers { get; set; } = new();
	public int TimeSpentSeconds { get; set; }
}

// DTO вопросов при прохождении
public class QuestionForTakingDto
{
	public int Id { get; set; }
	public string QuestionText { get; set; } = string.Empty;
	public List<string> Options { get; set; } = new();
	public int Points { get; set; }
}

// DTO результата теста
public class TestResultDto : TestDtoBase
{
	public int Score { get; set; }
	public int MaxScore { get; set; }
	public double Percentage { get; set; }
	public DateTime CompletedAt { get; set; }
	public Dictionary<int, bool> QuestionResults { get; set; } = new();
}

//DTO для ответа сервера
internal class TestCreateResponse
{
	public int TestId { get; set; }
}

// DTO под токен
public class TokenResponse
{
	public string Token { get; set; } = string.Empty;
}