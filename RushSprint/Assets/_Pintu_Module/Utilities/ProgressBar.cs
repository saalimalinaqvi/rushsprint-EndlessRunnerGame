using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ProgressBar<T> : MonoBehaviour
{
    public static ProgressBar<T> Instance;
    protected T maxValue;
    [SerializeField] protected Slider m_Slider;
    [SerializeField] protected TextMeshProUGUI m_PercentageText;

    private void Awake()
    {
        Instance = this;
    }

    public static void Init(T total)
    {
        Instance.maxValue = total;
    }

    public static void Value(T value)
    {
        Instance.ShowProgress(value);
    }

    protected abstract void ShowProgress(T value);
}
