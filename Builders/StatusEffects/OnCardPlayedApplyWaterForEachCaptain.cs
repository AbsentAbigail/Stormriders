#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedApplyWaterForEachCaptain : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayedBoostableScriptable>(Name)
            .WithText($"Apply <{{a}}>{Stormriders.KeywordTag("water")} for each {Stormriders.KeywordTag("captain")} on board")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayedBoostableScriptable>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Water.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Target;
                status.scriptableAmount = new Script<ScriptableTraitsOnBoard>("Captains on board", script =>
                {
                    script.trait = Stormriders.GetTrait(Captain.Name);
                    script.self = true;
                    script.allies = true;
                    script.enemies = true;
                });
            });
    }
}