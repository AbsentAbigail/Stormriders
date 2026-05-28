#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using Stormriders.Helpers;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class OnCardPlayedSmeltAlliedCaptains : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectApplyXOnCardPlayed>(Name)
            .WithText($"{Stormriders.KeywordTag(Keywords.Smelt.Name)} <{{a}}> to all {Stormriders.KeywordTag(Captain.Name)} allies")
            .WithStackable(true)
            .WithCanBeBoosted(true)
            .SubscribeToAfterAllBuildEvent<StatusEffectApplyXOnCardPlayed>(status =>
            {
                status.effectToApply = Stormriders.GetStatus(Smelt.Name);
                status.applyToFlags = StatusEffectApplyX.ApplyToFlags.Allies;
                status.applyConstraints =
                [
                    TargetConstraintHelper.HasTrait(Traits.Captain.Name)
                ];
            });
    }
}