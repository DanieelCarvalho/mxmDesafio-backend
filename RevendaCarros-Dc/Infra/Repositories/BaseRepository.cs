using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevendaCarros_Dc.Domain.Context;
using RevendaCarros_Dc.Domain.Model;
using RevendaCarros_Dc.Infra.Repositories.Interfaces;

namespace RevendaCarros_Dc.Infra.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : Entity
{
    private readonly RevendaCarrosDbContext _context;
   
    protected BaseRepository(RevendaCarrosDbContext context)
    {
        _context = context;
      

    }

    public async Task<bool> DeleteById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> FindAll()
    {
       
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> FindById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Save(T entity)
    {

        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update( T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
}
