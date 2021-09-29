using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    public class RotationTutorial : MonoBehaviour
    {
        #region Fields

        [Tooltip("Экшен поворота на левом контроллере")] [SerializeField]
        private InputActionProperty leftRotateAction;

        [Tooltip("Экшен поворота на правом контроллере")] [SerializeField]
        private InputActionProperty rightRotateAction;

        [Tooltip("Компоненты обводки")] [SerializeField]
        private Outline[] stickOutlines;

        [Tooltip("Событие, вызываемое после поворота")] [SerializeField]
        private AppEvent rotatedEvent;

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            EnableOutline();

            leftRotateAction.action.performed += HandleRotateActionPerformed;
            rightRotateAction.action.performed += HandleRotateActionPerformed;
        }

        private void OnDisable()
        {
            leftRotateAction.action.performed -= HandleRotateActionPerformed;
            rightRotateAction.action.performed -= HandleRotateActionPerformed;

            DisableOutline();
        }

        #endregion

        #region Private Methods

        private void HandleRotateActionPerformed(InputAction.CallbackContext context)
        {
            rotatedEvent?.Invoke();
            Destroy(this);
        }

        private void EnableOutline()
        {
            foreach (var sticksOutline in stickOutlines)
            {
                sticksOutline.enabled = true;
            }
        }

        private void DisableOutline()
        {
            foreach (var sticksOutline in stickOutlines)
            {
                sticksOutline.enabled = false;
            }
        }

        #endregion
    }
}