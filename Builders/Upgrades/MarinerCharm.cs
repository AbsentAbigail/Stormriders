#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.Upgrades;

[UsedImplicitly]
public class MarinerCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Square_Mariner"))
            .WithTitle("Mariner Charm")
            .WithText($"Apply <1>{Stormriders.KeywordTag("water")} for each {Stormriders.KeywordTag("captain")} in play")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.becomesTargetedCard = true;
                charm.effects = [Stormriders.SStack(OnCardPlayedApplyWaterForEachCaptain.Name)];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.ApplyCharmConstraint(),
                ];
            });
    }
}