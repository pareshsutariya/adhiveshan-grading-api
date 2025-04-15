namespace AdhiveshanGrading.Models;

public class ConfigurationModel : ModelBase
{
    public int ConfigurationId { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public string ValidValues { get; set; }

    public string Status { get; set; }

    public bool HasValidValues
    {
        get
        {
            return this.ValidValues != null && this.ValidValues.Length > 0;
        }
    }

    public List<string> ValidValuesArray
    {
        get
        {
            return this.HasValidValues
                        ? this.ValidValues.Split(",").ToList()
                        : new List<string>();
        }
    }
}

public class ConfigurationCreateModel
{
    public string Name { get; set; }

    public string Value { get; set; }

    public string ValidValues { get; set; }

    public string Status { get; set; }
}

public class ConfigurationUpdateModel : ModelBase
{
    public int ConfigurationId { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public string ValidValues { get; set; }

    public string Status { get; set; }
}