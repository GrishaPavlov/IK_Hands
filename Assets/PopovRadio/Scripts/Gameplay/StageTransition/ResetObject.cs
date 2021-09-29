using System;
using PopovRadio.Scripts.Gameplay.EventSystem;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.StageTransition
{
    public class ResetObject : MonoBehaviour
    {

        #region Public Variables
        
        [Tooltip("Время, через которе предмет будет возвращен на исходную позицию")]
        [SerializeField] float resetDelayTime;
        [Tooltip("Максимальное расстояние, на которое может быть подвинут предмет без его телепортации на исходную позицию")]
        [SerializeField] float resetRadius;
        
        #endregion

        #region Private Variables

        private XRGrabInteractable m_GrabInteractable;
        private Vector3 _returnToPosition;
        private bool ShouldReturnHome { get; set; }
        private bool _itemsCollected;
        private bool _isController;

        #endregion

        #region LifeCycle
        
        void Awake()
        {
            m_GrabInteractable = GetComponent<XRGrabInteractable>();
            _returnToPosition = this.transform.position;
            ShouldReturnHome = false;
        }

        private void OnEnable()
        {
            m_GrabInteractable.selectExited.AddListener(OnSelectExit); 
            m_GrabInteractable.selectEntered.AddListener(OnSelect);
        }

        private void Start()
        {
            GameEvents.current.OnItemsCollected += () => _itemsCollected = true;
        }

        private void OnDisable()
        {
            m_GrabInteractable.selectExited.RemoveListener(OnSelectExit);
            m_GrabInteractable.selectEntered.RemoveListener(OnSelect);
        }

        #endregion

        #region Private Methods
        
        private void OnSelect(SelectEnterEventArgs arg0) => CancelInvoke("ReturnHome");
        private void OnSelectExit(SelectExitEventArgs arg0) => Invoke(nameof(ReturnHome), resetDelayTime);
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Equals("Table") && _itemsCollected)
            {
                if (!ShouldReturnHome)
                {
                    _returnToPosition = transform.position;
                    Debug.Log("Position set " + _returnToPosition);
                }
                ShouldReturnHome = true;
            }
        }

        private void ReturnHome()
        {
            
            if (ShouldReturnHome && Vector3.Distance(_returnToPosition, transform.position) > resetRadius)
                transform.position = _returnToPosition;
        }
        
        private bool ControllerCheck(GameObject collidedObject)
        {
            //first check that this is not the collider of a controller
            _isController = collidedObject.gameObject.GetComponent<XRBaseController>() != null ? true : false;
            return _isController;
        }
        
        #endregion
    }
    
}
