using ScreenUtils.Manager;

namespace ScreenUtils
{
    public class GameScreen : GameUI
    {
        public Screen backScreen;
        public Screen nextScreen;

        protected void ShowNextScreen()
        {
            ScreenManager.ShowScreen(nextScreen);
        }

        public static GameScreen currentScreen;
    }
}
