using TMPro;
using UnityEngine;

public class HyperlinkButtonView : BaseButtonView
{
    [SerializeField] private TextMeshProUGUI linkTMP;

    public override void OnButtonClick()
    {
        Application.OpenURL(linkTMP.text);
    }
}
