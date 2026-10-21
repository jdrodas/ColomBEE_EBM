using ColomBEE_CSharp_NoSQL.API.Models;

namespace ColomBEE_CSharp_NoSQL.API.Interfaces;

public interface ITipoSensorRepository
{
    public Task<List<TipoSensor>> GetAllAsync();

    public Task<TipoSensor> GetByIdAsync(string tipoSensorId);

    public Task<List<Sensor>> GetAssociatedSensorsAsync(string tipoSensorId);
    public Task<TipoSensor> GetByDetailsAsync(TipoSensor unTipoSensor);
    public Task<long> GetTotalAssociatedSensorsAsync(string tipoSensorId);
    public Task<bool> CreateAsync(TipoSensor unTipoSensor);
    public Task<bool> UpdateAsync(TipoSensor unTipoSensor);
    public Task<bool> RemoveAsync(string tipoSensorId);
}