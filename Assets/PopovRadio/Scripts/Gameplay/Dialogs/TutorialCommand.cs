﻿using System;
using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Tools;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    [CreateAssetMenu]
    public class TutorialCommand : Command
    {
        [SerializeField] private StringType itemName;

    }
}