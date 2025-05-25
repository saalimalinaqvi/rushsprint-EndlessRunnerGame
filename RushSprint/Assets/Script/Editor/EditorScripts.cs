using UnityEditor;
using UnityEditor.SceneManagement;

class EditorScripts : EditorWindow
{
    [MenuItem("Scene/Play/Splash _%h")]
    public static void RunMainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity");
        EditorApplication.isPlaying = true;
    }

    [MenuItem("SelectScene/Select")]
    public static void SetScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainLevel.unity");
    }
}