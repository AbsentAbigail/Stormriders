using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenDeployedRemoveConsumeFromCardsInHand : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenDeployed>(Name)
            .WithText($"When deployed, remove <keyword=consume> from cards in hand")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenDeployed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantRemoveConsume.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Hand;
            });
    }
}