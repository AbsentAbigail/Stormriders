#region

using System.Collections;
using Deadpan.Enums.Engine.Components.Modding;
using Stormriders.Builders.StatusEffects;
using Stormriders.GameSystems;
using Stormriders.Helpers;
using UnityEngine;

#endregion

namespace Stormriders.StatusEffectImplementations;

public class StatusEffectInstantChargeBell : StatusEffectInstant
{
    public bool fully = true;

    public override IEnumerator Process()
    {
        var redrawBellSystem = FindObjectOfType<RedrawBellSystem>(true);
        if (redrawBellSystem == null)
        {
            LogHelper.Log("No RedrawBellSystem found");
            yield return base.Process();
        }

        FindObjectOfType<ChargeRedrawBellSystem>(true)
            .ChargeRedrawBell(fully ? 0 : GetAmount());
        var localisedString = LocalizationHelper.GetCollection("UI Text", SystemLanguage.English).GetString(fully ? "ChargeBellFully" : "ChargeBell");
        FindObjectOfType<BattleLogSystem>()?.Log(localisedString, BattleLogType.Buff, BattleLogSystem.GetBattleEntity(applier), GetAmount());
        yield return Remove();
    }
}