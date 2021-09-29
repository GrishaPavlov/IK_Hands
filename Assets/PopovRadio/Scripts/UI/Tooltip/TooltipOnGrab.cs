using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.UI.Tooltip
{
    public class TooltipOnGrab : MonoBehaviour
    {
        [SerializeField] private XRGrabInteractable[] grabbableObjects;
        [SerializeField] private bool showOnce;
        [SerializeField] private float duration = 5f;
        [SerializeField] private string description;
        [SerializeField] private string actionTip;

        private void OnEnable()
        {
            foreach (var grabbableObject in grabbableObjects)
            {
                grabbableObject.selectEntered.AddListener(OnSelectEntered);
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs eventArgs)
        {
            var objectTransform = eventArgs.interactable.transform;
            TooltipSystem.Instance.AddTooltip(objectTransform, description, actionTip, duration);
            if (showOnce)
            {
                Destroy(this);
            }
        }
    }
}