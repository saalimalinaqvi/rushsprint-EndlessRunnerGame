using System.Collections.Generic;
using UnityEngine;


namespace LNGT.Data
{
    [CreateAssetMenu(fileName = "LoadingText", menuName = "Scriptable Objects/Loading Text Data")]
    public class LoadingTextFactory : ScriptableObject
    {
        public List<LoadingText> loadingTexts;

        public List<string> GetLoadingText(string keyword)
        {
            LoadingText loading = loadingTexts.Find(x => x.keyword.ToLower() == keyword.ToLower());

            if(loading == null)
            {
                return new List<string> { keyword };
            }

            return loading.loadingText;
        }
    }

    [System.Serializable]
    public class LoadingText
    {
        public string keyword;
        public List<string> loadingText;
    }
}
