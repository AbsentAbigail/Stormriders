using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenWaterdEnemyTakesDamageTrigger : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenEnemyTakesDamage>(Name)
            .WithText($"Trigger when a {Stormriders.KeywordTag("water")}'d enemy takes damage")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenEnemyTakesDamage>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Trigger (High Prio)");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.ignoreOwnDamage = true;
                status.unitConstraints =
                [
                    TargetConstraintHelper.HasStatus(Water.Name)
                ];

                status.isReaction = true;
                status.descColorHex = "F99C61";
            });
    }
}