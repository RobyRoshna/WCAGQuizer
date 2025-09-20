using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WCAGQuizer.Data;
using WCAGQuizer.DTOs;
using WCAGQuizer.Models;

namespace WCAGQuizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController(AppDbContext db) : ControllerBase
{
    // POST /api/quiz/submit
    [HttpPost("submit")]
    public async Task<ActionResult<QuizResultDto>> Submit([FromBody] QuizSubmitDto payload)
    {
        // Fetch the involved questions (type + explanation + criterion)
        var qIds = payload.Answers.Select(a => a.QuestionId).Distinct().ToList();

        var questions = await db.QuizQuestions
            .AsNoTracking()
            .Where(q => qIds.Contains(q.Id))
            .Select(q => new { q.Id, q.CriterionId, q.Type, q.Explanation })
            .ToListAsync();

        if (questions.Count != qIds.Count)
            return BadRequest("One or more question IDs are invalid.");

        // Ensure all questions belong to the submitted criterion
        if (questions.Any(q => q.CriterionId != payload.CriterionId))
            return BadRequest("Submitted answers contain questions from a different criterion.");

        // Map of correct option IDs per question
        var correct = await db.AnswerOptions
            .AsNoTracking()
            .Where(a => qIds.Contains(a.QuizQuestionId) && a.IsCorrect)
            .GroupBy(a => a.QuizQuestionId)
            .Select(g => new { QuestionId = g.Key, CorrectIds = g.Select(a => a.Id).ToList() })
            .ToListAsync();

        var correctMap = correct.ToDictionary(x => x.QuestionId, x => x.CorrectIds);

        int correctCount = 0;
        var details = new List<QuizFeedbackDto>();

        foreach (var ans in payload.Answers)
        {
            var meta = questions.First(q => q.Id == ans.QuestionId);
            var selected = (ans.SelectedOptionIds ?? new()).Distinct().OrderBy(x => x).ToList();
            var correctIds = correctMap.TryGetValue(ans.QuestionId, out var ids)
                ? ids.OrderBy(x => x).ToList()
                : new List<int>();

            bool isCorrect = meta.Type switch
            {
                "single" => selected.Count == 1 && correctIds.Count == 1 && selected[0] == correctIds[0],
                "multi" => selected.SequenceEqual(correctIds),
                "fill" => false, // optional: implement text grading against AnswerOption.Text
                _ => false
            };

            if (isCorrect) correctCount++;

            details.Add(new QuizFeedbackDto(
                QuestionId: ans.QuestionId,
                SelectedOptionIds: selected,
                CorrectOptionIds: correctIds,
                IsCorrect: isCorrect,
                Explanation: meta.Explanation
            ));
        }

        var result = new QuizResultDto(
            CriterionId: payload.CriterionId,
            TotalQuestions: payload.Answers.Count,
            Correct: correctCount,
            Details: details
        );

        return Ok(result);
    }
}
