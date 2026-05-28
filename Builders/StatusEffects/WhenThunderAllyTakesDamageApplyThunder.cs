#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenThunderAllyTakesDamageApplyThunder : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenEnemyTakesDamage>(Name)
            .WithText($"When an ally with {Stormriders.KeywordTag("thunder")} takes damage, increase their {Stormriders.KeywordTag("thunder")} by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenEnemyTakesDamage>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Thunder.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Target;
                status.ally = true;
                status.enemy = false;
                status.unitConstraints =
                [
                    TargetConstraintHelper.HasStatus(Thunder.Name)
                ];
            });
    }
}