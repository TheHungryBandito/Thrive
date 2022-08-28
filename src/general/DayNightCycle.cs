using System;
using Godot;
using Newtonsoft.Json;

[JsonObject(IsReference = true)]
[UseThriveSerializer]
public class DayNightCycle : Node
{
    [JsonProperty]
    public DayNightConfiguration LightCycleConfig = null!;

    /// <summary>
    ///   The multiplier used for calculating DayLightPercentage.
    /// </summary>
    /// <remarks>
    ///   This exists as it only needs to be calculated once and
    ///   the calculation for it is confusing.
    /// </remarks>
    [JsonIgnore]
    private float daytimeMultiplier;

    [JsonIgnore]
    public float AdverageSunlightPercentage { get; private set; }

    /// <summary>
    ///   The current time in hours
    /// </summary>
    [JsonProperty]
    public float Time { get; private set; }

    [JsonIgnore]
    public float PercentOfDayElapsed => Time / LightCycleConfig.HoursPerDay;

    /// <summary>
    ///   The percentage of daylight you should get.
    ///   light = max(-(PercentOfDayElapsed - 0.5)^2 * daytimeMultiplier + 1, 0)
    ///   desmos: https://www.desmos.com/calculator/vrrk1bkac2
    /// </summary>
    [JsonIgnore]
    public float DayLightPercentage =>
        Math.Max(-(float)Math.Pow(PercentOfDayElapsed - 0.5, 2) * daytimeMultiplier + 1, 0);

    public override void _Ready()
    {
        if (LightCycleConfig == null)
        {
            LightCycleConfig = SimulationParameters.Instance.GetDayNightCycleConfiguration();
            Time = LightCycleConfig.HoursPerDay / 2;
        }

        float halfPercentage = LightCycleConfig.DaytimePercentage / 2;

        // This converts the percentage in DaytimePercentage to the power of two needed for DayLightPercentage
        daytimeMultiplier = (float)Math.Pow(2, 1 / halfPercentage);

        AdverageSunlightPercentage = CalculateASP(0.5f + halfPercentage) - CalculateASP(0.5f - halfPercentage);
    }

    public override void _Process(float delta)
    {
        Time = (Time + (1 / LightCycleConfig.RealTimePerDay) * LightCycleConfig.HoursPerDay * delta)
            % LightCycleConfig.HoursPerDay;
    }

    /// <summary>
    ///   Calculates an AdverageSunlightPercentage.
    /// </summary>
    private float CalculateASP(float x)
    {
        return (float)(daytimeMultiplier * (Math.Pow(x, 3) / 3 - Math.Pow(x, 2) / 2 + 0.25 * x) + x);
    }
}
