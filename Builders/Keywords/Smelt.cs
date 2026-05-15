#region

using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;

#endregion

namespace Stormriders.Builders.Keywords;

[UsedImplicitly]
public class Smelt : IKeywordBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name.ToLower();

    public DataFileBuilder<KeywordData, KeywordDataBuilder> Builder()
    {
        return new KeywordDataBuilder(Stormriders.Instance)
            .Create(Name)
            .WithTitle("Smelt")
            .WithTitleColour(KeywordColours.Orange)
            .WithShowName(true)
            .WithDescription(
                $"""
                If the target has {Stormriders.KeywordTag("thunder")}, increase {Stormriders.KeywordTag("thunder")}
                Otherwise, convert {Stormriders.VanillaKeywordTag("attack")} to equal {Stormriders.KeywordTag("thunder")}
                """)
            .WithBodyColour(KeywordColours.White)
            .WithNoteColour(KeywordColours.Orange);
    }
}