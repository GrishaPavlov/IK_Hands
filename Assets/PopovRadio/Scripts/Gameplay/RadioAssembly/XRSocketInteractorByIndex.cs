using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.RadioAssembly
{
    public class XRSocketInteractorByIndex : XRSocketInteractor
    {
        #region Fields

        [Tooltip("Индекс данного сокета")] [SerializeField]
        private int index;

        #endregion

        #region Properties

        public int Index => index;

        #endregion

        #region Public Methods

        public override bool CanHover(XRBaseInteractable interactable)
        {
            return base.CanHover(interactable) && MatchingIndex(interactable);
        }

        public override bool CanSelect(XRBaseInteractable interactable)
        {
            return base.CanSelect(interactable) && MatchingIndex(interactable);
        }

        #endregion

        #region Private Methods

        private bool MatchingIndex(XRBaseInteractable interactable)
        {
            if (interactable.TryGetComponent<Item>(out var itemComponent))
            {
                return itemComponent.index == Index;
            }

            return false;
        }

        #endregion
    }
}