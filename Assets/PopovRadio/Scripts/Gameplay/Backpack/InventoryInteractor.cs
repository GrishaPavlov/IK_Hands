using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PopovRadio.Scripts.Tools;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.Backpack
{
    public class InventoryInteractor : XRBaseInteractor
    {
        #region Fields

        public override bool requireSelectExclusive => true;

        [Tooltip("Маска доступных к хранению объектов")] [SerializeField]
        private string[] equipableTags;

        [SerializeField] private InteractableStackSet inventoryItems;

        [Tooltip("Камера для отслеживания положения игрока")] [SerializeField]
        private Camera playerCamera;

        private XRBaseInteractable _currentInteractable;

        #endregion

        #region LifeCycle

        private void FixedUpdate()
        {
            ChangeBackpackPosition();
            ChangeBackpackRotation();
        }

        private void OnTriggerEnter(Collider other)
        {
            SetInteractable(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ClearInteractable(other);
        }

        #endregion

        #region Public Methods

        public override void GetValidTargets(List<XRBaseInteractable> targets)
        {
            targets.Clear();
            targets.Add(_currentInteractable);
        }

        public override bool CanHover(XRBaseInteractable interactable)
        {
            return base.CanHover(interactable) && _currentInteractable == interactable;
        }

        public override bool CanSelect(XRBaseInteractable interactable)
        {
            return base.CanSelect(interactable) && _currentInteractable == interactable;
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            var interactable = args.interactable;
            var interactableRigidbody = interactable.GetComponent<Rigidbody>();
            interactableRigidbody.useGravity = false;
            interactableRigidbody.isKinematic = true;

            interactable.transform.SetParent(attachTransform);
            DOTween.Sequence()
                .Join(interactable.transform.DOLocalMove(Vector3.zero, 0.5f))
                .Join(interactable.transform.DOScale(Vector3.zero, 0.5f))
                .OnComplete(() =>
                {
                    interactable.gameObject.SetActive(false);
                    inventoryItems.Add(interactable);
                });

            _currentInteractable = null;
            selectTarget = null;
        }

        protected override void OnSelectExiting(SelectExitEventArgs args)
        {
        }

        #endregion

        #region Private Methods

        private bool TryGetInteractable(Collider collider, out XRBaseInteractable interactable)
        {
            interactable = interactionManager.GetInteractableForCollider(collider);
            return interactable != null;
        }

        private void SetInteractable(Collider other)
        {
            if (!TryGetInteractable(other, out var interactable)) return;
            if (_currentInteractable == null && interactable.isSelected && equipableTags.Contains(interactable.tag))
            {
                _currentInteractable = interactable;
            }
        }

        private void ClearInteractable(Collider other)
        {
            if (!TryGetInteractable(other, out var interactable)) return;
            if (_currentInteractable == interactable)
            {
                _currentInteractable = null;
            }
        }

        private void ChangeBackpackPosition()
        {
            transform.localPosition = playerCamera.transform.localPosition;
        }

        private void ChangeBackpackRotation()
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, playerCamera.transform.eulerAngles.y, 0));
        }

        #endregion
    }
}