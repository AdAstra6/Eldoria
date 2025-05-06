using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip menuTheme;
    public AudioClip gameTheme;
    public AudioClip buttonClick;
    public AudioClip startGameSFX;

    [Header("Game SFX")]
    public AudioClip correctAnswerSFX;
    public AudioClip wrongAnswerSFX;
    public AudioClip scrollOpenSFX;
    public AudioClip diceRollSFX;
    public AudioClip penaltySFX;



    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        PlayMusic(menuTheme);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayStartGameSFX()
    {
        if (startGameSFX != null)
            sfxSource.PlayOneShot(startGameSFX);
    }

    public void PlayCorrectAnswer()
    {
        PlaySFX(correctAnswerSFX);
    }

    public void PlayWrongAnswer()
    {
        PlaySFX(wrongAnswerSFX);
    }

    public void PlayScrollOpen()
    {
        PlaySFX(scrollOpenSFX);
    }
    public void PlayDiceRoll()
    {
        PlaySFX(diceRollSFX);
    }

    public void PlayPenalty()
    {
        PlaySFX(penaltySFX);
    }
}
