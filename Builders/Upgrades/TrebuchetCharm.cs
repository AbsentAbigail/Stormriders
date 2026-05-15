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
public class TrebuchetCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Charm_-Trebuchet"))
            .WithTitle("Trebuchet Charm")
            .WithText($"Gain <keyword=longshot> and Apply <3>{Stormriders.KeywordTag("water")}")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.becomesTargetedCard = true;
                charm.attackEffects = [Stormriders.SStack(Water.Name, 3)];
                charm.giveTraits = [Stormriders.TStack("Longshot")];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.ApplyCharmConstraint(),
                ];
            });
    }
}