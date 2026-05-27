using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnKillGainCaptain : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnKill>(Name)
            .WithText($"On kill, gain {Stormriders.KeywordTag("captain")}")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnKill>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(TemporaryCaptain.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.applyConstraints = [
                    TargetConstraintHelper.HasTrait(Captain.Name, not: true)
                ];
            });
    }
}