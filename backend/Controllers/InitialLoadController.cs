using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WCAGQuizer.Data;
using WCAGQuizer.Models;

namespace WCAGQuizer.Controllers;

[ApiController]
[Route("api/dev-load")]
public class InitialLoadController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Load([FromBody] JsonElement json)
    {
      

        // Deserialize to a dynamic doc
        var doc = JsonSerializer.Deserialize<JsonDoc>(json.GetRawText(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (doc is null) return BadRequest("Invalid JSON");

        foreach (var c in doc.Criteria)
        {
            if (await db.Criteria.AnyAsync(x => x.Code == c.Code)) continue;

            var criterion = new Criterion
            {
                Code = c.Code,
                Name = c.Name,
                Level = c.Level,
                Description = c.Description
            };

            if (c.BestPractices != null)
                foreach (var b in c.BestPractices)
                    criterion.BestPractices.Add(new BestPractice { Kind = b.Kind, Title = b.Title, Body = b.Body });

            if (c.Snippets != null)
                foreach (var s in c.Snippets)
                    criterion.CodeSnippets.Add(new CodeSnippet { Language = s.Language, IsAntiPattern = s.IsAntiPattern, Content = s.Content });

            if (c.Quiz != null)
                foreach (var q in c.Quiz)
                {
                    var qq = new Question { Type = q.Type, Text = q.Text, Explanation = q.Explanation };
                    if (q.Options != null)
                        foreach (var o in q.Options)
                            qq.Options.Add(new AnswerOption { Text = o.Text, IsCorrect = o.IsCorrect });
                    criterion.QuizQuestions.Add(qq);
                }

            db.Criteria.Add(criterion);
        }

        await db.SaveChangesAsync();
        return Ok(new { message = "Loaded JSON", criteriaCount = db.Criteria.Count() });
    }

    // minimal DTOs just for JSON import
    public class JsonDoc { public int Version { get; set; } public List<JsonCriterion> Criteria { get; set; } = new(); }
    public class JsonCriterion
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Level { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<JsonBP>? BestPractices { get; set; }
        public List<JsonSnippet>? Snippets { get; set; }
        public List<JsonQuiz>? Quiz { get; set; }
    }
    public class JsonBP { public string Kind { get; set; } = default!; public string Title { get; set; } = default!; public string Body { get; set; } = default!; }
    public class JsonSnippet { public string Language { get; set; } = default!; public bool IsAntiPattern { get; set; } public string Content { get; set; } = default!; }
    public class JsonQuiz { public string Type { get; set; } = default!; public string Text { get; set; } = default!; public string? Explanation { get; set; } public List<JsonOption>? Options { get; set; } }
    public class JsonOption { public string Text { get; set; } = default!; public bool IsCorrect { get; set; } }
}
