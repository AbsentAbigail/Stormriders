#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.Upgrades;

[UsedImplicitly]
public class DawnCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Charm_-_Dawn"))
            .WithTitle("Dawn Charm")
            .WithText($"Gain \"When deployed, removed <keyword=consume> from cards in hand\"")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.effects = [Stormriders.SStack(WhenDeployedRemoveConsumeFromCardsInHand.Name)];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.IsCardType(["Item"], not: true)
                ];
            });
    }
}