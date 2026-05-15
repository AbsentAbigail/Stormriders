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
public class BickernCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Charm_-_Bickern"))
            .WithTitle("Bickern Charm")
            .WithText($"Gain \"{Stormriders.KeywordTag("smelt")} <1> to self and all {Stormriders.KeywordTag("captain")}s\"")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.effects = [Stormriders.SStack(OnCardPlayedSmeltSelfAndCaptains.Name)];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.AttackOrThunder()
                ];
            });
    }
}