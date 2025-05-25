using System;
using System.Collections;
using UnityEngine;

namespace Notification
{
    public abstract class NotificationUI : MonoBehaviour
    {
        private NotificationInfo currentNotificationInfo;
        public static Action<Transform> OnAddToCanvas;

        public static Action<bool> OnEnterAccessible;

        public virtual void ShowNotification(NotificationInfo info)
        {
            Debug.Log("ShowNotification: ");
            currentNotificationInfo = info;
            this.gameObject.SetActive(true);
            EnableEnterKey(false);
        }

        protected void CloseNotification()
        {
            this.gameObject.SetActive(false);
            NotificationController.RemoveNotificationFromQueue(currentNotificationInfo);
        }

        protected IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(2.5f);
            EnableEnterKey(true);
            CloseNotification();
        }

        protected void OnDestroy()
        {
            CloseNotification();
        }

        protected void EnableEnterKey(bool on)
        {
            OnEnterAccessible?.Invoke(on);
        }
    }
}
