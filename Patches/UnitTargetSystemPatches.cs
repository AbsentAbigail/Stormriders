using HarmonyLib;
using JetBrains.Annotations;
using Stormriders.Helpers;

namespace Stormriders.Patches;

[HarmonyPatch(typeof(UnitTargetSystem), nameof(UnitTargetSystem.EntityHover), typeof(Entity))]
public class UnitTargetSystemPatches
{
    [UsedImplicitly]
    private static void Postfix(UnitTargetSystem __instance, Entity entity)
    {
        if (__instance.hover is null)
        {
            return;
        }
        if (!entity.inPlay || entity.counter.max <= 0 || !Battle.IsOnBoard(__instance.hover))
        {
            return;
        }

        if (entity.HasAttackIcon() || entity.FindStatus("thunder") is null)
        {
            return;
        }
        
        __instance.ShowTargets(__instance.hover);
    }
}