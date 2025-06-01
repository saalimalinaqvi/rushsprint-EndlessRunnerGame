using System.Collections.Generic;
using UnityEngine;


namespace LNGT.Data
{
    [CreateAssetMenu(fileName = "SingleLoadingText", menuName = "Scriptable Objects/Single Loading Text Data")]
    public class SingleLoadingTextFactory : ScriptableObject
    {
        public List<SingleLoadingText> loadingTexts;

        public string GetLoadingText(string keyword)
        {
            SingleLoadingText loading = loadingTexts.Find(x => x.keyword.ToLower() == keyword.ToLower());

            if(loading == null)
            {
                return string.Empty;
            }

            return loading.loadingText;
        }
    }

    [System.Serializable]
    public class SingleLoadingText
    {
        public string keyword;
        public string  loadingText;
    }
}
