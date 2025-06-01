using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressView : MonoBehaviour
{
    [SerializeField] protected GameObject loadinPanel;
    [SerializeField] protected TextMeshProUGUI loadingText;
    [SerializeField] protected Slider slider;
    [SerializeField] protected LoadingMessageView loadingMessageView;


    public GameObject LoadingGameObject => loadinPanel;

    public void SetLoading(float progress)
    {
        loadingText.text =  "Loading..." + (progress).ToString("F0") + "%";
        loadinPanel.SetActive(true);        
        slider.value = progress;
    }
    
    public void SetLoadingText(bool status)
    {
        slider.value = 0;
        loadingText.text = "";
        loadingText.gameObject.SetActive(status);
        slider.gameObject.SetActive(status);      
    }

    public void SetLoadingText(string message)
    {
        loadingText.text = message;
        loadingText.gameObject.SetActive(true);
        slider.gameObject.SetActive(false);
    }

    public void InitLoading(string sceneName)
    {
        SetLoading(0);
        if (loadingMessageView != null)
        {
            loadingMessageView.Init(sceneName);
        }
    }

    public void OnDisable()
    {
        loadingText.text = "";
        slider.gameObject.SetActive(false);
        slider.value = 0f;
    }
}