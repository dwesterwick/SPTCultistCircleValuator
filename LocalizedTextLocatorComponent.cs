using EFT.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SPTCultistCircleValuator
{
    internal class LocalizedTextLocatorComponent : MonoBehaviour
    {
        protected void Awake()
        {
            
        }

        public LocalizedText? FindLocalizedText(string formattedText)
        {
            foreach (LocalizedText localizedText in FindObjectsOfType<LocalizedText>())
            {
                if (localizedText.FormattedText == formattedText)
                {
                    return localizedText;
                }
            }

            return null;
        }
    }
}
