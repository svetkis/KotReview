using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory
builder.Services.AddDbContext<BookDb>(opt => opt.UseInMemoryDatabase("books"));

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed данных
await SeedDb(app);

app.MapControllers();

// ¬ключаем Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

static async Task SeedDb(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<BookDb>();
        if (!await db.Books.AnyAsync())
        {
            db.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Name = "Clean Code",
                    Publisher = "Prentice Hall",
                    RegDate = DateTime.UtcNow.AddDays(-10),
                    Authors = { new Author { FirstName = "Robert", MiddleName = "C.", LastName = "Martin" } }
                },
                new Book
                {
                    Id = 12,
                    Name = "CLR via C#",
                    Publisher = "Microsoft Press",
                    RegDate = DateTime.UtcNow.AddDays(-20),
                    Authors = { new Author { FirstName = "Jeffrey", MiddleName = "", LastName = "Richter" } }
                },
                new Book
                {
                    Id = 123,
                    Name = "The Pragmatic Programmer",
                    Publisher = "Addison-Wesley",
                    RegDate = DateTime.UtcNow.AddDays(-5),
                    Authors = { new Author { FirstName = "Andrew", MiddleName = "", LastName = "Hunt" },
                            new Author { FirstName = "David", MiddleName = "", LastName = "Thomas" } }
                },
                new Book
                {
                    Id = 9876,
                    Name = "Domain-Driven Design",
                    Publisher = "Addison-Wesley",
                    RegDate = DateTime.UtcNow.AddDays(-1),
                    Authors = { new Author { FirstName = "Eric", MiddleName = "", LastName = "Evans" } }
                }
            );
            await db.SaveChangesAsync();
        }
    }
}