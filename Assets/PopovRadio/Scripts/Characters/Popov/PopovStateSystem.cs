using System;
using PopovRadio.Scripts.Common;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;
using UnityEngine.Events;

namespace PopovRadio.Scripts.Characters.Popov
{
    public class PopovStateSystem : AStateMachine<PopovState>
    {
        public AudioSource AudioSource;

        public UnityEvent OnSpeechStart;
        public UnityEvent OnSpeechFinish;

        private void Awake()
        {
            if (!AudioSource)
            {
                AudioSource = GetComponent<AudioSource>();
            }
        }

        public void StartSpeakingState()
        {
            StopAllCoroutines();
            SetState(new Speaking(this));
        }

        public void AppendAudioClipToQueue(AudioClip clip)
        {
            StartCoroutine(State.AppendClipToQueue(clip));
        }

        public void PlayQueue(AppEvent completeEvent = null)
        {
            StartCoroutine(State.PlayQueue(completeEvent));
        }
    }
}