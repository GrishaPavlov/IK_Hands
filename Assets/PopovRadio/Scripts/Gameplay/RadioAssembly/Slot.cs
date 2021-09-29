using System;
using PopovRadio.Scripts.Gameplay.EventSystem;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.RadioAssembly
{
    [RequireComponent(typeof(XRSocketInteractor))]
    public class Slot : MonoBehaviour
    {
        #region Fields

        private XRSocketInteractorByIndex _socketInteractor;

        #endregion

        #region Properties

        public int Index => _socketInteractor.Index;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _socketInteractor = GetComponent<XRSocketInteractorByIndex>();
        }

        private void OnEnable()
        {
            _socketInteractor.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            _socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
        }

        #endregion

        #region Private Methods

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            GameEvents.current.PutCurrentPart(Index);
            args.interactable.interactionLayerMask = LayerMask.NameToLayer("Static");
        }

        #endregion
    }
}