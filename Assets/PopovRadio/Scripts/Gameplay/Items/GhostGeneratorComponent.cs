using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.Items
{
    public class GhostGeneratorComponent : MonoBehaviour
    {
        #region Settings

        [Tooltip("Расположение призрака-подсказки")] [SerializeField]
        private Transform ghostTransform;

        [Tooltip("Материал призрака-подсказки")] [SerializeField]
        private Material ghostMaterial;

        [Tooltip("Можно ли отобразить призрак-подсказку")] [SerializeField]
        private bool isGhostDisplayable;

        #endregion

        #region Fields

        private GameObject _ghost;
        private GameObject _currentObject;

        private XRGrabInteractable _grabInteractable;

        #endregion

        #region LifeCycle

        private void Start()
        {
            _currentObject = transform.GetChild(0).gameObject;

            _grabInteractable = gameObject.GetComponent<XRGrabInteractable>();
        }

        private void Update()
        {
            if (IsGhostEmpty() && IsItemGrabbed())
            {
                GenerateGhost();
            }
            else if (!IsGhostEmpty() && !IsItemGrabbed())
            {
                DestroyGhost();
            }
        }

        #endregion

        #region Methods

        private void GenerateGhost()
        {
            if (isGhostDisplayable)
            {
                CreateGhostGameObject();
            }
        }

        private void DestroyGhost()
        {
            if (!IsGhostEmpty())
            {
                Destroy(_ghost);
            }
        }

        public void SetGhostDisplayAbility(bool isAble)
        {
            isGhostDisplayable = isAble;
        }

        private bool IsGhostEmpty() => _ghost == null;

        private bool IsItemGrabbed() => _grabInteractable.isSelected;

        private void CreateGhostGameObject()
        {
            _ghost = Instantiate(_currentObject, ghostTransform.position, ghostTransform.rotation);
            _ghost.GetComponent<MeshRenderer>().materials[0] = ghostMaterial;
            _ghost.GetComponent<BoxCollider>().isTrigger = true;
        }

        #endregion
    }
}