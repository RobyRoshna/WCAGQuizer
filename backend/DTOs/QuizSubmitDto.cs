namespace WCAGQuizer.DTOs;
public class QuizSubmitDto
{
    public int CriterionId { get; set; }
    public List<QuizSubmitAnswerDto> Answers { get; set; } = new();
}

public class QuizSubmitAnswerDto
{
    public int QuestionId { get; set; }
    public List<int>? SelectedOptionIds { get; set; }
    public string? TextAns { get; set; }
}
