using System;
using System.Collections.Generic;
using AdhiveshanGrading.Entities;
using MongoDB.Driver;
using AdhiveshanGrading.Settings;
using AutoMapper;
using AdhiveshanGrading.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AdhiveshanGrading.Services;

public interface IConfigurationsService { }

public class ConfigurationsService : BaseService, IConfigurationsService
{
    private readonly IMongoCollection<Configuration> _mongoCollection;

    public ConfigurationsService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _mongoCollection = Database.GetCollection<Configuration>(settings.ConfigurationsCollectionName);
    }

    public async Task<List<ConfigurationModel>> GetAll() => (await _mongoCollection.Find(item => true).SortBy(c => c.Name).ToListAsync())
                                                                    .Select(c => c.Map<ConfigurationModel>(mapper))
                                                                    .ToList();

    public async Task<ConfigurationModel> GetById(int id) => (await _mongoCollection.Find(item => item.ConfigurationId == id).FirstOrDefaultAsync())?.Map<ConfigurationModel>(mapper);

    public async Task<ConfigurationModel> GetByName(string name) => (await _mongoCollection.Find(item => item.Name == name).FirstOrDefaultAsync())?.Map<ConfigurationModel>(mapper);

    public async Task<ConfigurationModel> Create(ConfigurationCreateModel model)
    {
        var maxId = _mongoCollection.Find(c => true).SortByDescending(c => c.ConfigurationId).FirstOrDefault()?.ConfigurationId;
        maxId = maxId.HasValue == false ? 0 : maxId.Value;

        var entity = model.Map<Configuration>(mapper);
        entity.ConfigurationId = (maxId.Value + 1);

        _mongoCollection.InsertOne(entity);

        return entity.Map<ConfigurationModel>(mapper);
    }

    public void Update(ConfigurationUpdateModel model)
    {
        var entity = model.Map<Configuration>(mapper);

        _mongoCollection.ReplaceOne(item => item.ConfigurationId == model.ConfigurationId, entity);
    }

    public void Update(ConfigurationModel model)
    {
        var entity = model.Map<Configuration>(mapper);

        _mongoCollection.ReplaceOne(item => item.ConfigurationId == model.ConfigurationId, entity);
    }
}