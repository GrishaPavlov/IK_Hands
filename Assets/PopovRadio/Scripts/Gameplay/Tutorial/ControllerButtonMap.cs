using System;
using PopovRadio.Scripts.Gameplay.Hands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [Serializable]
    public class ControllerButtonMap
    {
        [Tooltip("Экшен кнопки, который должен сработать")]
        public InputActionProperty Action;

        [Tooltip("Объект для подсветки и выделения")]
        public GameObject ButtonObject;

        [Tooltip("Текст тултипа")] public string TooltipText;
    }
}