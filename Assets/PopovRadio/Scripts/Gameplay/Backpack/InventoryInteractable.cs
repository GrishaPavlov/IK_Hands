using DG.Tweening;
using PopovRadio.Scripts.Tools;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.Backpack
{
    public class InventoryInteractable : XRBaseInteractable
    {
        [SerializeField] private InteractableStackSet inventoryItems;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            var objectToGrab = inventoryItems.Remove(null);
            if (!objectToGrab) return;

            objectToGrab.gameObject.SetActive(true);
            objectToGrab.transform.SetParent(null);

            var objectRigidbody = objectToGrab.transform.GetComponent<Rigidbody>();
            objectRigidbody.useGravity = true;
            objectRigidbody.isKinematic = false;

            interactionManager.ForceSelect(args.interactor, objectToGrab);
            objectToGrab.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        }
    }
}