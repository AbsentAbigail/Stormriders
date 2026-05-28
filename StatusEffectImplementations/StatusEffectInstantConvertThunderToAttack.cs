#region

using System.Collections;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectInstantConvertThunderToAttack : StatusEffectInstant
{
    public override IEnumerator Process()
    {
        var thunder = target.FindStatus("thunder");
        if (thunder == null)
        {
            yield break;
        }

        var amount = thunder.count;

        yield return thunder.Remove();
        
        var cardData = target.data;
        cardData.hasAttack = true;
        if (cardData.playType == Card.PlayType.None)
        {
            cardData.playType = Card.PlayType.Play;
        }
        cardData.needsTarget = true;
        target.damage.max = amount;
        target.damage.current = amount;
        yield return base.Process();
    }
}