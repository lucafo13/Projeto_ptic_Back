using Models.Produto;
using Microsoft.EntityFrameworkCore;

namespace API_PTIC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options){}
        public DbSet<Produtos> produtos { get; set;}   
    }
}