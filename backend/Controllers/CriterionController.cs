using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WCAGQuizer.Data;
using WCAGQuizer.DTOs;

namespace WCAGQuizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CriteriaController(AppDbContext db) : ControllerBase
{
    // GET /api/criteria
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CriterionListItemDto>>> GetAll()
    {
        var list = await db.Criteria
            .AsNoTracking()
            .OrderBy(c => c.Code)
            .Select(c => new CriterionListItemDto(
                c.Id, c.Code, c.Name, c.Level, c.Description
            ))
            .ToListAsync();

        return Ok(list);
    }

    // GET /api/criteria/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CriterionDetailDto>> GetOne(int id)
    {
        var dto = await db.Criteria
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CriterionDetailDto(
                c.Id, c.Code, c.Name, c.Level, c.Description,
                c.BestPractices
                    .OrderBy(bp => bp.Id)
                    .Select(bp => new BestPracticeDto(bp.Id, bp.Title, bp.Kind, bp.Body))
                    .ToList(),
                c.CodeSnippets
                    .OrderBy(cs => cs.Id)
                    .Select(cs => new CodeSnippetDto(cs.Id, cs.Language, cs.IsAntiPattern, cs.Content))
                    .ToList()
            ))
            .FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }

    // GET /api/criteria/{id}/quiz
    [HttpGet("{id:int}/quiz")]
    public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetQuiz(int id)
    {
        // Return only what the client needs; no IsCorrect.
        var quiz = await db.QuizQuestions
            .AsNoTracking()
            .Where(q => q.CriterionId == id)
            .OrderBy(q => q.Id)
            .Select(q => new QuizQuestionDto(
                q.Id,
                q.Text,
                q.Type,
                q.Explanation,
                q.Options
                    .OrderBy(o => o.Id)
                    .Select(o => new QuizAnswerOptionDto(o.Id, o.Text))
                    .ToList()
            ))
            .ToListAsync();

        if (quiz.Count == 0 && !await db.Criteria.AnyAsync(c => c.Id == id))
            return NotFound();

        return Ok(quiz);
    }
}
