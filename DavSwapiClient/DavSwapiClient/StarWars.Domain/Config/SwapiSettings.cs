namespace StarWars.Domain.Config;

public class SwapiSettings
{
    public string? BaseUrl { get; set; }
    public int TimeoutSeconds { get; set; }
    public int RetryCount { get; set; }
    public int RetryDelaySeconds { get; set; }
}
