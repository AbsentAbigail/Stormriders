#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.Scriptables.CardScripts;

#endregion

namespace Stormriders.Builders.Upgrades;

[UsedImplicitly]
public class DischargeCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Square_Discharge"))
            .WithTitle("Discharge Charm")
            .WithText($"Reduce <keyword=attack> by <2>, then convert <keyword=attack> to {Stormriders.KeywordTag("thunder")}")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.damage = -2;
                charm.scripts =
                [
                    new Script<DischargeScript>()
                ];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.AttackMoreThan(2)
                ];
            });
    }
}