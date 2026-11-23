using EFT.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;

namespace SPTCultistCircleValuator.Helpers
{
    public static class TextHelpers
    {
        public static TMP_Text GetLabel(this LocalizedText localizedText)
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

        public static UnityEngine.Color ColorFromValue(this double value)
        {
            if (value > 400000) return UnityEngine.Color.green;
            if (value > 200000) return UnityEngine.Color.yellow;
            if (value > 0) return UnityEngine.Color.red;

            return UnityEngine.Color.white;
        }
    }
}
