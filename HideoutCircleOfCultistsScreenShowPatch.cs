using Comfort.Common;
using EFT.UI;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using TMPro;

namespace SPTCultistCircleValuator
{
    internal class HideoutCircleOfCultistsScreenShowPatch : ModulePatch
    {
        private const string CULTIST_CIRCLE_FORMATTED_TEXT = "CULTIST CIRCLE";

        private static string _originalText = string.Empty;

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
            TMP_Text? cultistCircleTextLabel = GetCultistCircleTextLabel();
            if (cultistCircleTextLabel == null)
            {
                return;
            }

            cultistCircleTextLabel.text = _originalText + " TEST";

        }

        private static TMP_Text? GetCultistCircleTextLabel()
        {
            LocalizedText? cultistCircleText = CultistCircleValuatorPlugin.LocalizedTextLocator.FindLocalizedText(CULTIST_CIRCLE_FORMATTED_TEXT);
            if (cultistCircleText == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("Cannot find LocalizedText for Cultist Circle");
                return null;
            }

            TMP_Text? cultistCircleTextLabel = GetLabelForLocalizedText(cultistCircleText);
            if (cultistCircleTextLabel == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("Cannot find LocalizedText label for Cultist Circle");
                return null;
            }

            if (_originalText == string.Empty)
            {
                _originalText = cultistCircleTextLabel.text;
            }

            return cultistCircleTextLabel;
        }

        private static TMP_Text GetLabelForLocalizedText(LocalizedText localizedText)
        {
            foreach (TextMeshProUGUI label in localizedText._labels)
            {
                if (!((UnityEngine.Object)(object)label == null))
                {
                    return (TMP_Text)label;
                }
            }

            throw new InvalidOperationException("Cannot find TMP_Text for LocalizedText " + localizedText.gameObject.name);
        }
    }
}
