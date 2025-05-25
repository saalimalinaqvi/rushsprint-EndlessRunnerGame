using LNGT;
using LNGT.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Notification
{
    public class NotificationController : MonoBehaviour
    {
        [SerializeField] bool showVersion = false;
        public TextMeshProUGUI versionText;

        [SerializeField] private GameObject loadingPopup;
        [SerializeField] private NotificationUI autoHidePopup;
        [SerializeField] private NotificationUI confirmPopup;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private SingleLoadingTextFactory loadingTextData;

        public static NotificationController Instance;
        public static Action<NotificationInfo> OnShowNotification;
        public List<NotificationInfo> notificationQueue;

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            //   versionText.text = showVersion ? "V " + Application.version : "";
            notificationQueue = new List<NotificationInfo>();
        }

        public static void Notify(string message, bool isSuccess = false)
        {
            ShowLoading(false);
            Debug.Log("Notify, " + message);

            NotificationInfo info = new NotificationInfo();
            info.message = ServerUtils.IsEmpty(message) ? string.Empty : message.Trim();
            info.isAutoHide = true;
            info.isSuccess = isSuccess;

            if (Application.isPlaying)
            {
                Instance.AddToNotificationQueue(info);
            }
        }

        public static void ConfirmPopup(string message, string title, bool isSuccess, UnityAction callback)
        {
            NotificationInfo info = new NotificationInfo();
            info.message = message.Trim();
            info.title = title.Trim();
            info.isAutoHide = false;
            info.isSuccess = isSuccess;
            info.callBack = callback;

            Instance.AddToNotificationQueue(info);
        }

        private void AddToNotificationQueue(NotificationInfo info)
        {
            notificationQueue.Add(info);

            if (notificationQueue.Count == 1)
            {
                ShowNotification(info);
            }
        }

        public static void RemoveNotificationFromQueue(NotificationInfo info)
        {
            Instance.notificationQueue.Remove(info);

            if (Instance.notificationQueue.Count > 0)
            {
                Instance.ShowNotification(Instance.notificationQueue[0]);
            }
        }

        public static void ShowLoading(bool show, string api_name = "")
        {
            Debug.Log("show loading : " + show);
            if (Instance)
            {
                Instance.loadingPopup.SetActive(show);

                string msg = "";
                if (Instance.loadingTextData != null)
                {
                    msg = Instance.loadingTextData.GetLoadingText(api_name);
                }

                Instance.messageText.text = msg;
            }
        }

        public void ShowNotification(NotificationInfo info)
        {
            NotificationUI popup;
            popup = (info.isAutoHide) ? autoHidePopup : confirmPopup;
            popup.ShowNotification(info);
        }
    }
}
