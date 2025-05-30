﻿using System;

namespace AdhiveshanGrading.Settings;

public class AdvGradingSettings : IAdvGradingSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ParticipantsCollectionName { get; set; }
    public string UsersCollectionName { get; set; } = null!;
    public string ConfigurationsCollectionName { get; set; } = null!;
    public string CompetitionEventsCollectionName { get; set; } = null!;
    public string EventCheckInCollectionName { get; set; } = null!;
    public string EventSchedulesCollectionName { get; set; } = null!;
    public string GradingCriteriasCollectionName { get; set; } = null!;
    public string GradesCollectionName { get; set; } = null!;
    public string SkillCategoriesCollectionName { get; set; } = null!;
}

public interface IAdvGradingSettings
{
    string ConnectionString { get; set; }

    string DatabaseName { get; set; }

    string ParticipantsCollectionName { get; set; }
    string UsersCollectionName { get; set; }
    string ConfigurationsCollectionName { get; set; }
    string CompetitionEventsCollectionName { get; set; }
    string EventCheckInCollectionName { get; set; }
    string EventSchedulesCollectionName { get; set; }
    string GradingCriteriasCollectionName { get; set; }
    string GradesCollectionName { get; set; }
    string SkillCategoriesCollectionName { get; set; }
}
