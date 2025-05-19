
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

    public BaseService(IAdvGradingSettings settings, IMapper mapper, IWebHostEnvironment hostingEnvironment)
    {
        MongoClient client;
        if (hostingEnvironment.IsDevelopment())
        {
            client = new MongoClient(settings.ConnectionString);
            this.database = client.GetDatabase(settings.DatabaseName);
        }
        else
        {
            client = new MongoClient(settings.ConnectionString.Replace("DATABASE_CONNECTION_STRING", Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING")));
            this.database = client.GetDatabase(settings.DatabaseName.Replace("DATABASE_NAME", Environment.GetEnvironmentVariable("DATABASE_NAME")));
        }
        this.mapper = mapper;
        this.settings = settings;
    }

    public IMongoDatabase Database => this.database;
    public IAdvGradingSettings Settings => this.settings;
}
