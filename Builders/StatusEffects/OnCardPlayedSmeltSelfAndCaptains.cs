#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedSmeltSelfAndCaptains : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"{Stormriders.KeywordTag(Keywords.Smelt.Name)} <{{a}}> to self and all {Stormriders.KeywordTag(Captain.Name)}s")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(InstantSmeltSelfAndCaptains.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Self;
            });
    }
}