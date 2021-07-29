using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Extensione.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioClip buttonSFX;

        private AudioSource audioMusic;
        private AudioSource audioSFX;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(Instance);

            audioMusic = gameObject.AddComponent<AudioSource>();
            audioSFX = gameObject.AddComponent<AudioSource>();

            audioMusic.playOnAwake = false;
            audioSFX.playOnAwake = false;

            audioMusic.loop = true;
            audioSFX.loop = false;

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void Update()
        {
            TransitionBetweenMusic();
        }

        public void PlaySFXOnce(AudioClip clip)
        {
            if (audioSFX)
            {
                audioSFX.PlayOneShot(clip);
            }
        }

        public void PlayMusicDelayed(AudioClip clip, float delay)
        {
            if (audioMusic)
            {
                audioMusic.clip = clip;
                audioMusic.PlayDelayed(delay);
            }
        }

        public void PlayMusic(AudioClip clip = null)
        {
            if (audioMusic)
            {
                if (clip)
                {
                    audioMusic.clip = clip;
                }
                audioMusic.Play();
            }
        }

        private int fadingState = 0; //0 nothing, 1 fade out, 2 fade in, 0 done
        private AudioClip fadingClipTarget;

        public void ChangeMusic(AudioClip clip)
        {
            if (audioMusic)
            {
                fadingState = 1;
                fadingClipTarget = clip;
            }
        }

        public void ChangeMusicSilent(AudioClip clip)
        {
            if (audioMusic)
            {
                if (fadingClipTarget != clip)
                {
                    ChangeMusic(clip);
                }
            }
        }

        private void TransitionBetweenMusic()
        {
            if (audioMusic)
            {
                if (fadingState == 1) //fade out
                {
                    audioMusic.volume += (0.0f - audioMusic.volume) * 0.1f;

                    if (audioMusic.volume <= 0.02f)
                    {
                        fadingState = 2;
                        audioMusic.clip = fadingClipTarget;
                        audioMusic.Play();
                    }
                }
                else if (fadingState == 2) //fade in
                {
                    audioMusic.volume += (1.0f - audioMusic.volume) * 0.1f;

                    if (audioMusic.volume >= 0.98f)
                    {
                        fadingState = 0;
                        audioMusic.volume = 1.00f;
                    }
                }
            }
        }

        public void PauseMusic()
        {
            if (audioMusic)
            {
                audioMusic.Pause();
            }
        }

        public void UnPauseMusic()
        {
            if (audioMusic)
            {
                audioMusic.UnPause();
            }
        }

        public void StopMusic()
        {
            if (audioMusic)
            {
                audioMusic.Stop();
            }
        }

        public void ChangeMusicVolume(float volume)
        {
            audioMusic.volume = volume;
        }

        public void ChangeSFXVolume(float volume)
        {
            audioSFX.volume = volume;
        }

        public float GetMusicVolume()
        {
            return audioMusic.volume;
        }

        public float GetSFXVolume()
        {
            return audioSFX.volume;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (!audioMusic.isPlaying)
            {
                audioMusic.Play();
            }

            foreach (Button b in FindObjectsOfType<Button>())
            {
                b.onClick.AddListener(ButtonClick);
            }
        }

        private void ButtonClick()
        {
            PlaySFXOnce(buttonSFX);
        }
    }
}