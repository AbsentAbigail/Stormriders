#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.Scriptables.CardScripts;

#endregion

namespace Stormriders.Builders.Upgrades;

[UsedImplicitly]
public class SwashbucklerCharm : IUpgradeBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<CardUpgradeData, CardUpgradeDataBuilder> Builder()
    {
        return new CardUpgradeDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithType(CardUpgradeData.Type.Charm)
            .WithImage(Stormriders.GetSprite("Charm_-_Swashbuckler"))
            .WithTitle("Swashbuckler Charm")
            .WithText($"Remove {Stormriders.KeywordTag("captain")}, then increase <keyword=attack> or {Stormriders.KeywordTag("thunder")} by <3>")
            .SubscribeToAfterAllBuildEvent(charm =>
            {
                charm.scripts =
                [
                    new Script<SwashbucklerScript>()
                ];
                charm.targetConstraints =
                [
                    TargetConstraintHelper.HasTrait(Captain.Name),
                    TargetConstraintHelper.AttackOrThunder(),
                ];
            });
    }
}