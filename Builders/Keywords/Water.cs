#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.Keywords;

[UsedImplicitly]
public class Water : IKeywordBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name.ToLower();

    public DataFileBuilder<KeywordData, KeywordDataBuilder> Builder()
    {
        return new KeywordDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithTitle("Water")
            .WithTitleColour(KeywordColours.Blue)
            .WithDescription(
                """
                Easily electrocuted
                Douses <sprite=spice>|Counts down after being struck by <sprite name=thunder>
                """)
            .WithBodyColour(KeywordColours.White)
            .WithNoteColour(KeywordColours.Blue);
    }
}