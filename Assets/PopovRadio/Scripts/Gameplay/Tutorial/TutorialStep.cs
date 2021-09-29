using System;
using PopovRadio.Scripts.Tools.AppEvents;
// using UnityEditor;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [Serializable]
    public class TutorialStep
    {
        #region Fields

        [Tooltip("Сцена окружения")] public GameObject EnvironmentScene;

        [Tooltip("Текст заголовка")] [TextArea]
        public string TitleText;

        [Tooltip("Настройки описания")] public DescriptionSettings DescriptionText;

        [Tooltip("Список кнопок в интерфейсе")]
        public EventButton[] Buttons;

        [Tooltip("Список фраз записанных")]
        public AudioClip[] Voices;

        [Tooltip("Событие, вызываемое при запуске шага")]
        public AppEvent StartEvent;

        [Tooltip("Событие, завершающее шаг обучения после срабатывания")]
        public AppEvent DoneEvent;

        #endregion
    }
}