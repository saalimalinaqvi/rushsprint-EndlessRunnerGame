using UnityEngine;

public class CloseButtonView : BaseButtonView
{
    [SerializeField] private GameObject parentPanel;

    public override void OnButtonClick()
    {
        parentPanel.SetActive(false);
    }
}

