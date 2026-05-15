#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.Keywords;

[UsedImplicitly]
public class Thunder : IKeywordBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name.ToLower();

    public DataFileBuilder<KeywordData, KeywordDataBuilder> Builder()
    {
        return new KeywordDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithTitle("Thunder")
            .WithTitleColour(KeywordColours.Orange)
            .WithDescription(
                $"""
                Strike damage onto a target once, plus additional times equal to each stack of {Stormriders.KeywordTag("water")} on the target
                The shock prevents on hit effects from triggering
                """)
            .WithBodyColour(KeywordColours.White)
            .WithNoteColour(KeywordColours.Orange);
    }
}