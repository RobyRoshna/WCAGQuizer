using Microsoft.EntityFrameworkCore;
using WCAGQuizer.Models;

namespace WCAGQuizer.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Criterion> Criteria => Set<Criterion>();
    public DbSet<BestPractice> BestPractices => Set<BestPractice>();
    public DbSet<CodeSnippet> CodeSnippets => Set<CodeSnippet>();
    public DbSet<Question> QuizQuestions => Set<Question>();
    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
}
