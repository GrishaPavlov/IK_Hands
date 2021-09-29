using System.Collections.Generic;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;
using XNode;

namespace PopovRadio.Scripts.Gameplay.EventSystem.xNodeEditor.Nodes
{
    public class EventNode : Node
    {
        #region Fields

        [Tooltip("Предметы, которые нужно ставить потом")] [Input(ShowBackingValue.Never)]
        public EventTrigger input;

        [Tooltip("Предметы, которые нужно ставить после")] [Output]
        public List<EventTrigger> output;

        [Tooltip("Индекс события")] public int index;
        
        [Tooltip("Событие начала")] public AppEvent StartEvent;

        #endregion

        #region Public Methods

        // Функция, возвращающая индекс данного узла дерева
        public override object GetValue(NodePort port)
        {
            return index;
        }

        #endregion
    }
}