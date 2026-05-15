using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedGainHpEqualToWaterOnBoard : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"Gain {Stormriders.VanillaKeywordTag("health")} equal to total {Stormriders.KeywordTag("water")} on board")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Increase Max Health");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.scriptableAmount = new Script<ScriptableStatusOnBoard>("Water on board", script =>
                {
                    script.statusType = "water";
                    script.self = true;
                    script.allies = true;
                    script.enemies = true;
                    script.countStatus = true;
                });
            });
    }
}