using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Notification
{
    public class AutoPopupView : NotificationUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite errorSprite, successSprite;

        public override void ShowNotification(NotificationInfo info)
        {
            base.ShowNotification(info);
            Debug.Log("AutoPopupView  ShowNotification: ");
            messageText.text = info.message;
            StartCoroutine(AutoHide());
        }
    }
}
