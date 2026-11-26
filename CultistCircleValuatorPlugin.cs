using BepInEx;
using Comfort.Common;
using SPTCultistCircleValuator.Utils;
using System;

namespace SPTCultistCircleValuator
{
    [BepInPlugin("com.danw.cultistcirclevaluator", "CultistCircleValuator", "1.0.1")]
    public class CultistCircleValuatorPlugin : BaseUnityPlugin
    {
        internal static Components.LocalizedTextLocatorComponent LocalizedTextLocator = null!;

        protected void Awake()
        {
            Logger.LogInfo("Loading CultistCircleValuator...");

            Singleton<LoggingUtil>.Create(new LoggingUtil(Logger));
            Singleton<CultistCircleTextLabelUtil>.Create(new CultistCircleTextLabelUtil());

            LocalizedTextLocator = this.GetOrAddComponent<Components.LocalizedTextLocatorComponent>();

            new Patches.ShowScreenPatch().Enable();
            new Patches.HideScreenPatch().Enable();
            new Patches.SetInteractivityAndFinishCraftPatch().Enable();

            Singleton<LoggingUtil>.Instance.LogInfo("Loading CultistCircleValuator...done.");
        }
    }
}
