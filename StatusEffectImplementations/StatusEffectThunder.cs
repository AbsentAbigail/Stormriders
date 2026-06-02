using System.Collections;
using System.Linq;
using Stormriders.Helpers;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectThunder : StatusEffectApplyX
{
    private bool _hadAttack;

    public override object GetMidBattleData()
    {
        return _hadAttack;
    }

    public override void RestoreMidBattleData(object data)
    {
        _hadAttack = (bool)data;
    }

    public override void Init()
    {
        OnCardPlayed += CardPlayed;
        Events.OnEntityDisplayUpdated += PreventAttack;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= PreventAttack;
        if (_hadAttack)
        {
            target.data.hasAttack = true;
        }
    }

    private void PreventAttack(Entity entity)
    {
        if (entity != target)
        {
            return;
        }

        if (entity.data.hasAttack)
        {
            _hadAttack = true;
            entity.data.hasAttack = false;
        }

        if (entity.tempDamage.Value > 0)
        {
            entity.tempDamage.Value = 0;
        }
        
        if (entity.damage.max > 0)
        {
            entity.damage.max = 0;
        }

        if (entity.damage.current > 0)
        {
            entity.damage.current = 0;
        }

        if (entity.display.damageIcon != null)
        {
            target.display.RemoveStatusIcon("damage", "damage");
        }
    }

    private IEnumerator CardPlayed(Entity entity, Entity[] targets)
    {
        if (!targets.Any())
        {
            yield return NoTargetTextSystem.Run(target, NoTargetType.NoTargetToAttack);
            yield break;
        }
        yield return targets.Select(hitTarget => Check(hitTarget)).GetEnumerator();
    }

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        return target.enabled && entity == target && (!targetMustBeAlive || target.alive);
    }

    private IEnumerator Check(Entity hitTarget)
    {
        var water = hitTarget.FindStatus("water")?.count ?? 0;
            
        ActionQueue.Insert(0, new ActionSequence(Run(hitTarget, water, 0.2f))
        {
            note = name + " - " + water
        });
        yield break;
    }

    private IEnumerator Run(Entity hitTarget, int water, float delay)
    {
        if (hitTarget.hp.current < 0)
        {
            yield break;
        }

        VFXHelper.VFX.TryPlayEffect("thunder_attack", hitTarget.transform.position, target.transform.lossyScale,
            GIFLoader.PlayType.damageEffect);
        yield return new Hit(target, hitTarget, count)
        {
            canRetaliate = false,
            countsAsHit = true,
            trigger = new Trigger(target, target, "thunder", [hitTarget]),
            damageType = type
        }.Process();
        
        VFXHelper.SFX.TryPlaySound("thunder_attack");
        yield return new WaitForSeconds(delay);

        if (water-- > 0)
        {
            ActionQueue.Insert(0, new ActionSequence(Run(hitTarget, --water, 0.2f))
            {
                note = name + " - " + water
            });
        }
        else
        {
            var waterEffect = hitTarget.FindStatus("water");
            if (waterEffect is null)
            {
                yield break;
            }
            
            var amount = 1;
            Events.InvokeStatusEffectCountDown(waterEffect, ref amount);
            yield return waterEffect.CountDown(hitTarget, amount);
        
            hitTarget.display.promptUpdateDescription = true;
            hitTarget.PromptUpdate();
        }
    }
}