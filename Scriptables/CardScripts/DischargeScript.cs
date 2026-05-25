using Stormriders.Builders.StatusEffects;

namespace Stormriders.Scriptables.CardScripts;

public class DischargeScript : CardScript
{
    public override void Run(CardData target)
    {
        target.startWithEffects =
        [
            .. target.startWithEffects,
            Stormriders.SStack(Thunder.Name, target.damage)
        ];
    }
}