using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    public abstract class RuntimeStackSet<TItemType> : RuntimeSet<Stack<TItemType>, TItemType>
    {
        private void OnEnable()
        {
            Items = new Stack<TItemType>(10);
        }

        public override void Add(TItemType item)
        {
            if (Items.Contains(item)) return;
            Items.Push(item);
            OnItemAdded.Invoke(item);
        }

        public override TItemType Remove(TItemType item)
        {
            if (Items.Count == 0) return item;
            var poppedItem = Items.Pop();
            OnItemRemoved.Invoke(poppedItem);
            return poppedItem;
        }
    }
}