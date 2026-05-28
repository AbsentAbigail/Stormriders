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
public class Water : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectWater>(Name)
            .WithText("Water {a}")
            .WithStackable(true)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectWater>(status =>
            {
                status.type = "water";
                status.offensive = true;
                status.removeOnDiscard = true;
            })
            .Subscribe_WithStatusIcon("water");
    }
}