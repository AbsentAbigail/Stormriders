using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Tribes;
using Stormriders.GameSystems;
using Stormriders.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using WildfrostHopeMod.SFX;
using WildfrostHopeMod.Utils;
using WildfrostHopeMod.VFX;
using Extensions = Deadpan.Enums.Engine.Components.Modding.Extensions;

namespace Stormriders;

[UsedImplicitly]
public class Stormriders : WildfrostMod
{
    public static Stormriders Instance;

    public Stormriders(string directory) : base(directory)
    {
        Instance = this;
    }

    public override string GUID => "absentabigail.wildfrost.stormriders";

    public override string[] Depends => [];

    public override string Title => "Stormriders";

    public override string Description => "Water and Lightning";

    private List<object> _assets;
    private bool _loaded = false;

    public static List<string> CharmNames = [];
    public static List<string> CompanionNames = [];
    public static List<string> ItemNames = [];
    public static List<string> ClunkerNames = [];
    public static List<string> LeaderNames = [];

    //this is here to allow our icon to appear in the text box of cards
    public override TMP_SpriteAsset SpriteAsset => _assetSprites;
    private TMP_SpriteAsset _assetSprites;

    public override void Load()
    {
        VFXHelper.VFX = new GIFLoader(this, ImagePath("Anim"));
        VFXHelper.VFX.RegisterAllAsApplyEffect();

        VFXHelper.SFX = new SFXLoader(ImagePath("Sounds"));
        VFXHelper.SFX.RegisterAllSoundsToGlobal();

        
        //Needed to get sprites in text boxes
        _assetSprites = HopeUtils.CreateSpriteAsset("StormRidersAssets", ImagePath("Icons"));
        SpriteAsset.RegisterSpriteAsset();

        if (!_loaded)
        { 
            CreateModAssets();
        }
        base.Load();

        LoadEvents();
        
        var gameMode = TryGet<GameMode>("GameModeNormal"); //GameModeNormal is the standard game mode. 
        gameMode.classes = gameMode.classes.Append(TryGet<ClassData>(StormriderTribe.Name)).ToArray();
    }

    public override void Unload()
    {
        UnloadEvents();
        UnloadFromClasses();
        base.Unload();
        
        var gameMode = TryGet<GameMode>("GameModeNormal");
        gameMode.classes = RemoveNulls(gameMode.classes);
        UnloadFromClasses(); 
    }

    private static void LoadEvents()
    {
        Events.OnSceneLoaded += SceneLoaded;
    }

    private static void UnloadEvents()
    {
        Events.OnSceneLoaded -= SceneLoaded;
    }

    private static void SceneLoaded(Scene scene)
    {
        if (scene.name != "Campaign")
            return;

        GameObject.Find("Systems")?.AddComponent<ChargeRedrawBellSystem>();
    }
    
