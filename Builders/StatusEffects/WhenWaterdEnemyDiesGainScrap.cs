#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenWaterdEnemyDiesGainScrap : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenUnitIsKilled>(Name)
            .WithText($"When a {Stormriders.KeywordTag("water")}'d enemy dies, gain <{{a}}>{Stormriders.VanillaKeywordTag("scrap")}")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenUnitIsKilled>(status =>
            {
                status.ally = false;
                status.enemy = true;
                
                status.effectToApply = Stormriders.GetStatus("Scrap");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.unitConstraints =
                [
                    TargetConstraintHelper.HasStatus(Water.Name)
                ];
            });
    }
}