using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private bool playMusicOnStart = true;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicStack = new AudioClip[0];
    [SerializeField] private float musicBreak = 2.5f;
    private Coroutine musicCoroutine;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField, Tooltip("Pitch = Random(1 - variety, 1 + variety)")] private float pitchVariety = .15f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);  
    }

    private void Start()
    {
        if(playMusicOnStart)
            PlayMusic();
    }

    #region Playing Music
    public void PlayMusic()
    {
        if (musicCoroutine != null)
            return;

        musicSource.loop = false;
        musicCoroutine = StartCoroutine(MusicCoroutine());
    }

    public void StopMusic()
    {
        if (musicCoroutine == null)
            return;

        StopCoroutine(musicCoroutine);
        musicCoroutine = null;

        musicSource.Stop();
    }

    IEnumerator MusicCoroutine()
    {
        if (musicStack.Length == 0)
            StopMusic();

        int musicIndex = 0;
        while (true)
        {
            AudioClip newMusic = musicStack[musicIndex % musicStack.Length];

            musicSource.clip = newMusic;
            musicSource.Play();

            yield return new WaitForSecondsRealtime(newMusic.length + musicBreak);

            musicIndex++;
        }
    }
    #endregion

    #region Playing SFX
    public void PlaySFX(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        PlaySFX(sfxSource, clips[Random.Range(0, clips.Length)]);
    }

    public void PlaySFX(AudioSource source, AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        PlaySFX(source, clips[Random.Range(0, clips.Length)]);
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(sfxSource, clip);
    }

    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (source == null)
            return;

        source.pitch = Random.Range(1 - pitchVariety, 1 + pitchVariety);
        source.PlayOneShot(clip);
    }
    #endregion
}