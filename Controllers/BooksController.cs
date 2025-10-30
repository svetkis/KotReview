using KotReview.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KotReview.Controllers;

[ApiController]
[Route("books")]
public sealed class BooksController : ControllerBase
{
    private readonly BookDb _db;

    public BooksController(BookDb db) => _db = db;

    // GET /books?searchInput=...&pageIndex=0&pageSize=10
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string searchInput,
        [FromQuery] int pageIndex,
        [FromQuery] int pageSize,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
