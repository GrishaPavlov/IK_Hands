using System;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [Serializable]
    public class EventButton
    {
        [Tooltip("Текст кнопки")] public string Text;

        [Tooltip("Событие, вызываемое после нажатия кнопки")]
        public AppEvent ClickAppEvent;
    }
}