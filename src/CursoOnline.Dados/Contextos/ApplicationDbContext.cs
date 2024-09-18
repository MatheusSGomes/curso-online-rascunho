using CursoOnline.Dominio.Cursos;
using Microsoft.EntityFrameworkCore;

namespace CursoOnline.Dados.Contextos;

public class ApplicationDbContext : DbContext
{
    public DbSet<Curso> Cursos { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public async Task Commit()
    {
        await SaveChangesAsync();
    }
}
