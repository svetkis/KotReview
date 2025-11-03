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
        if (string.IsNullOrWhiteSpace(searchInput))
        {
            throw new NullReferenceException("SearchInput must not be empty");
        }

        searchInput = searchInput.Trim();

        pageSize = pageSize > 0 ? pageSize : 10;
        int skip = pageIndex * pageSize;

        var baseQuery = _db.Books
            .Include(b => b.Authors)
            .Where(b => !b.Blocked);

        IQueryable<Book> filteredQuery;

        if (int.TryParse(searchInput, out int prefix))
        {
            int prefixLength = searchInput.Length;
            int maxIdLength = _db.Books.Max(x => x.Id.ToString().Length);
            int factor = (int)Math.Pow(10, maxIdLength - prefixLength);
            int from = prefix * factor;
            int to = (prefix + 1) * factor;

            filteredQuery = baseQuery
                .Where(b => (b.Id >= from && b.Id < to) || EF.Functions.Like(b.Name, $"%{searchInput}%"))
                .OrderBy(b => !(b.Id >= from && b.Id < to))
                .ThenByDescending(b => b.RegDate);
        }
        else
        {
            filteredQuery = baseQuery
                .Where(b => EF.Functions.Like(b.Name, $"%{searchInput}%"))
                .OrderByDescending(b => b.RegDate);
        }

        var result = await filteredQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(x => new BookQuickSearchDto
            {
                Id = x.Id,
                Name = x.Name,
                Publisher = x.Publisher,
                Authors = x.Authors
                    .Select(a => a.LastName + " " + a.FirstName + " " + a.MiddleName)
                    .ToArray()
            })
            .ToArrayAsync(ct);

        return Ok(result);
    }
}
