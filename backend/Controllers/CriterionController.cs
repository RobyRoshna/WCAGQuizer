using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WCAGQuizer.Data;
using WCAGQuizer.Models;

namespace WCAGQuizer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CriteriaController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await db.Criteria
            .AsNoTracking()
            .Select(c => new { c.Id, c.Code, c.Name, c.Level, c.Description })
            .OrderBy(c => c.Code).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var c = await db.Criteria
            .Include(x => x.BestPractices)
            .Include(x => x.CodeSnippets)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return c is null ? NotFound() : Ok(c);
    }

    [HttpGet("{id:int}/quiz")]
    public async Task<IActionResult> GetQuiz(int id)
    {
        var q = await db.QuizQuestions
            .Where(q => q.CriterionId == id)
            .Include(x => x.Options)
            .AsNoTracking().ToListAsync();
        return Ok(q);
    }
}

