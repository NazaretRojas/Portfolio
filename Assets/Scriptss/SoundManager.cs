using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioClip buttonClick;

    [SerializeField] private AudioClip bulletShot;
    [SerializeField] private AudioClip burnEffect;
    [SerializeField] private AudioClip freezeEffect;
    [SerializeField] private AudioClip poisonEffect;

    public AudioClip menuMusicClip;
    public AudioClip gameMusicClip;

    public float MusicVolume
    {
        get => musicSource.volume;
        set => musicSource.volume = Mathf.Clamp01(value);
    }

    public float SfxVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = Mathf.Clamp01(value);
    }

   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   
    private void Start()
    {
        PlayMenuMusic();
    }

    public void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        SfxVolume = volume;
    }
    public void PlayMenuMusic()
    {
        if (musicSource.clip != menuMusicClip)
        {
            musicSource.clip = menuMusicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (musicSource.clip != gameMusicClip)
        {
            musicSource.clip = gameMusicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string[] gameScenes = {
            "onegameEasy", "onegameMedium", "onegameHard",
            "twogameEasy", "twogameMedium", "twogameHard",
            "threegameEasy", "threegameMedium", "threegameHard"
        };

        if (System.Array.Exists(gameScenes, s => s == scene.name))
        {
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    public void PlayButtonClick() => PlaySound(buttonClick);
    public void PlayBulletShot() => PlaySound(bulletShot);
    public void PlayBurnEffect() => PlaySound(burnEffect);
    public void PlayFreezeEffect() => PlaySound(freezeEffect);
    public void PlayPoisonEffect() => PlaySound(poisonEffect);

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
