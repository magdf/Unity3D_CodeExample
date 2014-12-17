using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Global class located in the Title scene.
/// </summary>
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private AudioClip[] _clipsForGameLevels;

    [SerializeField]
    private AudioClip _titleClip;

    [SerializeField]
    private float _fadeDuration = 1f;

    [SerializeField]
    private float _startVolume = 0.05f;

    private AudioSource _titleAudioSource;
    private AudioSource _battleAudioSource;
    private AudioSource _nextAudio;

    private void Start()
    {
        var audios = GetComponents<AudioSource>();
        _titleAudioSource = audios[0];
        _battleAudioSource = audios[1];

        _titleAudioSource.clip = _titleClip;

        if (IsNotBattle)
            _titleAudioSource.Play();
        else
            _battleAudioSource.Play();
    }

    private void Update()
    {
        if ((IsNotBattle && _titleAudioSource.isPlaying) || (!IsNotBattle && _battleAudioSource.isPlaying) || _nextAudio!=null)
            return;

        if (!_titleAudioSource.isPlaying && !_battleAudioSource.isPlaying)
        {
            _nextAudio = IsNotBattle ? _titleAudioSource : _battleAudioSource;
            Invoke("StartAudio", _fadeDuration);
            return;
        }

        if (IsNotBattle)
        {
            _nextAudio = _titleAudioSource;
            Invoke("StartAudio", _fadeDuration);
            StartCoroutine(FadeOutCoroutine(_battleAudioSource));
        }
        else
        {
            _battleAudioSource.clip = RandomUtils.GetRandomItem(_clipsForGameLevels);
            _nextAudio = _battleAudioSource;
            Invoke("StartAudio", _fadeDuration);
            StartCoroutine(FadeOutCoroutine(_titleAudioSource));
        }
    }

    private void StartAudio()
    {
        if (!IsNotBattle)
            _battleAudioSource.clip = RandomUtils.GetRandomItem(_clipsForGameLevels);
        _nextAudio.Play();
        _nextAudio = null;
    }


    private IEnumerator FadeOutCoroutine(AudioSource target)
    {
        while (target.volume > 0)
        {
            target.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }

        target.Stop();
        target.volume = _startVolume;
    }

    private bool IsNotBattle
    {
        get { return !Conditions.Application.IsBattleScene; }
    }

}

