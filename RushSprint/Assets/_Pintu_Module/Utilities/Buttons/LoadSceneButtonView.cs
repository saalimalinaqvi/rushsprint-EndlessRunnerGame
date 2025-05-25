using UnityEngine;

public class LoadSceneButtonView : BaseButtonView
{
    [SerializeField] protected SceneProperties sceneProperties;

    public override void OnButtonClick()
    {
        Time.timeScale = 1f;
        SceneController.LoadScene(sceneProperties);
    }
}
