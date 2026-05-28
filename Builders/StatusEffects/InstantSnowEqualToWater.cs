#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantSnowEqualToWater : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXInstant>(Name)
            .WithText($"Apply <keyword=snow> equal to the target's {Stormriders.KeywordTag("water")}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXInstant>(status =>
            {
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.effectToApply = Stormriders.GetStatus("Snow");
                status.scriptableAmount = new Script<ScriptableCurrentStatus>("Current Water",
                    script => script.statusType = "water");
            });
    }
}