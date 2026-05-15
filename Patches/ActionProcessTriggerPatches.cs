using System.Collections;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;

namespace Stormriders.Patches;

[HarmonyPatch(typeof(ActionProcessTrigger), nameof(ActionProcessTrigger.Run))]
public class ActionProcessTriggerPatches
{
    [UsedImplicitly]
    private static bool Prefix(ref IEnumerator __result, ActionProcessTrigger __instance)
    {
        __result = Process(__instance); // Replace Process to add check for Thunder to prevent targets being nulled
        return false;
    }

    private static IEnumerator Process(ActionProcessTrigger instance)
    {
        if (instance.trigger == null && instance.GetTriggerMethod != null)
            instance.trigger = instance.GetTriggerMethod();
        Events.InvokeEntityPreTrigger(ref instance.trigger);
        yield return StatusEffectSystem.PreTriggerEvent(instance.trigger);
        var num = instance.trigger.entity.HasAttackIcon() ? 1 : 0;
        // this line is changed v
        if (num == 0 && instance.trigger.entity.attackEffects.Count <= 0 && instance.trigger.entity.FindStatus("thunder") is null)
            instance.trigger.targets = null;
        if (num != 0)
        {
            var targets = instance.trigger.targets;
            if (targets is not { Length: > 0 } && NoTargetTextSystem.Exists())
                yield return NoTargetTextSystem.Run(instance.trigger.entity, NoTargetType.NoTargetToAttack);
        }
        if (instance.trigger.targets != null)
            instance.trigger.targets = instance.trigger.targets.Where(t => t.IsAliveAndExists()).ToArray();
        instance.trigger.entity.triggeredBy = instance.trigger.triggeredBy;
        Events.InvokeEntityTrigger(ref instance.trigger);
        if (!instance.trigger.nullified && !instance.trigger.entity.IsSnowed)
        {
            var targets = instance.trigger.targets;
            if (targets is { Length: > 0 })
            {
                yield return instance.trigger.Process();
                yield return Sequences.Wait(0.167f);
            }
            else
                yield return instance.trigger.Process();
        }
        Events.InvokeEntityTriggered(ref instance.trigger);
        instance.trigger.entity.triggeredBy = null;
    }
}