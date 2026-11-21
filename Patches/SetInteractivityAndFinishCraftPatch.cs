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
    internal class SetInteractivityAndFinishCraftPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(HideoutCircleOfCultistsScreen).GetMethod(nameof(HideoutCircleOfCultistsScreen.SetInteractivityAndFinishCraft), BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        protected static void PatchPostfix(HideoutCircleOfCultistsScreen __instance)
        {
            Singleton<CultistCircleTextLabelUtil>.Instance.UpdateStashValue(__instance);
        }
    }
}
