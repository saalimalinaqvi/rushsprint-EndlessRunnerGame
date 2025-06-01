namespace Notification
{
    public class LoadingView : NotificationUI
    {
        private void OnEnable()
        {
            EnableEnterKey(true);
        }

        private void OnDisable()
        {
            EnableEnterKey(false);
        }

        public void SetLoading(float progress)
        {
        }
    }
}
