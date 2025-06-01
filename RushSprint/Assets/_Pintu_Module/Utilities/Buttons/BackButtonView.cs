using ScreenUtils.Manager;

public class BackButtonView : BaseButtonView
{
    public override void OnButtonClick()
    {
        ScreenManager.ShowBackScreen();
    }
}

