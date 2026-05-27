#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.StatusEffects;

[UsedImplicitly]
public class InstantKill : IStatusBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public DataFileBuilder<StatusEffectData, StatusEffectDataBuilder> Builder()
    {
        return new StatusEffectDataBuilder(Stormriders.Instance)
            .Create<StatusEffectInstantKill>(Name)
            .WithText("Kill")
            .WithStackable(false)
            .WithCanBeBoosted(false);
    }
}