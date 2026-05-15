#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Keywords;
using UnityEngine;
using WildfrostHopeMod.VFX;

#endregion

namespace Stormriders.Builders.Icons;

[UsedImplicitly]
public class Thunder : IIconBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name.ToLower();

    public DataFileBuilder<_StatusIconData, StatusIconBuilder> Builder()
    {
        return new StatusIconBuilder(Stormriders.Instance)
            .Create(name: Name,
                statusType: "thunder",
                Stormriders.Instance.ImagePath("Icons/thunder.png").ToSprite())
            .WithIconGroupName(StatusIconBuilder.IconGroups.damage)
            .WithTextColour(new Color(0.2f, 0.2f, 0.3f))
            .WithTextShadow(KeywordColours.White)
            .WithTextboxSprite()
            .WithKeywords(Keywords.Thunder.Name);
    }
}