using UnityEngine;

public class ProgressBarView : ProgressBar<int>
{
    protected override void ShowProgress(int value)
    {
        value = maxValue - value;
        m_Slider.value = (float)value / maxValue;
        m_PercentageText.text = "Downloading... " + Mathf.Round(m_Slider.value * 100) + " %";
    }
}
