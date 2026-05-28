#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenHitApplyWaterToAttacker : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenHit>(Name)
            .WithText($"When hit, apply <{{a}}>{Stormriders.KeywordTag("water")} to the attacker")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenHit>(status =>
            {
                status.targetMustBeAlive = false;
                status.effectToApply = Stormriders.GetStatus(Water.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Attacker;
            });
    }
}