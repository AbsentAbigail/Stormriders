#region

using System;
using System.Linq;
using System.Reflection;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Cards.Clunkers;
using Stormriders.Builders.Cards.Companions;
using Stormriders.Builders.Cards.Items;
using Stormriders.Builders.Interfaces;
using Stormriders.Helpers;
using UnityEngine;
using Extensions = Deadpan.Enums.Engine.Components.Modding.Extensions;
using Object = UnityEngine.Object;

#endregion

namespace Stormriders.Builders.Tribes;

[UsedImplicitly]
public class StormriderTribe : IClassBuilder
{
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;

    public const string UnitPoolName = "Stormriders.UnitPool";
    public const string ItemPoolName = "Stormriders.ItemPool";
    public const string CharmPoolName = "Stormriders.CharmPool";
    public static readonly string TitleKey = Stormriders.PrefixGuid("PlushTribeTitle");
    public static readonly string DescKey = Stormriders.PrefixGuid("PlushTribeDesc");
    
    public DataFileBuilder<ClassData, ClassDataBuilder> Builder()
    {
        return Stormriders.TribeCopy("Basic", Name)
            .WithFlag(Stormriders.GetSprite("banner"))
            .SubscribeToAfterAllBuildEvent(tribe =>
            {
                var playerCharacter = tribe.characterPrefab.gameObject.InstantiateKeepName();
                Object.DontDestroyOnLoad(playerCharacter);
                playerCharacter.name = "Stormriders.Stormrider";
                tribe.characterPrefab = playerCharacter.GetComponent<Character>();

                Inventory inventory = new Script<Inventory>("Inventory (Stormriders.StormriderTribe)", null);
                inventory.deck.list = DataList<CardData>(
                    SlushBolas.Name,
                    BrightCutlass.Name,
                    BrightCutlass.Name,
                    BrightCutlass.Name,
                    SlopMop.Name,
                    SunMelonade.Name,
                    ThunderRum.Name,
                    Wethead.Name
                ).ToList();
                tribe.startingInventory = inventory;

                tribe.leaders = DataList<CardData>([.. Stormriders.LeaderNames]);

                tribe.rewardPools =
                [
                    UnitPool(),
                    ItemPool(),
                    CharmPool(),
                    Extensions.GetRewardPool("GeneralModifierPool"),
                    Extensions.GetRewardPool("GeneralUnitPool"),
                    Extensions.GetRewardPool("GeneralItemPool"),
                    Extensions.GetRewardPool("GeneralCharmPool"),
                ];
            });
    }

    private static RewardPool UnitPool()
    {
        return CreateRewardPool(UnitPoolName, nameof(RewardPool.Type.Units),
        [
            .. DataList<CardData>(
                [.. Stormriders.CompanionNames]
            )
        ]);
    }

    private static RewardPool ItemPool()
    {
        return CreateRewardPool(ItemPoolName, nameof(RewardPool.Type.Items),
        [
            .. DataList<CardData>(
                [.. Stormriders.ItemNames,
                .. Stormriders.ClunkerNames]
            )
        ]);
    }

    private static RewardPool CharmPool()
    {
        return CreateRewardPool(CharmPoolName, nameof(RewardPool.Type.Charms),
        [
            .. DataList<CardUpgradeData>(
                [.. Stormriders.CharmNames]
            )
        ]);
    }

    private static T[] DataList<T>(params string[] names) where T : DataFile
    {
        return names.Select(Stormriders.TryGet<T>).ToArray();
    }

    private static RewardPool CreateRewardPool(string name, string type, DataFile[] list)
    {
        var pool = ScriptableObject.CreateInstance<RewardPool>();
        pool.name = name;
        pool.type = type;
        pool.list = list.ToList();
        return pool;
    }
}