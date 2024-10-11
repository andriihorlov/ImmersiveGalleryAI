using System;
using ImmersiveGalleryAI.Common.PlayerLocation;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace ImmersiveGalleryAI.Common.AudioSystem
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource _sfxAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private bool _isMusicLooped;
        [Header("Settings")] 
        [SerializeField] private float _sfxVolume;
        [SerializeField] private float _musicVolume;

        [Space] 
        [SerializeField] private AudioClip[] _buttonsClickAudio;
        [SerializeField] private AudioClip _musicAudio;
        
        [Inject] private IAudioSystem _audioSystem;
        [Inject] private IPlayerLocation _playerLocation;

        private void Awake()
        {
            _sfxAudioSource.volume = _sfxVolume;
            _musicAudioSource.volume = _musicVolume;
        }

        private void OnEnable()
        {
            _audioSystem.PlayAudioClick += PlayAudioClickEventHandler;
            _audioSystem.PlayAudioMusic += PlayAudioMusicEventHandler;
        }

        private void OnDisable()
        {
            _audioSystem.PlayAudioClick -= PlayAudioClickEventHandler;
            _audioSystem.PlayAudioMusic -= PlayAudioMusicEventHandler;
        }

        private void PlayAudioClickEventHandler()
        {
            ChangeAudioSourceLocation();
            int audioMusicIndex = Random.Range(0, _buttonsClickAudio.Length);
            _sfxAudioSource.PlayOneShot(_buttonsClickAudio[audioMusicIndex]);
        }

        private void PlayAudioMusicEventHandler()
        {
            _musicAudioSource.clip = _musicAudio;
            _musicAudioSource.Play();
            _musicAudioSource.loop = _isMusicLooped;
        }

        private void ChangeAudioSourceLocation()
        {
            _sfxAudioSource.transform.position = _playerLocation.CameraRigTransform.position;
        }
    }
}