using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class WhenAllyDealsDamageApplyEqualSnowToTarget : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXWhenAllyDealsDamage>(Name)
            .WithText("When an ally deals damage, apply equal <keyword=snow> to the target")
            .WithStackable(false)
            .WithCanBeBoosted(false)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXWhenAllyDealsDamage>(status =>
            {
                status.effectToApply = Stormriders.GetStatus("Snow");
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Target;
                status.applyEqualAmount = true;
            });
    }
}