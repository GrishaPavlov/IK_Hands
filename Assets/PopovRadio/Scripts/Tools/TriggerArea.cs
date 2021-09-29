using System;
using System.Linq;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;
using UnityEngine.Events;

namespace PopovRadio.Scripts.Tools
{
    public class TriggerArea : MonoBehaviour
    {
        [SerializeField] private string[] interactingTags;
        [SerializeField] private bool onlyOnce;
        [SerializeField] private AppEvent triggeredEvent;

        public UnityEvent OnTriggered = new UnityEvent();
        public UnityEvent OnUnTriggered = new UnityEvent();

        private void OnTriggerEnter(Collider other)
        {
            if (!ValidateObject(other)) return;

            OnTriggered.Invoke();
            triggeredEvent?.Invoke();

            if (onlyOnce) Destroy(gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!ValidateObject(other)) return;

            OnUnTriggered.Invoke();
        }

        private bool ValidateObject(Collider other)
        {
            return interactingTags.Length == 0 || interactingTags.Contains(other.tag);
        }
    }
}