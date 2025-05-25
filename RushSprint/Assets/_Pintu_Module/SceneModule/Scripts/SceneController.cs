using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    public static SceneController m_Instance;
    public static Action<string> OnNewLevelLoaded;
    public static Action<string> OnUnloadSceneCompleted;
    public static string CurrentScene;

    [SerializeField] protected GameObject loadingCanvas;
    [SerializeField] protected LoadingProgressView loadingView;

    public LoadingProgressView LoadingProgressView => loadingView;
    protected Coroutine showLoadingCoroutine = null;

    #region staticFunctions

    public static void RestartScene()
    {
    }

    public static void ReloadAdditiveScene(Scenes scene)
    {

        SceneProperties sceneProperties = new SceneProperties()
        {
            isAsync = true,
            loadSceneMode = LoadSceneMode.Additive,
            sceneName = scene            
        };

        m_Instance.StartCoroutine(m_Instance.WaitForUnloadScene(scene.ToString(), LoadScene, sceneProperties));
    }

    public static void ReloadCurrentScene()
    {
        SceneProperties sceneProperties = new SceneProperties() { 
            isAsync = true, 
            loadSceneMode = LoadSceneMode.Single, 
            sceneName = (Scenes)Enum.Parse(typeof(Scenes), CurrentScene),
            showLoading = true 
        };

        LoadScene(sceneProperties);
    }

    public static void LoadScene<T>(T sceneName, Action callback = null)
    {
        m_Instance.ProcessLoadScene(new SceneProperties(sceneName.ToString()), callback);
    }

    public static void LoadScene(SceneProperties sceneProperties, Action callback = null)
    {
        m_Instance.ProcessLoadScene(sceneProperties, callback);
    }

    public static void UnloadScene(SceneProperties sceneProperties, Action callback = null)
    {
        m_Instance.ProcessUnLoadScene(sceneProperties, callback);
    }
    #endregion

    #region lifecycle

    private void Awake()
    {
        m_Instance = this;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        //PlayerData.Create();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }


    private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(scene);
        OnNewLevelLoaded?.Invoke(scene.name);
        CurrentScene = scene.name;
        
        //if (AudioManager.Instance != null)
        //{
        //    AudioManager.PlayMusic(scene.name);
        //}

#if UNITY_EDITOR && UNITY_WEBGL
        RefreshMaterials();
#endif
    }
    #endregion

    #region addressableHelper
    private void RefreshMaterials()
    {
        try
        {
            //foreach (Type t in textureReload)
            //{                 
            var gameObjs = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI obj in gameObjs)
            {
                var shaderName = obj.fontSharedMaterial.shader.name;
                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    obj.fontSharedMaterial.shader = newShader;
                }
            }

            var gameObjs2 = FindObjectsOfType<MeshRenderer>();
            foreach (MeshRenderer obj in gameObjs2)
            {

                var shaderName = obj.material.shader.name;
                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    obj.material.shader = newShader;
                }
            }

            var gameObjs3 = FindObjectsOfType<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer obj in gameObjs3)
            {

                var shaderName = obj.material.shader.name;
                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                {
                    obj.material.shader = newShader;
                }
            }

            //}          
        }
        catch (Exception e)
        {
        }
    }

    #endregion

    #region loadScene
    public virtual void ProcessLoadScene(SceneProperties sceneProperties, Action callback = null, bool callMyLoadScene = true)
    {
        string sceneName = sceneProperties.sceneName.ToString();

        Debug.Log("load scene ============" + sceneName);

        if (sceneProperties.isAsync)
        {
            if (callMyLoadScene)
            {
                StartCoroutine(LoadSceneAsync(sceneName, sceneProperties, callback));
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName, sceneProperties.loadSceneMode);
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName, sceneProperties.loadSceneMode);
        }
        callback?.Invoke();
    }
           
    protected virtual IEnumerator LoadSceneAsync(string sceneName, SceneProperties sceneProperties, Action callback)
    {
        yield return StartCoroutine(LoadScene(sceneName, sceneProperties, callback));

        if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }

        callback?.Invoke();
    }

    protected IEnumerator LoadScene(string sceneName, SceneProperties sceneProperties, Action callback)
    {
        yield return StartCoroutine(ShowLoading(sceneProperties));

        //Debug.Log("called again");       
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, sceneProperties.loadSceneMode);

        float dummyProgress = 0.1f;

        while (op.progress < 1)
        {
            float progress = (op.progress < 9) ? dummyProgress : op.progress;
            dummyProgress += 0.1f;
            dummyProgress = Mathf.Clamp(dummyProgress, 0f, 0.9f);

            loadingView?.SetLoading(progress * 100);
            yield return null;
        }

        loadingView?.SetLoading(1f * 100);
        yield return new WaitForSeconds(1f);
    }
    #endregion

    #region unloadScene
    public virtual void ProcessUnLoadScene(SceneProperties sceneProperties, Action callback = null)
    {
        string sceneName = sceneProperties.sceneName.ToString();

        Debug.Log("load scene ============" + sceneName);

        if (sceneProperties.isAsync)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        callback?.Invoke();
    }

    private IEnumerator WaitForUnloadScene(string sceneName, Action<SceneProperties, Action> callback, SceneProperties properties)
    {
        AsyncOperation sceneUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (sceneUnload != null && !sceneUnload.isDone)
        {
            yield return null;
        }

        callback?.Invoke(properties, null); 
    }
    #endregion

    #region showLoading
    public static void ShowLoading(bool status, bool isMessageShow = false)
    {
        Debug.Log("show loading ... :" + status);
        m_Instance.loadingCanvas?.SetActive(status);
        m_Instance.loadingView.SetLoadingText(isMessageShow);        
    }

    public IEnumerator ShowLoading(SceneProperties sceneProperties)
    {
        if (sceneProperties.showLoading)
        {        
            loadingView.InitLoading(sceneProperties.sceneName.ToString());            
            yield return new WaitForSeconds(1f);
        }
        else
        {
            loadingCanvas?.SetActive(false);
        }
    }
    #endregion
}

[System.Serializable]
public class SceneProperties
{
    public Scenes sceneName;
    public LoadSceneMode loadSceneMode;
    public bool isAsync;
    public bool showLoading;
    public bool keepLoadingOn;

    public SceneProperties()
    {

    }

    public SceneProperties(string sceneName)
    {
        Scenes scene = (Scenes) Enum.Parse(typeof(Scenes), sceneName);
        this.sceneName = scene;
        loadSceneMode = LoadSceneMode.Single;
        isAsync = true;
        showLoading = true;

    }

    public SceneProperties(string sceneName, bool isAdditive, bool keepLoadingOn)
    {
        Scenes scene = (Scenes)Enum.Parse(typeof(Scenes), sceneName);
        this.sceneName = scene;
        this.keepLoadingOn = keepLoadingOn;
        if (isAdditive)
        {
            loadSceneMode = LoadSceneMode.Additive;
            isAsync = false;
            showLoading = false;
        }
        else
        {
            loadSceneMode = LoadSceneMode.Single;
            isAsync = true;
            showLoading = true;
        }
    }
}

public enum Scenes
{
    None,
    Splash,
    MainMenu,
    MainLevel
}

