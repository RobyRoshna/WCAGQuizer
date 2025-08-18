namespace WcagLearner.Models;
public class CodeSnippet
{
    public int Id { get; set; }
    public int CriterionId { get; set; }
    public Criterion Criterion { get; set; } = default!;
    public string Language { get; set; } = "html";
    public bool IsAntiPattern { get; set; }
    public string Content { get; set; } = default!;
}
