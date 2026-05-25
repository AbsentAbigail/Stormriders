#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using WildfrostHopeMod.VFX;

#endregion

namespace Stormriders.Builders.Icons;

[UsedImplicitly]
public class Water : IIconBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name.ToLower();

    public DataFileBuilder<_StatusIconData, StatusIconBuilder> Builder()
    {
        return new StatusIconBuilder(Stormriders.Instance)
            .Create(name: Name,
                statusType: "water",
                Stormriders.Instance.ImagePath("Icons/water.png").ToSprite())
            .WithIconGroupName(StatusIconBuilder.IconGroups.health)
            .WithTextColour(KeywordColours.White)
            .WithTextShadow(KeywordColours.DarkPurple)
            .WithTextboxSprite()
            .WithKeywords(Keywords.Water.Name)
            .WithApplySFX(Stormriders.Instance.ImagePath("Sounds/water.mp3"))
            // .WithApplyVFX(Stormriders.Instance.ImagePath("Anim/water.gif"))
            .WithSiding(-1);
    }
}