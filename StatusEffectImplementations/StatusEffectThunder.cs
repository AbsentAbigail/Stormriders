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
    public override void Init()
    {
        OnCardPlayed += CardPlayed;
        Events.OnEntityDisplayUpdated += PreventAttack;
    }

    public void OnDestroy()
    {
        Events.OnEntityDisplayUpdated -= PreventAttack;
    }

    private void PreventAttack(Entity entity)
    {
        if (entity != target)
        {
            return;
        }

        if (entity.data.hasAttack)
        {
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
        for (var i = 0; i < (waterEffect != null ? waterEffect.count : 0); i++)
        {
            yield return new WaitForSeconds(0.1f);
            yield return Run([hit.target], 0.1f);
        }

        var amount = 1;
        Events.InvokeStatusEffectCountDown(waterEffect, ref amount);
        yield return waterEffect.CountDown(hit.target, amount);
        
        hit.target.display.promptUpdateDescription = true;
        hit.target.PromptUpdate();
    }

    private IEnumerator Run(List<Entity> targets, float delay)
    {
        foreach (var entity in targets)
        {
            VFXHelper.VFX.TryPlayEffect("thunder", entity.transform.position, target.transform.lossyScale,
                GIFLoader.PlayType.damageEffect);
            yield return new Hit(target, entity, count)
            {
                canRetaliate = false,
                countsAsHit = true,
                damageType = type
            }.Process();
        }
        VFXHelper.SFX.TryPlaySound("thunder_attack");
        yield return new WaitForSeconds(delay);
    }
}