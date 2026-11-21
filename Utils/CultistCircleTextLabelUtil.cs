using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPTCultistCircleValuator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;

namespace SPTCultistCircleValuator.Utils
{
    internal class CultistCircleTextLabelUtil
    {
        private const string CULTIST_CIRCLE_FORMATTED_TEXT = "CULTIST CIRCLE";

        public TMP_Text? CultistCircleTextLabel { get; private set; } = null;
        public string OriginalText { get; private set; } = string.Empty;

        private FieldInfo _areaStashFieldInfo;

        public CultistCircleTextLabelUtil()
        {
            _areaStashFieldInfo = AccessTools.Field(typeof(HideoutCircleOfCultistsScreen), "_areaStash");
        }

        public void SetAdditionalText(string text)
        {
            if (CultistCircleTextLabel == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("LocalizedText label for Cultist Circle has not been found");
                return;
            }

            CultistCircleTextLabel.text = OriginalText + " " + text;
        }

        public void UpdateStashValue(HideoutCircleOfCultistsScreen __instance)
        {
            if (CultistCircleTextLabel == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("LocalizedText label for Cultist Circle has not been found");
                return;
            }

            CompoundItem areaStash = (CompoundItem)_areaStashFieldInfo.GetValue(__instance);

            SetAdditionalText(areaStash.Grids[0].Items.Count().ToString());
        }

        public bool TryFindCultistCircleTextLabel()
        {
            CultistCircleTextLabel = null;

            LocalizedText? cultistCircleText = CultistCircleValuatorPlugin.LocalizedTextLocator.FindLocalizedText(CULTIST_CIRCLE_FORMATTED_TEXT);
            if (cultistCircleText == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("Cannot find LocalizedText for Cultist Circle");
                return false;
            }

            CultistCircleTextLabel = cultistCircleText.GetLabel();
            if (CultistCircleTextLabel == null)
            {
                Singleton<LoggingUtil>.Instance.LogError("Cannot find LocalizedText label for Cultist Circle");
                return false;
            }

            if (OriginalText == string.Empty)
            {
                OriginalText = CultistCircleTextLabel.text;
            }

            return true;
        }
    }
}
