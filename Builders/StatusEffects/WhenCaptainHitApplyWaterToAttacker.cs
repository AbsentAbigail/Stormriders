#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenCaptainHitApplyWaterToAttacker : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenAnyoneIsHit>(Name)
            .WithText($"When a {Stormriders.KeywordTag("captain")} is hit, apply <{{a}}>{Stormriders.KeywordTag("water")} to the attacker")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenAnyoneIsHit>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Water.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Attacker;
                status.constraints =
                [
                    TargetConstraintHelper.HasTrait(Captain.Name)
                ];
            });
    }
}