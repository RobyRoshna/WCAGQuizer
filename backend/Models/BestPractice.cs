namespace WCAGQuizer.Models;
public class BestPractice
{
    public int Id { get; set; }
    public int CriterionId { get; set; }
    public Criterion Criterion { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Kind { get; set; } = "best-practice";
    public string Body { get; set; } = default!;
}