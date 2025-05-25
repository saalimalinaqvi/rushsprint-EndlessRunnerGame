using System.Collections;
using System.Collections.Generic;
using LNGT.Data;
using TMPro;
using UnityEngine;

public class LoadingMessageView : MonoBehaviour
{
    [SerializeField] private LoadingTextFactory loadingTextFactory;

    [SerializeField] private TextMeshProUGUI myTMP;
    Coroutine showLoadingCoroutine;

    public void Init(string sceneName)
    {
        if (loadingTextFactory == null || string.IsNullOrEmpty(sceneName))
        {
            return;
        }

        myTMP.gameObject.SetActive(true);
                
        StopLoadingCoroutine();
        if (this.isActiveAndEnabled)
        {
            showLoadingCoroutine = StartCoroutine(ShowLoadingText(sceneName));
        }
    }

    protected IEnumerator ShowLoadingText(string sceneName)
    {
        List<string> loadingText = loadingTextFactory.GetLoadingText(sceneName);

        int i = 0;
        myTMP.text = loadingText[0];
        while (true)
        {
            i = i % loadingText.Count;
            myTMP.text = loadingText[i];
            yield return new WaitForSeconds(1f);
            i++;
        }
    }

    private void OnDisable()
    {
        StopLoadingCoroutine();
    }

    private void StopLoadingCoroutine()
    {
        if (showLoadingCoroutine != null)
        {
            StopCoroutine(showLoadingCoroutine);
        }
    }
}
