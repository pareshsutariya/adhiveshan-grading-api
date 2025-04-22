namespace AdhiveshanGrading.Services;

public interface ICentersService
{
    Task<List<CenterModel>> Get();
    
}

public class CentersService : BaseService, ICentersService
{
    private readonly IMongoCollection<Center> _CentersCollection;

    public CentersService(IAdvGradingSettings settings, IMapper mapper) : base(settings, mapper)
    {
        _CentersCollection = Database.GetCollection<Center>(settings.CentersCollectionName);
    }

    public async Task<List<CenterModel>> Get()
    {
        var entities = await _CentersCollection.Find(item => true).ToListAsync();

        var models = entities.Select(c => c.Map<CenterModel>(mapper)).OrderBy(c => c.CenterName).ToList();

        return models;
    }
}
