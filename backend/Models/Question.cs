
namespace WcagLearner.Models;
public class Question
{
    public int Id { get; set; }
    public int CriterionId { get; set; }
    public Criterion Criterion { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string Type { get; set; } = "single"; // single | multi | fill
    public string? Explanation { get; set; }
    public ICollection<AnswerOption> Options { get; set; } = new List<AnswerOption>();
}