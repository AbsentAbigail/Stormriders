#region

using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Builders.Interfaces;
using Stormriders.Builders.StatusEffects;
using Stormriders.Builders.Traits;
using Stormriders.Helpers;
using Stormriders.StatusEffectImplementations;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Stormriders.Builders.Cards.Companions;

[UsedImplicitly]
public class Kokolockay : ICardBuilder
{
    public DataFileBuilder<CardData, CardDataBuilder> Builder()
    {
        return new CardDataBuilder(Stormriders.Instance)
            .CreateUnit(Name, "Kokolockay",
                bloodProfile: "Blood Profile Berry")
            .SetStats(5, 0, 5)
            .SetSprites(
                Stormriders.GetSprite("Kokolockay"),
                Stormriders.GetSprite("Kokolockay_bg"))
            .DropsBling(4)
            .SubscribeToAfterAllBuildEvent(card =>
            {
                card.startWithEffects =
                [
                    Stormriders.SStack("On Card Played Damage To Self", 2),
                    Stormriders.SStack(WhenHitByWaterEnemyAbsorbAttacker.Name, 2),
                ];
                card.charmSlots = 0;
                card.greetMessages =
                [
                    "So much garbage is thrown into my beautiful ocean that some of it even falls all the way down here. To the depths of my abyss. Don't worry. I'll take out the garbage for you.",
                    "( Kokolockay looks at you and licks zir lips )",
                    "You're trying to be a hero. I couldn't care less about that. But what I do care about... is getting a delicious meal. And I think you can help me there.",
                ];
                card.traits =
                [
                    Stormriders.TStack(Abyssal.Name)
                ];
                card.scriptableImagePrefab = Stormriders.CreateScriptableCardImage<KokolockayCardImage>("kokolockay");
            });
    }
    
    public static string Name { get; } = AccessTools.GetOutsideCaller().DeclaringType!.Name;
}


internal class KokolockayCardImage : ScriptableCardImage
{
    public Image Image => GetComponent<Image>();

    // gets called when the card is created (e.g. Leaders having one consistent avatar)
    public override void AssignEvent()
    {
        // we use the CardData's main sprite for a backup here
        // otherwise it won't have any sprite
        Image.sprite = entity.data.mainSprite;
        // Move Kokolockay down to fit the card frame better
        transform.localPosition += new Vector3(0, -1f, 0);
    }

    public override void UpdateEvent()
    {
        var eatEffect = (StatusEffectWhenHitByWaterCount)entity.statusEffects.FirstOrDefault(status => status is StatusEffectWhenHitByWaterCount);
        if (eatEffect == null)
        {
            return;
        }
        var scale = 1 + eatEffect.eatCount * 0.1f;

        transform.localScale = new Vector3(scale, scale, 1f);
    }
}