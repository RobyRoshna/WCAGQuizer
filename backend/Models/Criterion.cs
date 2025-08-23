namespace WCAGQuizer.Models;
public class Criterion
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;   // EF will set it later nullable warning repressed
    public string Name { get; set; } = default!;
    public string Level { get; set; } = "AA";
    public string Description { get; set; } = default!;
    public ICollection<BestPractice> BestPractices { get; set; } = new List<BestPractice>();
    public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    public ICollection<Question> QuizQuestions { get; set; } = new List<Question>();
}
