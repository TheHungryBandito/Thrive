using System;
using Godot;

public class DayNightCycle : Node
{
    public DayNightConfiguration LightCycleConfig = null!;

    /// <summary>
    ///   The current time in hours
    /// </summary>
    public float Time = 0.5f;

    public float PercentOfDayElapsed
    {
        get { return Time / LightCycleConfig.HoursPerDay; }
    }

    /// <summary>
    ///   The percentage of daylight you should get.
    ///   light = max(-(PercentOfDayElapsed - 0.5)^2 * daytimeDaylightLen + 1, 0)
    ///   desmos: https://www.desmos.com/calculator/vrrk1bkac2
    /// </summary>
    public float DayLightPercentage
    {
        get 
        {
            return Math.Max(-(float)Math.Pow(PercentOfDayElapsed - 0.5, 2) * LightCycleConfig.DaytimeDaylightLen + 1, 0);
        }
    }

    public override void _Ready()
    {
        LightCycleConfig = SimulationParameters.Instance.GetDayNightCycleConfiguration();
    }

    public override void _Process(float delta)
    {
        Time = (Time + (1 / LightCycleConfig.RealTimePerDay) * LightCycleConfig.HoursPerDay * delta) 
            % LightCycleConfig.HoursPerDay;
    }
}
