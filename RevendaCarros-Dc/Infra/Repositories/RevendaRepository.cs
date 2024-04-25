using AutoMapper;
using RevendaCarros_Dc.Domain.Context;
using RevendaCarros_Dc.Domain.Model;
using RevendaCarros_Dc.Infra.Repositories.Interfaces;

namespace RevendaCarros_Dc.Infra.Repositories;

public class RevendaRepository : BaseRepository<Carro>, IRevendaRepository
{
    public RevendaRepository(RevendaCarrosDbContext context) : base(context)
    {
    }
}
