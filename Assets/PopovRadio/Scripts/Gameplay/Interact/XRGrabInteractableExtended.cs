using System;
using PopovRadio.Scripts.Gameplay.Hands;
using PopovRadio.Scripts.Tools;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.Interact
{
    [RequireComponent(typeof(Outline))]
    public class XRGrabInteractableExtended : XRGrabInteractable
    {
        #region Settings

        [Tooltip("При схатывании объекта он будет взят со смещением")] [SerializeField]
        private bool offsetGrab;

        [Tooltip("Оставить текущего родителя у объекта после взятия")] [SerializeField]
        private bool remainParent;

        [Tooltip("Привязать модель руки к объекту при схватывании")] [SerializeField]
        private bool attachHand;

        [Tooltip("Привязать объект к руке при схватывании")] [SerializeField]
        private bool attachToHand;

        [Tooltip("Скрывать руку при взятии объекта")] [SerializeField]
        private bool hideHand;

        #endregion

        #region Fields

        private Vector3 _interactorPosition;
        private Quaternion _interactorRotation;

        private GameHand _handPresence;
        private GameObject _handModel;

        private Vector3 _originalHandPosition;
        private Quaternion _originalHandRotation;

        private Outline _outline;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _outline = GetComponent<Outline>();
        }
        
        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            _outline.enabled = true;
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            _outline.enabled = false;
        }

        protected override void OnSelectEntering(SelectEnterEventArgs eventArgs)
        {
            Transform originalObjectParent = null;
            if (remainParent)
            {
                originalObjectParent = transform.parent;
            }

            base.OnSelectEntering(eventArgs);

            if (remainParent)
            {
                transform.SetParent(originalObjectParent);
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs eventArgs)
        {
            base.OnSelectEntered(eventArgs);

            if (attachToHand)
            {
                MoveToHand(eventArgs.interactor);
            }

            if (attachHand)
            {
                StoreHand(eventArgs.interactor);
                MoveHandToObject();
            }

            if (hideHand)
            {
                StoreHand(eventArgs.interactor);
                _handModel.SetActive(false);
            }

            if (offsetGrab)
            {
                StoreInteractorInfo(eventArgs.interactor);
                MatchAttachmentPoints(eventArgs.interactor);
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs eventArgs)
        {
            if (offsetGrab)
            {
                ResetAttachmentPoint(eventArgs.interactor);
                ClearInteractorInfo();
            }

            if (attachHand)
            {
                ResetHandParent();
            }

            if (hideHand)
            {
                _handModel.SetActive(true);
            }

            base.OnSelectExited(eventArgs);
        }
       
        private void StoreInteractorInfo(XRBaseInteractor interactor)
        {
            _interactorPosition = interactor.attachTransform.localPosition;
            _interactorRotation = interactor.attachTransform.localRotation;
        }

        private void StoreHand(XRBaseInteractor interactor)
        {
            _handPresence = interactor.transform.GetComponentInChildren<GameHand>();
            _handModel = _handPresence.transform.GetChild(0).gameObject;
        }

        private void MatchAttachmentPoints(XRBaseInteractor interactor)
        {
            var hasAttach = attachTransform != null;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }

        private void MoveHandToObject()
        {
            var handModelTransform = _handModel.transform;

            _originalHandPosition = handModelTransform.localPosition;
            _originalHandRotation = handModelTransform.localRotation;

            handModelTransform.SetParent(transform);

            // Если задана поза для объекта, ставим позицию и координаты позы
            if (TryGetComponent(out PoseContainer poseContainer))
            {
                var handInfo = poseContainer.Pose.GetHandInfo(_handPresence.HandType);
                handModelTransform.localPosition = handInfo.attachPosition;
                handModelTransform.localRotation = handInfo.attachRotation;
            }
        }

        private void MoveToHand(XRBaseInteractor interactor)
        {
            transform.SetParent(interactor.transform);
        }

        private void ResetAttachmentPoint(XRBaseInteractor interactor)
        {
            interactor.attachTransform.localPosition = _interactorPosition;
            interactor.attachTransform.localRotation = _interactorRotation;
        }

        private void ResetHandParent()
        {
            var handModelTransform = _handModel.transform;

            handModelTransform.SetParent(_handPresence.transform);
            handModelTransform.SetAsFirstSibling();
            handModelTransform.localPosition = _originalHandPosition;
            handModelTransform.localRotation = _originalHandRotation;
        }

        private void ClearInteractorInfo()
        {
            _interactorPosition = Vector3.zero;
            _interactorRotation = Quaternion.identity;
        }

        private void OnValidate()
        {
            _outline = GetComponent<Outline>();
            _outline.OutlineColor = Color.blue;
            _outline.OutlineWidth = 8f;
            _outline.enabled = false;
        }
    }
}