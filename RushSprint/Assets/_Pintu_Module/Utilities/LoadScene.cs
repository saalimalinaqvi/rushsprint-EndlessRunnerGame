using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private SceneProperties sceneProperties;

    // Start is called before the first frame update
    void Start()
    {
        SceneController.LoadScene(sceneProperties);
    }
}
