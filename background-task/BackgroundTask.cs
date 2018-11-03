using System;
using Windows.Services.Store;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class BackgroundTask : IBackgroundTask
    {
        private StoreContext storeContext = StoreContext.GetDefault();

        /// <summary>
        /// The Run method is the entry point of a background task.
        /// </summary>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // You would add all your fancy logic right here
            // ✨ 🎉 ✨ 🎉 ✨ 🎉✨ 🎉 ✨ 🎉 ✨ 🎉 ✨ 

            ShowToast("Hi, I'm Electron's UWP sidekick");
            //CheckTrialStatus();
        }

        /// <summary>
        /// Show toast notification
        /// </summary>
        private void ShowToast(string msg)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(msg));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(DateTime.Now.ToString()));

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        /// Update the live tile
        /// </summary>
        private void UpdateTile(string msg)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);

            XmlNodeList textNodes = tileXml.GetElementsByTagName("text");
            textNodes[0].InnerText = msg;
            textNodes[1].InnerText = DateTime.Now.ToString("HH:mm:ss");

            TileNotification tileNotification = new TileNotification(tileXml);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        public async void CheckTrialStatus()
        {
            string[] filterList = new string[] { "Consumable", "Durable", "UnmanagedConsumable" };
            StoreAppLicense license = await storeContext.GetAppLicenseAsync();
            //StoreProductQueryResult addOns = await storeContext.GetAssociatedStoreProductsAsync(filterList);
            //ShowToast(addOns.ToString());
            if (license.IsActive)
            {
                if (license.IsTrial)
                {
                    UpdateTile("Trial license");
                }
                else
                {
                    UpdateTile("Full license");
                }
            }
            else
            {
                UpdateTile("This license seems broken!");
                ShowToast("this license ain't working!");
            }
        }
    }
}