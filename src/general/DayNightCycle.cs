using System;
using Godot;
using Newtonsoft.Json;

public class DayNightCycle : Node
{
    public DayNightConfiguration LightCycleConfig = null!;

    /// <summary>
    ///   The current time in hours
    /// </summary>
    [JsonProperty]
    public float Time;

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
    public float PercentOfDayElapsed
    {
        get { return Time / LightCycleConfig.HoursPerDay; }
    }

    /// <summary>
    ///   The percentage of daylight you should get.
    ///   light = max(-(PercentOfDayElapsed - 0.5)^2 * daytimeMultiplier + 1, 0)
    ///   desmos: https://www.desmos.com/calculator/vrrk1bkac2
    /// </summary>
    [JsonIgnore]
    public float DayLightPercentage
    {
        get
        {
            return Math.Max(-(float)Math.Pow(PercentOfDayElapsed - 0.5, 2) * daytimeMultiplier + 1, 0);
        }
    }

    public override void _Ready()
    {
        LightCycleConfig = SimulationParameters.Instance.GetDayNightCycleConfiguration();

        // This converts the percentage in DaytimePercentage to the power of two needed for DayLightPercentage
        daytimeMultiplier = (float)Math.Pow(2, 0.5 / LightCycleConfig.DaytimePercentage);
        // Time = LightCycleConfig.HoursPerDay / 2;
    }

    public override void _Process(float delta)
    {
        Time = (Time + (1 / LightCycleConfig.RealTimePerDay) * LightCycleConfig.HoursPerDay * delta) 
            % LightCycleConfig.HoursPerDay;
    }
}
