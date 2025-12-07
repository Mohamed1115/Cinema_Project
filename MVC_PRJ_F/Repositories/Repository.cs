using Microsoft.EntityFrameworkCore;
using MVC_PRJ_F.Data;
using MVC_PRJ_F.IRepositories;
using MVC_PRJ_F.Models;

namespace MVC_PRJ_F.Repositories;

public class Repository<T> : IRepository<T> where T : class 
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<T> CreatAsync(T entity, CancellationToken cn = default)
    {
        await _context.Set<T>().AddAsync(entity,cn);
        await  _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cn = default)
    {
        return await _context.Set<T>().ToListAsync(cn);
    }

    public async Task<T> GetByIdAsync(int? id, CancellationToken cn = default)
    {
        return await _context.Set<T>().FindAsync(id, cn);  // ← Fixed: id first, then cn
    }
    
    public async Task<bool> IsOtpExpiredAsync(string email, string code)
    {
        var otp = await _context.Otps
            .Where(o => o.Email == email && o.OtpCode == code)
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync();

        if (otp == null)
            return false; 

        return otp.Expiration > DateTime.UtcNow;
    }
    public async Task<Otp?> GetOtpAsync(string email, string code)
    {
        var otp = await _context.Otps
            .Where(o => o.Email == email && o.OtpCode == code)
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync();

        if (otp == null)
            return null; 

        return otp;
    }



    public async Task UpdateAsync(T entity, CancellationToken cn = default)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        
    }

    public async Task DeleteAsync(int id, CancellationToken cn = default)
    {
        var entity = await GetByIdAsync(id, cn);
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
       
    }
    
    // public async Task<T> 
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}