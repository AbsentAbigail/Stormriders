using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using Stormriders.Helpers;

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedIncreaseThunderOfCaptains : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"Increase {Stormriders.KeywordTag(Keywords.Thunder.Name)} of all {Stormriders.KeywordTag(Captain.Name)} allies by <{{a}}>")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Thunder.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                status.applyConstraints =
                [
                    TargetConstraintHelper.HasTrait(Traits.Captain.Name),
                    TargetConstraintHelper.HasStatus(Thunder.Name)
                ];
            });
    }
}