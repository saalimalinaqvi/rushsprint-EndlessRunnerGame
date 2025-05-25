using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonView : MonoBehaviour
{
    protected Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    public abstract void OnButtonClick();

    protected void EnableButton(bool on)
    {
        button.interactable = on;
    }
}
