using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.StatusEffectImplementations;

namespace Stormriders.Patches;

[HarmonyPatch]
public class TargetConstraintDoesAttackPatches
{
    [UsedImplicitly, HarmonyPostfix, HarmonyPatch(typeof(TargetConstraintDoesAttack), nameof(TargetConstraintDoesAttack.Check), typeof(Entity))]
    private static bool CheckEntity(bool result, TargetConstraintDoesAttack __instance, Entity target)
    {
        if (result != __instance.not)
        {
            return result;
        }
        
        if (target.statusEffects.Any(status => status is StatusEffectThunder))
        {
            return !__instance.not;
        }

        return result;
    }
    
    [UsedImplicitly, HarmonyPostfix, HarmonyPatch(typeof(TargetConstraintDoesAttack), nameof(TargetConstraintDoesAttack.Check), typeof(CardData))]
    private static bool CheckEntity(bool result, TargetConstraintDoesAttack __instance, CardData targetData)
    {
        if (result != __instance.not)
        {
            return result;
        }
        
        if (targetData.startWithEffects.Any(status => status.data is StatusEffectThunder))
        {
            return !__instance.not;
        }

        return result;
    }
}