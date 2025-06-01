using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float score = 0;
    public int coin = 0;
    public int gem = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    public Button mainMenuButton;
    public Button retryButton;
    public Button playAgainButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button pauseMainMenuButton;
    public bool showAd = false;
    private bool isPaused = false;


    #region Monobehaviour Methods

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        HealthSystem playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        playerHealth.OnDeath += PlayerDead;

        // Load last saved values
        score = PlayerPrefs.GetFloat(Utils.SCORE, 0);
        coin = PlayerPrefs.GetInt(Utils.COIN, 0);
        gem = PlayerPrefs.GetInt(Utils.GEM, 0);

        UpdateUI();
        gameOverUI?.SetActive(false);
        pauseMenuUI?.SetActive(false);

        // Assign button listeners
        mainMenuButton?.onClick.AddListener(GoToMainMenu);
        retryButton?.onClick.AddListener(RestartGame);
        playAgainButton?.onClick.AddListener(PlayAgain);
        pauseButton?.onClick.AddListener(PauseGame);
        resumeButton?.onClick.AddListener(ResumeGame);
        pauseMainMenuButton?.onClick.AddListener(GoToMainMenu);
    }

    private void OnDestroy()
    {
        // Remove Listeners
        mainMenuButton?.onClick.RemoveListener(GoToMainMenu);
        retryButton?.onClick.RemoveListener(RestartGame);
        playAgainButton?.onClick.RemoveListener(PlayAgain);
        pauseButton?.onClick.RemoveListener(PauseGame);
        resumeButton?.onClick.RemoveListener(ResumeGame);
        pauseMainMenuButton?.onClick.RemoveListener(GoToMainMenu);
    }

    private void Update()
    {
        score += 10 * Time.deltaTime;
        UpdateUI();
    }

    #endregion


    #region Custom Methods

    public void AddCoins(int amount)
    {
        coin += amount;
        UpdateUI();
    }

    public void AddGems(int amount)
    {
        gem += amount;
        UpdateUI();
    }

    public void LoseCoins()
    {
        coin = 0;
        UpdateUI();
    }

    private void PlayerDead()
    {
        GameOver();
    }

    [System.Obsolete]
    public void PlayerHit(GameObject obj)
    {
        Debug.Log($"Player hit detected , {obj.name} going to game over");
        GameOver();
    }

    [System.Obsolete]
    public void GameOver()
    {
        SaveData();
        gameOverUI.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f; // Pause the game

        //Stop enemy spawning
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.StopAndClearEnemies();

        // ADs
        if (showAd)
        {
            FindObjectOfType<AdMobManager>().ShowInterstitialAd();
            Debug.Log("Game Over! Interstitial Ad Played.");
        }
    }

    public bool IsGameOver()
    {
        return gameOverUI.activeSelf;
    }

    public void RestartGame()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        LoadScene("MainMenu"); // Change this to your Main Menu scene name
    }

    public void PlayAgain()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadScene(object sceneIdentifier)
    {
        SaveData();
        Time.timeScale = 1f;

        if (sceneIdentifier is int index)
            SceneManager.LoadScene(index);
        else if (sceneIdentifier is string name)
            SceneManager.LoadScene(name);
    }

    public void PauseGame()
    {
        if (isPaused) return;

        SoundManager.Instance.Pause(AudioType.RUNNING);
        SoundManager.Instance.Pause(AudioType.BG);

        isPaused = true;
        pauseMenuUI?.SetActive(true);
        pauseButton?.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        SoundManager.Instance.Play(AudioType.RUNNING);

        isPaused = false;
        pauseMenuUI?.SetActive(false);
        pauseButton?.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "Score: " + (int)score;
        if (coinText) coinText.text = "Coin: " + coin;
        if (gemText) gemText.text = "Gem: " + gem;
    }

    private void SaveData()
    {
        PlayerPrefs.SetFloat(Utils.SCORE, score);
        PlayerPrefs.SetInt(Utils.COIN, coin);
        PlayerPrefs.SetInt(Utils.GEM, gem);
        PlayerPrefs.Save();
    }

    #endregion
}