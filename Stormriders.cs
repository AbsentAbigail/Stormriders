#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Cards.Companions;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.Tribes;
using Stormriders.GameSystems;
using Stormriders.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using WildfrostHopeMod.SFX;
using WildfrostHopeMod.Utils;
using WildfrostHopeMod.VFX;
using Extensions = Deadpan.Enums.Engine.Components.Modding.Extensions;
using Object = UnityEngine.Object;

#endregion

namespace Stormriders;

[UsedImplicitly]
public class Stormriders : WildfrostMod
{
    public static Stormriders Instance;

    public Stormriders(string directory) : base(directory)
    {
        Instance = this;
    }

    public override string GUID => "pelli.wildfrost.stormriders";

    public override string[] Depends => ["hope.wildfrost.vfx"];

    public override string Title => "Stormriders";

    public override string Description => "A custom tribe of 4 leaders, 2 pets, and a bunch of companions, items, clunkers, and charms. Apply water to your foe, then strike them with thunder!";

    private List<object> _assets;
    private bool _loaded;

    public static List<string> CharmNames = [];
    public static List<string> CompanionNames = [];
    public static List<string> ItemNames = [];
    public static List<string> ClunkerNames = [];
    public static List<string> LeaderNames = [];

    //this is here to allow our icon to appear in the text box of cards
    public override TMP_SpriteAsset SpriteAsset => _assetSprites;
    private TMP_SpriteAsset _assetSprites;

    // Change "Windows" to whatever you want it named
    // This is where the addressables will be stored
    public static string CatalogFolder => Path.Combine(Instance.ModDirectory, "Addressables");
	    
    // A helpful shortcut
    public static string CatalogPath => Path.Combine(CatalogFolder, "catalog.json");
    
    public static SpriteAtlas SpriteAtlas;

    public override void Load()
    {
        if (!Addressables.ResourceLocators.Any(r => r is ResourceLocationMap map && map.LocatorId == CatalogPath))
        {
            Addressables.LoadContentCatalogAsync(CatalogPath).WaitForCompletion();
        }
        
        SpriteAtlas = GetAsset<SpriteAtlas>($"Assets/{GUID}/spriteatlas.spriteatlas");
        
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
            Eyes();
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
        Events.OnEntityCreated += FixLeaderImage;
    }

    private static void UnloadEvents()
    {
        Events.OnSceneLoaded -= SceneLoaded;
        Events.OnEntityCreated -= FixLeaderImage;
    }

    private static void SceneLoaded(Scene scene)
    {
        if (scene.name != "Campaign")
            return;

        GameObject.Find("Systems")?.AddComponent<ChargeRedrawBellSystem>();
    }
    
