#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.Scriptables.ScriptableAmounts;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedIncreaseLowestHealthAllyHealthEqualToWater : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"Increase <keyword=health> to ally with lowest <keyword=health> equal to total {Stormriders.KeywordTag(Keywords.Water.Name)} on board")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Increase Max Health");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                status.selectScript = new Script<SelectScriptEntityLowestHealth>();
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