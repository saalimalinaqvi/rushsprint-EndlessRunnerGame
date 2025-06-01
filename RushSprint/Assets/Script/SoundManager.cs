using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource BGAudioSource;
    public AudioSource SFXAudioSource;
    public AudioSource RunningAudioSource;
    public AudioSource LoopingSFXAudioSource;

    [Header("All Sounds")]
    public Sound[] sounds;

    #region Monobehaviour Methods
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    // Helper Methods for Mute State
    private bool IsSoundEnabled() => PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_SOUND, 1) == 1;
    private bool IsMusicEnabled() => PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_MUSIC, 1) == 1;

    /// <summary>
    /// Play Specific Sound 
    /// </summary>
    /// <param name="soundType"></param>
    public void PlayAudio(SoundType soundType)
    {
        if (!IsSoundEnabled() || soundType == SoundType.NONE) return;

        Sound sound = System.Array.Find(sounds, s => s.soundType == soundType);
        if (sound != null && sound.clip != null)
        {
            if (sound.isLoop)
            {
                LoopingSFXAudioSource.clip = sound.clip;
                LoopingSFXAudioSource.volume = sound.volume;
                LoopingSFXAudioSource.loop = true;
                LoopingSFXAudioSource.Play();
            }
            else
            {
                SFXAudioSource.PlayOneShot(sound.clip, sound.volume);
            }
        }
        else
        {
            Debug.LogWarning("Audio Not Found for: " + soundType);
        }
    }

    public void Pause(AudioType audioType)
    {
        if (audioType == AudioType.BG)
            BGAudioSource.Pause();
        else
            RunningAudioSource.Pause();
    }

    /// <summary>
    /// Control Music Playback
    /// </summary>
    /// <param name="audioType"></param>
    public void Play(AudioType audioType)
    {
        if (!IsMusicEnabled()) return;

        if (audioType == AudioType.BG && !BGAudioSource.isPlaying)
            BGAudioSource.Play();
        else if (audioType == AudioType.RUNNING && !RunningAudioSource.isPlaying)
            RunningAudioSource.Play();

        //if (PlayerPrefs.GetInt(Utils.IS_MUTE_GAME_MUSIC) == 1)
        //{
        //    if (audioType == AudioType.BG)
        //        BGAudioSource.Play();
        //    else
        //        RunningAudioSource.Play();
        //}
    }

    /// <summary>
    /// Toggle Music ON/OFF
    /// </summary>
    /// <param name="index"></param>
    public void MuteUnmuteMusic(int index)
    {
        if (index == 1)
        {
            Play(AudioType.BG);
        }
        else
        {
            Pause(AudioType.BG);
            Pause(AudioType.RUNNING);
        }
    }

    public void StopBoostSound()
    {
        if (LoopingSFXAudioSource.isPlaying)
        {
            LoopingSFXAudioSource.Stop();
            LoopingSFXAudioSource.loop = false;
            LoopingSFXAudioSource.clip = null;
        }
    }

    /// <summary>
    /// Stop All Audio Sources
    /// </summary>
    public void StopAllAudio()
    {
        BGAudioSource.Stop();
        RunningAudioSource.Stop();
        SFXAudioSource.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1f;
    public bool isLoop = false;
}

public enum SoundType
{
    NONE,
    COIN_COLLECT,
    GEM_COLLECT,
    SLIDE,
    JUMP,
    SHOOT,
    BOOST,
    BULLET_COLLECT,
    GAMEOVER,
}
public enum AudioType
{
    BG,
    RUNNING
}
