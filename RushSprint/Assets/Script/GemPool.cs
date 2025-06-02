using UnityEngine;
using System.Collections.Generic;

public class GemPool : MonoBehaviour
{
    public GameObject gemPrefab;
    [SerializeField] private int poolSize = 50;

    private List<GameObject> gemPool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject gem = Instantiate(gemPrefab);
            gem.SetActive(false);
            gem.transform.SetParent(this.transform);
            gemPool.Add(gem);
        }
    }

    public GameObject GetPooledGem()
    {
        foreach (GameObject gem in gemPool)
        {
            if (!gem.activeInHierarchy)
                return gem;
        }
       return null;
    }
}
