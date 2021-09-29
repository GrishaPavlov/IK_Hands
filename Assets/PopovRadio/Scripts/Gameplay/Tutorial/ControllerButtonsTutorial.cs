using PopovRadio.Scripts.Gameplay.Hands;
using PopovRadio.Scripts.Gameplay.Interact;
using PopovRadio.Scripts.Tools.AppEvents;
using PopovRadio.Scripts.UI.Tooltip;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    public class ControllerButtonsTutorial : MonoBehaviour
    {
        #region Fields

        [Tooltip("Событие, вызываемое при завершении нажатия кнопок")] [SerializeField]
        private AppEvent onFinishEvent;

        [Tooltip("Последовательность кнопок, которые необходимо нажать")] [SerializeField]
        private ControllerButtonMap[] buttonsOrder;

        [Tooltip("Сила вибрации при нажатии верных кнопок")] [SerializeField] [Range(0, 1)]
        private float hapticAmplitude = 0.8f;

        [Tooltip("Продолжительность вибрации")] [SerializeField] [Range(0, 1)]
        private float hapticDuration = 0.5f;

        private int _currentButtonIndex;
        private Tooltip _currentTooltip;
        private Outline _currentButtonOutline;

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает обучение
        /// </summary>
        public void StartTutorial()
        {
            SetNextButton();
        }

        #endregion

        #region Private Methods

        private void SubscribeToPerformAction(InputAction action)
        {
            action.performed += HandleButtonPress;
        }

        private void UnsubscribeFromPerformAction(InputAction action)
        {
            action.performed -= HandleButtonPress;
        }

        private void SetNextButton()
        {
            if (_currentButtonIndex >= buttonsOrder.Length)
            {
                onFinishEvent?.Invoke();
                return;
            }

            var nextButton = buttonsOrder[_currentButtonIndex++];
            SubscribeToPerformAction(nextButton.Action.action);

            _currentTooltip = TooltipSystem.Instance.AddTooltip(nextButton.ButtonObject.transform,
                nextButton.TooltipText, new Vector3(0, 0.1f, 0.1f));
            if (nextButton.ButtonObject.TryGetComponent(out _currentButtonOutline))
            {
                _currentButtonOutline.enabled = true;
            }
        }

        private void HandleButtonPress(InputAction.CallbackContext context)
        {
            var handType = HandType.None;
            if (context.action.ToString().Contains("Left")) handType = HandType.Left;
            if (context.action.ToString().Contains("Right")) handType = HandType.Right;

            HapticSystem.Instance.SendImpulse(handType, hapticAmplitude, hapticDuration);

            _currentTooltip.Destroy();
            if (_currentButtonOutline)
            {
                _currentButtonOutline.enabled = false;
            }

            UnsubscribeFromPerformAction(context.action);
            SetNextButton();
        }

        #endregion
    }
}