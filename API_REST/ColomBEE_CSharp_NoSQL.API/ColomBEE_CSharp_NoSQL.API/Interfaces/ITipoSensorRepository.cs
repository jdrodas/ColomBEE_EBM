using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface ITipoSensorRepository
{
    public Task<List<TipoSensor>> GetAllAsync();
    
    public Task<TipoSensor> GetByIdAsync(string tipoSensorId);
    /*
    public Task<List<Sensor>> GetAssociatedSensorsAsync(Guid tipoSensorId);
    public Task<TipoSensor> GetByDetailsAsync(TipoSensor unTipoSensor);
    public Task<long> GetTotalAssociatedSensorsAsync(Guid tipoSensorId);
    public Task<bool> CreateAsync(TipoSensor unTipoSensor);
    public Task<bool> UpdateAsync(TipoSensor unTipoSensor);
    public Task<bool> RemoveAsync(Guid tipoSensorId);
    */
}