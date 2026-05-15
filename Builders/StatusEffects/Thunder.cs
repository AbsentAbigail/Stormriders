using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.StatusEffectImplementations;
using WildfrostHopeMod.VFX;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class Thunder : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectThunder>(Name)
            .WithText("Thunder {a}")
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectThunder>(status =>
            {
                status.type = "thunder";
                // status.doesDamage = true;
                // status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Target;
                // status.dealDamage = true;
                // status.countsAsHit = true;
            })
            .Subscribe_WithStatusIcon("thunder");
    }
}