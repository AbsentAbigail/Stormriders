#region

using UnityEngine;

#endregion

namespace Stormriders.Helpers;

public static class LeaderHelper
{
    public static CardScript GiveUpgrade(string name = "Crown")
    {
        return new Script<CardScriptGiveUpgrade>(
            $"Give {name}",
            script => script.upgradeData =
                Stormriders.GetCardUpgrade(name)
        );
    }

    public static CardScript AddRandomHealth(Vector2Int range)
    {
        return new Script<CardScriptAddRandomHealth>(
            $"Add Random Health Between {range.x} And {range.y}",
            script => script.healthRange = range
        );
    }

    public static CardScript AddRandomDamage(Vector2Int range)
    {
        return new Script<CardScriptAddRandomDamage>(
            $"Add Random Damage Between {range.x} And {range.y}",
            script => script.damageRange = range
        );
    }

    public static CardScript AddRandomCounter(Vector2Int range)
    {
        return new Script<CardScriptAddRandomCounter>(
            $"Add Random Counter Between {range.x} And {range.y}",
            script => script.counterRange = range
        );
    }
}