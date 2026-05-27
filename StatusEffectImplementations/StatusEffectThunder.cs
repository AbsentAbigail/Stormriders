using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stormriders.Helpers;
using UnityEngine;
using WildfrostHopeMod.VFX;

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectThunder : StatusEffectApplyX
{
    private bool _cancel;
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
        yield return targets.Select(t => Check(new Hit(entity, t))).GetEnumerator();
    }

    public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
    {
        return target.enabled && entity == target && (!targetMustBeAlive || target.alive);
    }

    private IEnumerator Check(Hit hit)
    {
        yield return Run([hit.target], 0.2f);
        
        var waterEffect = hit.target.FindStatus("water");
        if (waterEffect is null)
        {
            yield break;
        }

        _cancel = false;
        for (var i = 0; i < (waterEffect != null ? waterEffect.count : 0); i++)
        {
            ActionQueue.Stack(new ActionSequence(Run([hit.target], 0.2f))
            {
                note = name + " - " + i
            });
            if (_cancel)
            {
                break;
            }
        }

        var amount = 1;
        Events.InvokeStatusEffectCountDown(waterEffect, ref amount);
        yield return waterEffect.CountDown(hit.target, amount);
        
        hit.target.display.promptUpdateDescription = true;
        hit.target.PromptUpdate();
    }

    private IEnumerator Run(List<Entity> targets, float delay)
    {
        var hit = false;
        foreach (var entity in targets)
        {
            if (entity.hp.current < 0)
            {
                continue;
            }
            
            VFXHelper.VFX.TryPlayEffect("thunder_attack", entity.transform.position, target.transform.lossyScale,
                GIFLoader.PlayType.damageEffect);
            yield return new Hit(target, entity, count)
            {
                canRetaliate = false,
                countsAsHit = true,
                trigger = new Trigger(target, target, "thunder", [.. targets]),
                damageType = type
            }.Process();
            hit = true;
        }

        if (!hit)
        {
            _cancel = true;
            yield break;
        }
        VFXHelper.SFX.TryPlaySound("thunder_attack");
        yield return new WaitForSeconds(delay);
    }
}