using System;
using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    /// <summary>
    /// Информация о команде для выполнения определенного действия 
    /// </summary>
    [CreateAssetMenu]
    public class Command : ScriptableObject
    {
        [SerializeField] private string text;
        [SerializeField] private AppEvent appEvent;

        public virtual string Text => text;
        public AppEvent AppEvent => appEvent;
    }
}