    private void CreateModAssets()
    {
        _assets = [];

        _assets.AddRange(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Icons",
                    StringComparison.Ordinal)
                && typeof(IIconBuilder).IsAssignableFrom(t))
            .Select(type => ((IIconBuilder)Activator.CreateInstance(type)).Builder()).ToList()
        );

        _assets.AddRange(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.StatusEffects",
                    StringComparison.Ordinal)
                && typeof(IStatusBuilder).IsAssignableFrom(t))
            .Select(type => ((IStatusBuilder)Activator.CreateInstance(type)).Builder()).ToList()
        );

        _assets.AddRange(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Keywords",
                    StringComparison.Ordinal)
                && typeof(IKeywordBuilder).IsAssignableFrom(t))
            .Select(type => ((IKeywordBuilder)Activator.CreateInstance(type)).Builder()).ToList()
        );

        _assets.AddRange(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Traits",
                    StringComparison.Ordinal)
                && typeof(ITraitBuilder).IsAssignableFrom(t))
            .Select(type => ((ITraitBuilder)Activator.CreateInstance(type)).Builder()).ToList()
        );

        var companions = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Cards.Companions",
                    StringComparison.Ordinal)
                && typeof(ICardBuilder).IsAssignableFrom(t))
            .Select(type => ((ICardBuilder)Activator.CreateInstance(type)).Builder()).Cast<CardDataBuilder>().ToList();
        _assets.AddRange(companions);
        CompanionNames = GetNamesFromBuilders(companions);
        
        var clunkers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Cards.Clunkers",
                    StringComparison.Ordinal)
                && typeof(ICardBuilder).IsAssignableFrom(t))
            .Select(type => ((ICardBuilder)Activator.CreateInstance(type)).Builder()).Cast<CardDataBuilder>().ToList();
        _assets.AddRange(clunkers);
        ClunkerNames = GetNamesFromBuilders(clunkers);

        var items = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Cards.Items",
                    StringComparison.Ordinal)
                && typeof(ICardBuilder).IsAssignableFrom(t))
            .Select(type => ((ICardBuilder)Activator.CreateInstance(type)).Builder()).Cast<CardDataBuilder>().ToList();
        _assets.AddRange(items);
        ItemNames = GetNamesFromBuilders(items);

        var leaders = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Cards.Leaders",
                    StringComparison.Ordinal)
                && typeof(ILeaderBuilder).IsAssignableFrom(t))
            .Select(type =>
            {
                var builder = (ILeaderBuilder)Activator.CreateInstance(type);
                var cardBuilder = (CardDataBuilder)builder.Builder();
                cardBuilder
                    .WithCardType("Leader")
                    .FreeModify(card =>
                    {
                        card.createScripts =
                        [
                            LeaderHelper.GiveUpgrade(),
                        ];
                    });

                return cardBuilder;
            }).ToList();
        _assets.AddRange(leaders);
        LeaderNames = GetNamesFromBuilders(leaders);

        var charms = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Upgrades",
                    StringComparison.Ordinal)
                && typeof(IUpgradeBuilder).IsAssignableFrom(t))
            .Select(type => ((IUpgradeBuilder)Activator.CreateInstance(type)).Builder()).ToList();
        _assets.AddRange(charms);
        CharmNames = charms.Select(builder => builder._data.name).ToList();

        _assets.AddRange(Assembly.GetExecutingAssembly().GetTypes()
            .Where(t =>
                string.Equals(t.Namespace, "Stormriders.Builders.Tribes",
                    StringComparison.Ordinal)
                && typeof(IClassBuilder).IsAssignableFrom(t))
            .Select(type => ((IClassBuilder)Activator.CreateInstance(type)).Builder()).ToList()
        );

        CreateLocalizedStrings();

        _loaded = true;
    }

    private void UnloadFromClasses()
    {
        // Remove data from Tribes
        var tribes = AddressableLoader.GetGroup<ClassData>("ClassData");
        foreach (var pool in from tribe in tribes where tribe != null && tribe.rewardPools != null from pool in tribe.rewardPools where pool != null select pool)
        {
            pool.list.RemoveAllWhere(item => item == null || item.ModAdded == this);
        }
    }

    private static void CreateLocalizedStrings()
    {
        var uiText = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English);
        uiText.SetString(StormriderTribe.TitleKey, "The Stormriders");
        uiText.SetString(StormriderTribe.DescKey,
            """
            No text yet
            """);

        uiText.SetString("ChargeBell", "{0} charged the Redraw Bell by [{1}]");
        uiText.SetString("ChargeBellFully", "{0} fully charged the Redraw Bell");
    }

    public override List<T> AddAssets<T, TY>()
    {
        if (_assets.OfType<T>().Any())
            Debug.LogWarning($"[{Title}] adding {typeof(TY).Name}s: {_assets.OfType<T>().Select(a => a._data.name).Join()}");
        return _assets.OfType<T>().ToList();
    }
    
    
    public static T[] RemoveNulls<T>(T[] data) where T : DataFile
    {
        var list = data.ToList();
        list.RemoveAll(x => x == null || x.ModAdded == Instance);
        return list.ToArray();
    }

    public static Sprite GetSprite(string spriteName)
    {
        return Instance.ImagePath($"{spriteName}.png").ToSprite();
    }

    public static string PrefixGuid(string name)
    {
        return Extensions.PrefixGUID(name, Instance);
    }

    public static StatusEffectData GetStatus(string statusName)
    {
        return TryGet<StatusEffectData>(statusName);
    }

    public static CardData.StatusEffectStacks SStack(string statusName, int amount = 1)
    {
        return new CardData.StatusEffectStacks(
            GetStatus(statusName),
            amount);
    }

    public static CardData.TraitStacks TStack(string traitName, int amount = 1)
    {
        return new CardData.TraitStacks(
            GetTrait(traitName),
            amount);
    }

    public static T GetStatusOf<T>(string statusName) where T : StatusEffectData
    {
        return TryGet<T>(statusName);
    }

    public static CardData GetCard(string cardName)
    {
        return TryGet<CardData>(cardName);
    }

    public static CardUpgradeData GetCardUpgrade(string cardUpgradeName)
    {
        return TryGet<CardUpgradeData>(cardUpgradeName);
    }

    public static TraitData GetTrait(string traitName)
    {
        return TryGet<TraitData>(traitName);
    }

    public static KeywordData GetKeyword(string keywordName)
    {
        return TryGet<KeywordData>(keywordName);
    }

    public static ClassData GetTribe(string tribeName)
    {
        return TryGet<ClassData>(tribeName);
    }

    public static CardType GetCardType(string cardTypeName)
    {
        return TryGet<CardType>(cardTypeName);
    }

    public static T TryGet<T>(string datafileName) where T : DataFile
    {
        T dataFile;
        if (typeof(StatusEffectData).IsAssignableFrom(typeof(T)))
            dataFile = Instance.Get<StatusEffectData>(datafileName) as T;
        else
            dataFile = Instance.Get<T>(datafileName);

        return dataFile ??
               throw new Exception(
                   $"TryGet Error: Could not find a [{typeof(T).Name}] with the name [{datafileName}] or [{Extensions.PrefixGUID(datafileName, Instance)}]");
    }

    public static StatusEffectDataBuilder StatusCopy(string oldName, string newName)
    {
        var data = GetStatus(oldName).InstantiateKeepName();
        data.name = PrefixGuid(newName);
        var builder = data.Edit<StatusEffectData, StatusEffectDataBuilder>();
        builder.Mod = Instance;
        return builder;
    }

    public static ClassDataBuilder TribeCopy(string oldName, string newName)
    {
        var data = GetTribe(oldName).InstantiateKeepName();
        data.name = PrefixGuid(newName);
        var builder = data.Edit<ClassData, ClassDataBuilder>();
        builder.Mod = Instance;
        return builder;
    }

    public static string CardTag(string name)
    {
        return $"<card={PrefixGuid(name)}>";
    }

    public static string VanillaCardTag(string name)
    {
        return $"<card={name}>";
    }

    public static string KeywordTag(string name)
    {
        return $"<keyword={PrefixGuid(name)}>";
    }
    
    public static string VanillaKeywordTag(string name)
    {
        return $"<keyword={name}>";
    }

    private List<string> GetNamesFromBuilders(List<CardDataBuilder> data)
    {
        return data.Select(builder => builder._data.name).ToList();
    }
}