using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WCAGQuizer.Data;
using WCAGQuizer.Models;

namespace WCAGQuizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController(AppDbContext db) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] QuizSubmitDto payload)
    {
        var questions = await db.QuizQuestions
            .Include(q => q.Options)
            .Where(q => q.CriterionId == payload.CriterionId)
            .ToListAsync();

        int score = 0;
        var feedback = new List<object>();

        foreach (var ans in payload.Answers)
        {
            var question = questions.First(q => q.Id == ans.QuestionId);
            bool correct = false;

            if (question.Type == "fill")
            {
                // Test!!!!!!!: does free Text contain "alt"
                correct = !string.IsNullOrWhiteSpace(ans.TextAns) &&
                          ans.TextAns.Contains("alt", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                var correctIds = question.Options.Where(o => o.IsCorrect)
                                                 .Select(o => o.Id)
                                                 .ToHashSet();
                var givenIds = ans.SelectedOptionIds?.ToHashSet() ?? new HashSet<int>();
                correct = correctIds.SetEquals(givenIds);
            }

            if (correct) score++;

            feedback.Add(new
            {
                questionId = question.Id,
                correct,
                explanation = question.Explanation
            });
        }

        return Ok(new
        {
            total = questions.Count,
            score,
            feedback
        });
    }
}
