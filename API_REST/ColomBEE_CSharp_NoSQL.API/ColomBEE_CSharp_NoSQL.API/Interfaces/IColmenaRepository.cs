using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface IColmenaRepository
{
    public Task<List<Colmena>> GetAllAsync();
    public Task<Colmena> GetByIdAsync(string colmenaId);

    public Task<Colmena> GetByDetailsAsync(Colmena unaColmena);

    public Task<long> GetTotalAssociatedSensorsAsync(string colmenaId);
    public Task<bool> CreateAsync(Colmena unaColmena);
    //public Task<bool> UpdateAsync(Colmena unaColmena);
    public Task<bool> RemoveAsync(string colmenaId);
}