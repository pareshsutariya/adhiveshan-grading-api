using System;

namespace AdhiveshanGrading.Settings;

public class AdvGradingSettings : IAdvGradingSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ParticipantsCollectionName { get; set; }

    public string UsersCollectionName { get; set; } = null!;
    public string ConfigurationsCollectionName { get; set; } = null!;
}

public interface IAdvGradingSettings
{
    string ConnectionString { get; set; }

    string DatabaseName { get; set; }

    string ParticipantsCollectionName { get; set; }
    string UsersCollectionName { get; set; }
    string ConfigurationsCollectionName { get; set; }
}
