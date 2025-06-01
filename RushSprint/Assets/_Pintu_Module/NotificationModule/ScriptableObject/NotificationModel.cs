using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Notification
{
    [CreateAssetMenu(fileName = "NotificationData", menuName = "Scriptable Objects/Notification Data")]
    public class NotificationModel : ScriptableObject
    {
        [SerializeField] private GameObject autoHidePopup;
        [SerializeField] private GameObject confirmPopup;
        [SerializeField] private GameObject loadingPopup;

        private GameObject autoHidePopupObj;

        public List<NotificationInfo> data;

        public NotificationInfo GetInfo(string key)
        {
            NotificationInfo info = data.Find(x => x.key == key);
            if (info == null) return null;
            return info;
        }

        public void ShowNotification(NotificationInfo info)
        {
            if (info.isAutoHide)
            {
                CreateAutoHidePopup(info);
            }
            else
            {
                CreateConfirmPopup(info);
            }
        }

        private void CreateAutoHidePopup(NotificationInfo info)
        {
            Debug.Log("CreateAutoHidePopup: ");
            if (autoHidePopupObj == null)
            {
                GameObject popupObj = Instantiate(autoHidePopup) as GameObject;
                popupObj.name = "Notification";
                autoHidePopupObj = popupObj;
            }

            NotificationUI notificationUI = autoHidePopupObj.GetComponent<NotificationUI>();
            notificationUI.ShowNotification(info);
        }

        private void CreateConfirmPopup(NotificationInfo info)
        {
            GameObject popupObj = Instantiate(confirmPopup) as GameObject;
            popupObj.name = "Confirm";
            popupObj.GetComponent<NotificationUI>().ShowNotification(info);
        }

        public GameObject CreateLoading()
        {
            GameObject popupObj = Instantiate(loadingPopup) as GameObject;
            popupObj.name = "Loading";
            popupObj.GetComponent<NotificationUI>().ShowNotification(null);
            return popupObj;
        }
    }

    [System.Serializable]
    public class NotificationInfo
    {
        public string key;
        public string message;
        public string title;
        public string buttonText;
        public Sprite image;
        public bool isSuccess;
        public bool isAutoHide;
        public UnityAction callBack;
    }
}
