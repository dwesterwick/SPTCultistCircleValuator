using Comfort.Common;
using EFT;
using EFT.HandBook;
using EFT.InventoryLogic;
using EFT.Trading;
using EFT.UI;
using HarmonyLib;
using SPTCultistCircleValuator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;

namespace SPTCultistCircleValuator.Utils
{
    internal class CultistCircleTextLabelUtil
    {
        private const string CULTIST_CIRCLE_LOCALIZATION_KEY = "Hideout/CircleOfCultists";

        private FieldInfo _areaStashFieldInfo;
        private FieldInfo _sessionFieldInfo;
        private TMP_Text? CultistCircleTextLabel = null;
        private string OriginalText = string.Empty;
        private Dictionary<Task, CancellationTokenSource> pendingUpdateStashValueTasks = new Dictionary<Task, CancellationTokenSource>();

        public CultistCircleTextLabelUtil()
        {
            _areaStashFieldInfo = AccessTools.Field(typeof(HideoutCircleOfCultistsScreen), "_areaStash");
            _sessionFieldInfo = AccessTools.Field(typeof(HideoutCircleOfCultistsScreen), "_backEndSession");
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

            CancelAllPendingUpdateStashValueTasks();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task updateStashValueTask = UpdateStashValueAsync(__instance, cancellationTokenSource.Token);
        }

        public void CancelAllPendingUpdateStashValueTasks()
        {
            foreach (Task task in pendingUpdateStashValueTasks.Keys)
            {
                pendingUpdateStashValueTasks[task].Cancel();
            }

            pendingUpdateStashValueTasks.Clear();
        }

        private async Task UpdateStashValueAsync(HideoutCircleOfCultistsScreen __instance, CancellationToken cancellationToken)
        {
            SetAdditionalText(": Calculating...");

            CompoundItem areaStash = (CompoundItem)_areaStashFieldInfo.GetValue(__instance);
            IEftSession session = (IEftSession)_sessionFieldInfo.GetValue(__instance);

            double totalValue = 0;
            foreach (Item item in areaStash.Grids[0].Items)
            {
                double itemPrice = GetHandbookPrice(item, session);

                if ((itemPrice == 0) || (itemPrice == float.NaN))
                {
                    Singleton<LoggingUtil>.Instance.LogWarning("Handbook price for " + item.LocalizedName() + " is not valid; trying trader price instead...");

                    itemPrice = await GetTraderPrice(item, session, cancellationToken);
                }
#if DEBUG
                else
                {
                    double traderPrice = await GetTraderPrice(item, session, cancellationToken);
                    if (traderPrice != itemPrice)
                    {
                        Singleton<LoggingUtil>.Instance.LogWarning("Handbook price for " + item.LocalizedName() + " does not match trader price");
                    }
                }
#endif

                totalValue += itemPrice;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            double valueInThousands = Math.Round(totalValue / 1000.0, 0);

            string valueText = $"{valueInThousands}k ₽";
            string valueTextColored = valueText.SetColor(totalValue.ColorFromValue());

            SetAdditionalText($": {valueTextColored}");

            pendingUpdateStashValueTasks.Clear();
        }

        private async Task<double> GetTraderPrice(Item item, IEftSession session, CancellationToken cancellationToken)
        {
            foreach (Trader trader in session.Traders)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return double.NaN;
                }

                if (trader._supplyData == null)
                {
                    await trader.RefreshAssortment(true, false);
                }

                double traderPrice = PriceCalculator.CalculateBasePriceForAllItems(item, 0, trader._supplyData, trader.Settings.BuyerUp);
                Singleton<LoggingUtil>.Instance.LogDebug(trader.LocalizedName + " price for " + item.LocalizedName() + ": " + traderPrice);

                return traderPrice;
            }

            return double.NaN;
        }

        private float GetHandbookPrice(Item item, IEftSession session)
        {
            List<Item> allContainedItems = new List<Item>();
            item.GetAllItemsNonAlloc(allContainedItems);

            float handbookPrice = 0;
            float totalPrice = 0;
            foreach (Item containedItem in allContainedItems)
            {
                IEnumerable<HandbookData> handbookEntries = session.RagFair._handbook.Items.Where(x => x.Id == containedItem.TemplateId);
                if (handbookEntries.Count() > 1)
                {
                    Singleton<LoggingUtil>.Instance.LogError("More than one handbook entry found for " + containedItem.LocalizedName() + " (" + containedItem.TemplateId + ")");
                    return float.NaN;
                }

                if (handbookEntries.Any())
                {
                    handbookPrice = handbookEntries.First().Price;

#if DEBUG
                    Singleton<LoggingUtil>.Instance.LogDebug("Handbook price for " + containedItem.LocalizedName() + ": " + handbookPrice);
#endif
                }

                totalPrice += handbookPrice;
            }

            Singleton<LoggingUtil>.Instance.LogDebug("Total handbook price for " + item.LocalizedName() + ": " + totalPrice);

            return totalPrice;
        }

        public bool TryFindCultistCircleTextLabel()
        {
            CultistCircleTextLabel = null;

            LocalizedText? cultistCircleText = CultistCircleValuatorPlugin.LocalizedTextLocator.FindLocalizedText(CULTIST_CIRCLE_LOCALIZATION_KEY);
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

            OriginalText = CultistCircleTextLabel.text;

            return true;
        }
    }
}
