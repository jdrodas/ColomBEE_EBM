using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface ISensorRepository
{
    public Task<List<Sensor>> GetAllAsync();
    public Task<Sensor> GetByIdAsync(string sensorId);
    /*

    public Task<Sensor> GetByDetailsAsync(Sensor unSensor);
    public Task<long> GetTotalAssociatedReadingsAsync(Guid sensorId);
    public Task<List<Lectura>> GetAssociatedReadingsAsync(Guid sensorId);
    public Task<bool> CreateAsync(Sensor unSensor);
    public Task<bool> UpdateAsync(Sensor unSensor);
    public Task<bool> RemoveAsync(Guid sensorId);
    */
}