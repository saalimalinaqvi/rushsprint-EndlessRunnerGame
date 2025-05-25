using ScreenUtils.Manager;
using UnityEngine;

public class ShowScreenButtonView : BaseButtonView
{
    [SerializeField] private ScreenUtils.Screen screen;
    public override void OnButtonClick()
    {
        ScreenManager.ShowScreen(screen);
    }
}