    private static void FixLeaderImage(Entity entity)
    {
        if (entity.display is Card { hasScriptableImage: false } card) //These cards should use the static image
        {
            card.mainImage.gameObject.SetActive(true);               //And this line turns them on
        }
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
        CompanionNames = GetNamesFromBuilders(companions.Where(builder => builder._data.name != PrefixGuid(Cheashir.Name) && builder._data.name != PrefixGuid(Grinkaw.Name)).ToList());
        
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

    private static void Eyes()
    {
        var list = new List<EyeData>()
        {
            Eyes("pelli.wildfrost.stormriders.AbbessGilly", (-0.02f, 1.42f, 1.00f, 1.00f, 0f),
                (0.36f, 1.43f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Cheashir", (-0.05f, 1.11f, 1.00f, 1.00f, 0f),
                (0.79f, 1.12f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Clover", (0.02f, 1.48f, 1.00f, 1.00f, 0f),
                (0.38f, 1.37f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.BittyBirdy", (0.05f, 1.37f, 1.00f, 1.00f, 0f),
                (0.31f, 1.38f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.FatherGilly", (-0.12f, 1.49f, 1.00f, 1.00f, 0f),
                (0.22f, 1.55f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Grinkaw", (-0.27f, 2.01f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Kokolockay", (0.10f, 0.94f, 1.00f, 1.00f, 0f),
                (0.36f, 0.93f, 0.88f, 0.98f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Korda", (0.30f, 1.92f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.NavyGator", (-0.13f, 1.36f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Orga", (-0.01f, 1.43f, 1.00f, 1.00f, 0f),
                (0.36f, 1.53f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Powder", (0.18f, 1.56f, 1.00f, 1.00f, 0f),
                (0.45f, 1.46f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Sarviche", (-0.06f, 1.84f, 1.00f, 1.00f, 0f),
                (0.24f, 1.74f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Shawreck", (-0.02f, 1.29f, 1.00f, 1.00f, 0f),
                (0.25f, 1.30f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Smoker", (-0.36f, 1.30f, 1.10f, 1.30f, 0f),
                (-0.05f, 1.31f, 0.80f, 1.20f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Suturgeon", (0.20f, 1.64f, 1.00f, 1.00f, 0f),
                (0.54f, 1.62f, 1.00f, 1.00f, 0f), (-0.33f, 0.52f, 1.00f, 1.00f, 0f), (1.07f, 0.10f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Sweeb", (-0.22f, 1.68f, 1.00f, 1.00f, 0f),
                (0.06f, 1.75f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Tunar", (-0.06f, 1.85f, 0.90f, 0.90f, 0f),
                (0.21f, 1.88f, 0.90f, 0.90f, 0f), (1.58f, 2.49f, 0.50f, 0.50f, 0f), (1.41f, 1.89f, 0.50f, 0.50f, 0f),
                (1.52f, 1.08f, 0.50f, 0.50f, 0f), (1.09f, 1.19f, 0.50f, 0.50f, 0f), (0.93f, 1.70f, 0.50f, 0.50f, 0f),
                (0.60f, 1.00f, 0.50f, 0.50f, 0f), (0.93f, 0.54f, 0.50f, 0.50f, 0f), (0.42f, 0.38f, 0.50f, 0.50f, 0f),
                (0.04f, 0.91f, 0.50f, 0.50f, 0f), (-0.45f, 1.59f, 0.50f, 0.50f, 0f), (-0.48f, 1.22f, 0.50f, 0.50f, 0f),
                (-0.97f, 1.01f, 0.50f, 0.50f, 0f), (-1.43f, 1.05f, 0.50f, 0.50f, 0f), (-1.42f, 0.38f, 0.50f, 0.50f, 0f),
                (-0.92f, 0.55f, 0.50f, 0.50f, 0f), (-0.91f, -0.01f, 0.50f, 0.50f, 0f),
                (-0.44f, 0.72f, 0.50f, 0.50f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Vulklang", (-0.31f, 1.33f, 1.00f, 1.00f, 0f),
                (0.08f, 1.43f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Wayne", (0.01f, 2.10f, 1.00f, 1.00f, 0f),
                (0.39f, 2.13f, 1.00f, 1.00f, 0f)),
            Eyes("pelli.wildfrost.stormriders.Shoregan", (0.17f, 2.23f, 1.08f, 1.08f, 4f)),
            Eyes("pelli.wildfrost.stormriders.Maryanne", (-0.2f, 1.50f, 1.08f, 1.08f, 1f)),
            Eyes("pelli.wildfrost.stormriders.CalicoKid", (0.20f, 1.94f, 1.18f, 1.18f, 2f),
                (0.58f, 1.92f, 1.08f, 1.08f, 2f)),
            Eyes("pelli.wildfrost.stormriders.Berrybeard", (0.00f, 2.11f, 0.98f, 0.98f, 2f),
                (0.27f, 2.10f, 0.88f, 0.88f, 2f)),
        };
        
        AddressableLoader.AddRangeToGroup("EyeData", list);
    }

    private static EyeData Eyes(string cardName, params (float, float, float, float, float)[] data)
    {
        EyeData eyeData = new Script<EyeData>();
        eyeData.cardData = cardName;
        eyeData.name = cardName + "_EyeData";
        eyeData.eyes = data.Select(e => new EyeData.Eye
        {
            position = new Vector2(e.Item1, e.Item2),
            scale = new Vector2(e.Item3, e.Item4),
            rotation = e.Item5,
        }).ToArray();
        return eyeData;
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
            As the storm spreads from the land and out into the open sea, pirates and merfolk have come to defend their turf from a frigid demise.
            
            Applying water onto enemies in order to compound thunderous damage onto them is the heart of their plan. Captains guide their team of companions who scale through increasing their thunder and out-healing their wet foes.
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
        return SpriteAtlas?.GetSprite(spriteName) ?? Instance.ImagePath($"{spriteName}.png").ToSprite();
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
    
    // Code by Phan
    public static T CreateScriptableCardImage<T>(string name) where T : ScriptableCardImage
    {
        // Create a new GameObject that will host the ScriptableImage
        var ghostObject = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(T))
        {
            // HideAndDontSave so it doesn't get touched during gameplay, OR
            hideFlags = HideFlags.HideAndDontSave
        };

        // ensure the GameObject is kept in memory this session
        Object.DontDestroyOnLoad(ghostObject);

        // Set the GameObject's size to the card size
        ghostObject.GetComponent<RectTransform>().sizeDelta = new Vector2(3.8f, 5.7f);

        // The image will try to autofill to fit the RectTransform size
        ghostObject.GetComponent<Image>().preserveAspect = true;
        // This fixes the card being hoverable
        ghostObject.GetComponent<Image>().raycastTarget = false;

        return ghostObject.GetComponent<T>();
    }
}