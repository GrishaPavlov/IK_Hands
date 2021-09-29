using System.Collections.Generic;
using System.Linq;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    public class GripTutorial : MonoBehaviour
    {
        #region Fields

        [Tooltip("Экшен схвата левым контроллером")] [SerializeField]
        private InputActionProperty leftGripAction;

        [Tooltip("Экшен схвата правым контроллером")] [SerializeField]
        private InputActionProperty rightGripAction;

        [Tooltip("Событие, срабатываемое при схвате обеих контроллеров")] [SerializeField]
        private AppEvent bothHandsGripEvent;

        private readonly Dictionary<InputAction, bool> _gripStatus = new Dictionary<InputAction, bool>();

        #endregion

        #region Properties

        private bool BothHandsGrip => _gripStatus.Count == 2 && _gripStatus.Values.All(status => status);

        #endregion

        #region LifeCycle

        private void OnEnable()
        {
            leftGripAction.action.performed += HandleGripActionPerformed;
            rightGripAction.action.performed += HandleGripActionPerformed;

            leftGripAction.action.canceled += HandleGripActionCanceled;
            rightGripAction.action.canceled += HandleGripActionCanceled;
        }

        private void OnDisable()
        {
            leftGripAction.action.performed -= HandleGripActionPerformed;
            rightGripAction.action.performed -= HandleGripActionPerformed;

            leftGripAction.action.canceled -= HandleGripActionCanceled;
            rightGripAction.action.canceled -= HandleGripActionCanceled;
        }

        #endregion

        #region Private Methods

        private void HandleGripActionPerformed(InputAction.CallbackContext context)
        {
            _gripStatus[context.action] = true;

            CheckBothHandsGrip();
        }

        private void HandleGripActionCanceled(InputAction.CallbackContext context)
        {
            _gripStatus[context.action] = false;
        }

        private void CheckBothHandsGrip()
        {
            if (!BothHandsGrip) return;
            bothHandsGripEvent?.Invoke();

            Destroy(this);
        }

        #endregion
    }
}