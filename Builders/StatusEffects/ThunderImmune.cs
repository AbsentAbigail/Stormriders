#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;
using WildfrostHopeMod.VFX;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class ThunderImmune : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenYAppliedToNotStatus>(Name)
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenYAppliedToNotStatus>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantDoNothing.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
                status.instead = true;
                
                status.whenAppliedTypes = ["thunder", "smelt"];
                status.whenAppliedToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            });
    }
}