namespace WCAGQuizer.Models;
public class AnswerOption
{
    public int Id { get; set; }
    public int QuizQuestionId { get; set; }
    public Question QuizQuestion { get; set; } = default!;
    public string Text { get; set; } = default!;
    public bool IsCorrect { get; set; }
}