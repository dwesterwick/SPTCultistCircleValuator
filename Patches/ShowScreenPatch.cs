using Comfort.Common;
using EFT.UI;
using SPT.Reflection.Patching;
using SPTCultistCircleValuator.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SPTCultistCircleValuator.Patches
{
    internal class ShowScreenPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(HideoutCircleOfCultistsScreen).GetMethod(
                nameof(HideoutCircleOfCultistsScreen.Show),
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new Type[] { typeof(HideoutCircleOfCultistsScreen.HideoutCircleOfCultistsScreenController) },
                null);
        }

        [PatchPostfix]
        protected static void PatchPostfix(HideoutCircleOfCultistsScreen __instance)
        {
            if (Singleton<CultistCircleTextLabelUtil>.Instance.TryFindCultistCircleTextLabel() == false)
            {
                return;
            }

            Singleton<CultistCircleTextLabelUtil>.Instance.UpdateStashValue(__instance);
        }
    }
}
