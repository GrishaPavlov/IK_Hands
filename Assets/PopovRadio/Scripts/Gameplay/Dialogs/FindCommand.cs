using System;
using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Tools;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    [CreateAssetMenu]
    public class FindCommand : Command
    {
        [SerializeField] private StringType itemName;

        public override string Text => $"Я ищу {itemName.Value}";
    }
}