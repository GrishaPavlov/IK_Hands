using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [RequireComponent(typeof(AudioSource))]
    public class Tutorial : MonoBehaviour
    {
        #region Fields

        [Tooltip("Компонент управления интерфейсом туториала")] [SerializeField]
        private InformationUI informationUI;

        [Tooltip("Список шагов обучения")] [SerializeField]
        private TutorialStep[] steps;

        [Tooltip("Событие при завершении обучения")] [SerializeField]
        private AppEvent doneEvent;

        private int _currentStepIndex;
        private TutorialStep _currentStep;
        private AudioSource _audioSource;
        private Coroutine _voicesCoroutine;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            try
            {
                SetNextStep();
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogWarning("Не задан ни один шаг обучения");
            }
        }

        private void OnEnable()
        {
            informationUI.OnTimerEnd.AddListener(HandleDoneStep);
        }

        private void OnDisable()
        {
            informationUI.OnTimerEnd.RemoveListener(HandleDoneStep);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает следующий шаг обучения
        /// </summary>
        public void SetNextStep()
        {
            _currentStep = steps[_currentStepIndex++];

            HandleStep(_currentStep);
        }

        #endregion

        #region Private Methods

        private void HandleDoneStep()
        {
            if (_currentStep.EnvironmentScene)
            {
                SceneManager.UnloadSceneAsync(_currentStep.EnvironmentScene.name);
            }

            if (_currentStep.Voices.Any())
            {
                StopCoroutine(_voicesCoroutine);
            }

            try
            {
                SetNextStep();
            }
            catch (IndexOutOfRangeException)
            {
                doneEvent.Invoke();
                Debug.LogWarning("Шаги обучения закончились");
            }
        }

        private void HandleStep(TutorialStep step)
        {
            if (step.EnvironmentScene)
            {
                SceneManager.LoadSceneAsync(step.EnvironmentScene.name, LoadSceneMode.Additive);
            }

            informationUI.SetTitle(step.TitleText);
            informationUI.SetDescription(step.DescriptionText);
            informationUI.SetButtons(step.Buttons);

            if (_currentStep.DoneEvent)
            {
                var newListener = new AppEventScriptListener();
                newListener.Event.AddListener(HandleDoneStep);
                _currentStep.DoneEvent.Register(newListener);
            }

            if (_currentStep.StartEvent)
            {
                _currentStep.StartEvent.Invoke();
            }

            if (_currentStep.Voices.Any())
            {
                _voicesCoroutine = StartCoroutine(VoicePlaying(_currentStep.Voices));
            }
        }

        private IEnumerator VoicePlaying(IReadOnlyList<AudioClip> clips)
        {
            var i = 0;
            while (i < clips.Count)
            {
                var currentClip = clips[i];

                _audioSource.PlayOneShot(currentClip);

                yield return new WaitWhile(() => _audioSource.isPlaying);
                yield return new WaitForSeconds(0.5f);
                i++;
            }
        }

        #endregion
    }
}