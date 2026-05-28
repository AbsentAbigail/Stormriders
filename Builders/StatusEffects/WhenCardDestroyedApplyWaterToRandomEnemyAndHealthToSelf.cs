#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenCardDestroyedApplyWaterToRandomEnemyAndHealthToSelf : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenCardDestroyed>(Name)
            .WithText($"When a card is destroyed, apply <{{a}}>{Stormriders.KeywordTag("water")} to a random enemy and <{{a}}><keyword=health> to self")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenCardDestroyed>(status =>
            {
                status.targetMustBeAlive = false;
                status.mustBeOnBoard = false;
                status.effectToApply = Stormriders.GetStatus(InstantIncreaseHealthToSelfAndWaterRandomEnemy.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            });
    }
}