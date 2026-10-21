using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface IApiarioRepository
{
    public Task<List<Apiario>> GetAllAsync();
    public Task<Apiario> GetByIdAsync(string apiarioId);
    public Task<Apiario> GetByNameAsync(string apiarioNombre);
    public Task<List<Colmena>> GetAssociatedBeehivesAsync(string apiarioId);
    public Task<Apiario> GetByDetailsAsync(Apiario unApiario);
    public Task<long> GetTotalAssociatedBeehivesAsync(string apiarioId);
    public Task<bool> CreateAsync(Apiario unApiario);
    public Task<bool> UpdateAsync(Apiario unApiario);
    public Task<bool> UpdateAssociatedBeehivesAsync(string apiarioId);
    public Task<bool> RemoveAsync(string apiarioId);
    }