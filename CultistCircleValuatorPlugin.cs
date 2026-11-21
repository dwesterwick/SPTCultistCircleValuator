using BepInEx;
using Comfort.Common;
using System;

namespace SPTCultistCircleValuator
{
    [BepInPlugin("com.danw.cultistcirclevaluator", "CultistCircleValuator", "1.0.0")]
    public class CultistCircleValuatorPlugin : BaseUnityPlugin
    {
        internal static LocalizedTextLocatorComponent LocalizedTextLocator = null!;

        protected void Awake()
        {
            Logger.LogInfo("Loading CultistCircleValuator...");

            Singleton<LoggingUtil>.Create(new LoggingUtil(Logger));

            LocalizedTextLocator = this.GetOrAddComponent<LocalizedTextLocatorComponent>();
            new HideoutCircleOfCultistsScreenShowPatch().Enable();

            Singleton<LoggingUtil>.Instance.LogInfo("Loading CultistCircleValuator...done.");
        }
    }
}
