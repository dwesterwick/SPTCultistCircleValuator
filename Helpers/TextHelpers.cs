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
    }
}
