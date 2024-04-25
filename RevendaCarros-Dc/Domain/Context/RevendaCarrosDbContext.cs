using Microsoft.EntityFrameworkCore;
using RevendaCarros_Dc.Domain.Model;

namespace RevendaCarros_Dc.Domain.Context;

public class RevendaCarrosDbContext : DbContext
{

    public DbSet<Carro> Carros { get; set; }
    public RevendaCarrosDbContext(DbContextOptions<RevendaCarrosDbContext> options)
     : base(options)
    {
    }
}
