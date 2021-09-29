using System;
using TMPro;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    [Serializable]
    public class DescriptionSettings
    {
        [Tooltip("Текст описания")] [TextArea] public string Text;

        [Tooltip("Выравнивание текста по вертикали")]
        public VerticalAlignmentOptions VerticalAlign;

        [Tooltip("Начальное значение таймера для перехода на следующий шаг")]
        public int TimeoutValue;
    }
}