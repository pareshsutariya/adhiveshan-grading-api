using System;
using System.Collections.Generic;
using AdhiveshanGrading.Entities;
using MongoDB.Driver;
using AdhiveshanGrading.Settings;
using AutoMapper;

namespace AdhiveshanGrading.Services;

public interface IBaseService { }

public class BaseService : IBaseService
{
    private readonly IMongoDatabase database;
    protected readonly IMapper mapper;
    protected readonly IAdvGradingSettings settings;

    public BaseService(IAdvGradingSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        this.database = client.GetDatabase(settings.DatabaseName);
        this.settings = settings;
    }

    public BaseService(IAdvGradingSettings settings, IMapper mapper)
    {
        var client = new MongoClient(settings.ConnectionString);
        this.database = client.GetDatabase(settings.DatabaseName);
        this.mapper = mapper;
        this.settings = settings;
    }

    public IMongoDatabase Database => this.database;
    public IAdvGradingSettings Settings => this.settings;
}
