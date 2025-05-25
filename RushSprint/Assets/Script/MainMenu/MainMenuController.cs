using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private Toggle musicToggle;
    private int totalCoins;
    private int totalGems;

    private void OnEnable()
    {
        totalCoins = PlayerPrefs.GetInt(Utils.COIN, 0);
        totalGems = PlayerPrefs.GetInt(Utils.GEM, 0);

        UpdateToggles();
        UpdateUI();

        SoundManager.Instance.MuteUnmuteMusic(PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_MUSIC));
    }

    private void UpdateToggles()
    {
        soundToggle.isOn = PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_SOUND) == 1 ? true : false;
        musicToggle.isOn = PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_MUSIC) == 1 ? true : false;
    }

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        settingButton.onClick.AddListener(Setting);

        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(Play);
        settingButton.onClick.RemoveListener(Setting);

        soundToggle.onValueChanged.RemoveListener(OnSoundToggleChanged);
        musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
    }

    private void Play()
    {
        SceneController.LoadScene(new SceneProperties{ sceneName = Scenes.MainLevel, isAsync = true });
    }

    private void Setting()
    {
    }

    private void OnMusicToggleChanged(bool isOn)
    {
        int isMuteGameMusic = isOn ? 1 : 0;
        PlayerPrefs.SetInt(Utils.IS_MUTE_GAME_MUSIC, isMuteGameMusic);

        SoundManager.Instance.MuteUnmuteMusic(PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_MUSIC));
    }

    private void OnSoundToggleChanged(bool isOn)
    {
        int isMuteGameSound = isOn ? 1 : 0;
        PlayerPrefs.SetInt(Utils.IS_MUTE_GAME_SOUND, isMuteGameSound);
    }

    private void UpdateUI()
    {
        coinText.text = totalCoins.ToString();
        gemText.text = totalGems.ToString();
    }
}
