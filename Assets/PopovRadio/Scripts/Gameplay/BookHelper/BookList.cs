using System.Collections;
using DG.Tweening;
using PopovRadio.Scripts.Gameplay.Interact;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.BookHelper
{
    public class BookList : MonoBehaviour
    {
        #region Fields
    
        [SerializeField] [Tooltip("Время открытия книги")]
        private float TimeToOpen;
        [SerializeField] [Tooltip("Время возвращения книги на исходную позицию")]
        private float TimeToClose;
        [SerializeField] [Tooltip("Кнопка активации книги (первая кнопка на левом контроллере)")]
        private InputActionProperty activateButton;
        [SerializeField] [Tooltip("Камера для отслеживания положения игрока")]
        private Camera playerCamera;
        [SerializeField] [Tooltip("Ссылка на Animator книги")] 
        private Animator _animator;
        [SerializeField] [Tooltip("Ссылка на Grab Interactable книги")]
        private XRGrabInteractableExtended _xrGrabInteractable;
        [SerializeField] [Tooltip("DirentInteractor левого контроллера")]
        private XRDirectInteractor leftHand;
        [SerializeField] [Tooltip("Ссылка на InteractionManager")]
        private XRInteractionManager interactionManager;
        [SerializeField] [Tooltip("Объект книги, позицию которой мы будем изменять")]
        private GameObject bookModel;
        private Vector3 _startingPosition;
        private Vector3 _startingRotation;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            // bookModel = transform.GetChild(0).gameObject;
            _startingPosition = bookModel.transform.localPosition;
            _startingRotation = bookModel.transform.localRotation.eulerAngles;
            _xrGrabInteractable.selectEntered.AddListener(Open);
            _xrGrabInteractable.selectExited.AddListener(Close);
            activateButton.action.started += _ => OpenOnButtonPress();
            activateButton.action.canceled += _ => StartCoroutine(CloseOnButtonRelease());
        }

        private void Update()
        {
            ChangePosition();
            ChangeRotation();
        }

        #endregion

        #region Private Methods

        private void ChangePosition()
        {
            transform.localPosition = playerCamera.transform.localPosition;
        }

        private void ChangeRotation()
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, playerCamera.transform.eulerAngles.y, 0));
        }

        private void OpenOnButtonPress()
        {
            DOTween.Sequence()
                .Join(bookModel.transform.DOLocalMove(leftHand.transform.position, TimeToOpen))
                .Join(bookModel.transform.DOLocalRotate(leftHand.transform.rotation.eulerAngles, TimeToOpen));
            interactionManager.ForceSelect(leftHand, GetComponentInChildren<XRGrabInteractableExtended>());
        }

        private IEnumerator CloseOnButtonRelease()
        {
            leftHand.allowSelect = false;
            // _animator.SetTrigger("BookClose");
            _animator.SetBool("BookState", false);
            bookModel.transform.SetParent(transform);
            DOTween.Sequence()
                .SetDelay(0.05f)
                .Join(bookModel.transform.DOLocalMove(_startingPosition, TimeToClose))
                .Join(bookModel.transform.DOLocalRotate(_startingRotation, TimeToClose));
            yield return new WaitForEndOfFrame();
            leftHand.allowSelect = true;
        }

        private void Open(SelectEnterEventArgs args)
        {
            // _animator.SetTrigger("BookOpen");
            _animator.SetBool("BookState", true);

        }
        

        private void Close(SelectExitEventArgs args)
        {
            // _animator.SetTrigger("BookClose");
            _animator.SetBool("BookState", false);

            bookModel.transform.SetParent(transform);
            DOTween.Sequence()
                .Join(bookModel.transform.DOLocalMove(_startingPosition, TimeToClose))
                .Join(bookModel.transform.DOLocalRotate(_startingRotation, TimeToClose));
        }

        #endregion
    }
}
