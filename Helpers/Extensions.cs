using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace Stormriders.Helpers;

public static class Extensions
{
    public static CardDataBuilder DropsBling(this CardDataBuilder builder, int amount)
    {
        return builder.WithValue(amount * 36);
    }

    public static object GetCustomDataOrNull(this CardData cardData, string key)
    {
        return cardData.customData?.Get<object>(key, null);
    }
}