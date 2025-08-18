namespace WcagLearner.Models;
public class Criterion
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;   // e.g., "2.4.7"
    public string Name { get; set; } = default!;
    public string Level { get; set; } = "AA";
    public string Description { get; set; } = default!;
    public ICollection<BestPractice> BestPractices { get; set; } = new List<BestPractice>();
    public ICollection<CodeSnippet> CodeSnippets { get; set; } = new List<CodeSnippet>();
    public ICollection<Question> QuizQuestions { get; set; } = new List<Question>();
}
