using EFT.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SPTCultistCircleValuator.Components
{
    internal class LocalizedTextLocatorComponent : MonoBehaviour
    {
        protected void Awake()
        {
            
        }

        public LocalizedText? FindLocalizedText(string localizationKey)
        {
            foreach (LocalizedText localizedText in FindObjectsOfType<LocalizedText>())
            {
                if (localizedText.LocalizationKey == localizationKey)
                {
                    return localizedText;
                }
            }

            return null;
        }
    }
}
