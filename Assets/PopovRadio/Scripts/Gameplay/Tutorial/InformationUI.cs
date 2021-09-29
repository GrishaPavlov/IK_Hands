using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    public class InformationUI : MonoBehaviour
    {
        #region Fields

        [Tooltip("Контейнер для заголовка")] [SerializeField]
        private TextMeshProUGUI title;

        [Tooltip("Контейнер для описания")] [SerializeField]
        private TextMeshProUGUI description;

        [Tooltip("Контейнер для кнопок")] [SerializeField]
        private Transform buttonsContainer;

        [Tooltip("Префаб кнопки")] [SerializeField]
        private GameObject buttonPrefab;

        #endregion

        #region Events

        public UnityEvent OnTimerEnd = new UnityEvent();

        #endregion

        #region Public Methods

        /// <summary>
        /// Задает текст заголовка
        /// </summary>
        /// <param name="text"></param>
        public void SetTitle(string text)
        {
            title.text = text;
        }

        /// <summary>
        /// Задает настройки описания
        /// </summary>
        /// <param name="descriptionSettings"></param>
        public void SetDescription(DescriptionSettings descriptionSettings)
        {
            description.text = descriptionSettings.Text;
            description.verticalAlignment = descriptionSettings.VerticalAlign;
            if (descriptionSettings.TimeoutValue > 0)
            {
                if (!description.TryGetComponent<TimerText>(out var timerText))
                {
                    timerText = description.gameObject.AddComponent<TimerText>();
                }

                timerText.SourceText = descriptionSettings.Text;
                timerText.TimerValue = descriptionSettings.TimeoutValue;
                timerText.OnTimerEnd.AddListener(HandleTimerEnd);
                timerText.StartTimer();
            }
        }

        /// <summary>
        /// Задает кнопки 
        /// </summary>
        /// <param name="eventButtons"></param>
        public void SetButtons(IEnumerable<EventButton> eventButtons)
        {
            ClearButtons();

            foreach (var eventButton in eventButtons)
            {
                var spawnedButton = Instantiate(buttonPrefab, buttonsContainer).GetComponent<Button>();
                var spawnedButtonText = spawnedButton.GetComponentInChildren<TextMeshProUGUI>();
                spawnedButtonText.text = eventButton.Text;
                spawnedButton.onClick.AddListener(() => eventButton.ClickAppEvent.Invoke());
            }
        }

        #endregion

        #region Private Methods

        private void ClearButtons()
        {
            foreach (Transform button in buttonsContainer)
            {
                Destroy(button.gameObject);
            }
        }

        private void HandleTimerEnd(TimerText timerText)
        {
            OnTimerEnd.Invoke();
            timerText.OnTimerEnd.RemoveListener(HandleTimerEnd);
        }

        #endregion
    }
}