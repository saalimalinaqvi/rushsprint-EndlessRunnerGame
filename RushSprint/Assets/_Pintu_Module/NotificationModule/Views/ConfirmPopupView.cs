using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Notification
{
    public class ConfirmPopupView : NotificationUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite errorSprite, successSprite;
        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;

        public override void ShowNotification(NotificationInfo info)
        {
            bool isOKOnly = info.callBack == null;
            cancelButton.gameObject.SetActive(!isOKOnly); //show cancel if its not ok only

            if (isOKOnly)
            {
                info.callBack = CancelButtonClick;
            }

            base.ShowNotification(info);
            messageText.text = info.message;
            okButton.onClick.AddListener(() => OKButtonClicked(info.callBack));
            cancelButton.onClick.AddListener(CancelButtonClick);
        }

        private void OKButtonClicked(UnityAction callback)
        {
            callback?.Invoke();
            EnableEnterKey(true);
            CloseNotification();
        }

        private void CancelButtonClick()
        {
            EnableEnterKey(true);
            CloseNotification();
        }

        private void OnDisable()
        {
            okButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
        }
    }
}