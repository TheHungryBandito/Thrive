using Newtonsoft.Json;

/// <summary>
///   Definition for the day night cycle
/// </summary>
/// <remarks>
///   <para>
///     Values for the DayNightCycle, as given in day_night_cycle.json
///   </para>
/// </remarks>
public class DayNightConfiguration : IRegistryType
{
    public string InternalName { get; set; } = null!;

    [JsonProperty]
    public float HoursPerDay { get; private set; }

    /// <summary>
    ///   This is how long it takes to complete a full day in realtime seconds
    /// </summary>
    [JsonProperty]
    public float RealTimePerDay { get; private set; }

    /// <summary>
    ///   This controls the amount of the day that has sunlight 
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     This is based on the equation. Test it in the desmos link in DayNightCycle before changing.
    ///   </para>
    /// </remarks>
    [JsonProperty]
    public float DaytimeDaylightLen { get; private set; }

    public void Check(string name)
    {
    }

    public void ApplyTranslations()
    {
    }
}
