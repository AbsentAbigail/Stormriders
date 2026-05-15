using System.Collections;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectApplyXWhenUnitIsKilledMultipleTimes : StatusEffectApplyXWhenUnitIsKilled
{
    
    public override void Init()
    {
        OnEntityDestroyed += Multiple;
    }

    private IEnumerator Multiple(Entity entity, DeathType deathType)
    {
        for (var i = 0; i < GetAmount(); i++)
        {
            yield return Check(entity,  deathType);
        }
    }
}