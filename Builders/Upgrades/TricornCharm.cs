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
public class TricornCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Square_Tricorn"))
            .WithTitle("Tricorn Charm")
            .WithText($"Gain {Stormriders.KeywordTag("captain")}")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.giveTraits = [Stormriders.TStack(Captain.Name)];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.HasTrait(Captain.Name, not: true),
                    TargetConstraintHelper.IsCardType(["Item"], not: true)
                ];
            });
    }
}