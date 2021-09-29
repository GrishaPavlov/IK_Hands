using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PopovRadio.Scripts.Gameplay.Teleport
{
    public class TeleportController : MonoBehaviour
    {
        [SerializeField] private GameObject controller;
        [SerializeField] private InputActionProperty enableAction;

        private void Awake()
        {
            controller.SetActive(false);
        }

        private void OnEnable()
        {
            enableAction.action.performed += OnEnableActionPerformed;
            enableAction.action.canceled += OnEnableActionCanceled;
        }

        private void OnDisable()
        {
            enableAction.action.performed -= OnEnableActionPerformed;
            enableAction.action.canceled -= OnEnableActionCanceled;
        }

        private void OnEnableActionPerformed(InputAction.CallbackContext context)
        {
            EnableController();
        }

        private void OnEnableActionCanceled(InputAction.CallbackContext context)
        {
            StartCoroutine(DisableController());
        }

        private void EnableController()
        {
            controller.SetActive(true);
        }

        private IEnumerator DisableController()
        {
            yield return new WaitForEndOfFrame();
            controller.SetActive(false);
        }
    }
}