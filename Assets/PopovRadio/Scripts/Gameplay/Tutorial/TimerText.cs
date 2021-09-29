using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimerText : MonoBehaviour
    {
        #region Constant Fields

        private const string FormatString = "{{t}}";

        #endregion

        #region Fields

        [Tooltip("Значение таймера в секундах")] [SerializeField]
        private int timeoutValue;

        private TextMeshProUGUI _textContainer;
        private string _sourceText;
        private int _timerValue;

        #endregion

        #region Properties

        /// <summary>
        /// Текущее значение таймера
        /// </summary>
        public int TimerValue
        {
            get => _timerValue;
            set
            {
                _timerValue = value;
                UpdateText();
            }
        }

        /// <summary>
        /// Исходный текст
        /// </summary>
        public string SourceText
        {
            get => _sourceText;
            set
            {
                _sourceText = value;
                UpdateText();
            }
        }

        #endregion

        #region Events

        public UnityEvent<TimerText> OnTimerEnd = new UnityEvent<TimerText>();

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _textContainer = GetComponent<TextMeshProUGUI>();
            SourceText = _textContainer.text;

            TimerValue = timeoutValue;
        }

        private void OnDisable()
        {
            StopTimer();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает таймер
        /// </summary>
        public void StartTimer()
        {
            StartCoroutine(TimerCount());
        }

        /// <summary>
        /// Останавливает таймер
        /// </summary>
        public void StopTimer()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Private Methods

        private IEnumerator TimerCount()
        {
            while (TimerValue > 0)
            {
                yield return new WaitForSeconds(1);
                TimerValue -= 1;
            }

            OnTimerEnd.Invoke(this);
        }

        private string GetParsedText(int timerValue)
        {
            return SourceText.Replace(FormatString, timerValue.ToString());
        }

        private void UpdateText()
        {
            _textContainer.text = GetParsedText(_timerValue);
        }

        #endregion
    }
}