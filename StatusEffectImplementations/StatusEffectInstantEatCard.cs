#region

using System.Collections;
using System.Linq;
using UnityEngine;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusInstantEatCard : StatusEffectInstantApplyEffect
{
    public bool gainHealth = true;
    public bool gainAttack = true;
    public bool gainEffects = false;
    public TraitData[] illegalTraits;

    public StatusEffectData[] illegalEffects;

    public override IEnumerator Process()
    {
        if ((bool)applier && applier.alive && (bool)target && (gainHealth || gainAttack || gainEffects))
        {
            Trigger trigger = new(applier, applier, "eat", [target])
            {
                countsAsTrigger = false
            };
            Hit hit = new(applier, target, 0)
            {
                canBeNullified = false,
                canRetaliate = false,
                damageType = "eat",
                trigger = trigger,
                doAnimation = false
            };
            trigger.hits = [hit];
            yield return trigger.Process();
            yield return Eat();
        }

        SfxSystem.OneShot("event:/sfx/card/destroy");
        yield return base.Process();
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator Eat()
    {
        if (gainHealth)
        {
            GainHealth();
        }

        if (gainAttack)
        {
            GainAttack();
        }

        if (gainEffects)
        {
            yield return GainEffects();
        }

        applier.PromptUpdate();
    }

    private void GainHealth()
    {
        applier.hp.current += target.hp.current;
        applier.hp.max += target.hp.max;
    }

    private void GainAttack()
    {
        applier.damage.current += target.damage.current;
        applier.damage.max += target.damage.max;
    }

    private IEnumerator GainEffects()
    {
        applier.attackEffects = CardData.StatusEffectStacks.Stack(applier.attackEffects, target.attackEffects).ToList();
        var list = target.statusEffects.Where(e =>
            e != this &&
            !illegalEffects.Select(e2 => e2.name).Contains(e.name)
            && e is not StatusEffectNextPhase
        ).ToList();
        foreach (var trait in target.traits.ToArray())
        {
            foreach (var passiveEffect in trait.passiveEffects)
                list.Remove(passiveEffect);

            var num = trait.count - trait.tempCount;
            if (num > 0 && !illegalTraits.Select(t => t.name).Contains(trait.data.name))
                applier.GainTrait(trait.data, num);
        }

        foreach (var item in list)
            yield return StatusEffectSystem.Apply(applier, target, item, item.count);

        yield return applier.UpdateTraits();
        applier.display.promptUpdateDescription = true;
    }
}