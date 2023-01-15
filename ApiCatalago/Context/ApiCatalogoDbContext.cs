using ApiCatalago.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalago.Context;

public class ApiCatalogoDbContext : DbContext
{
    public ApiCatalogoDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
}
