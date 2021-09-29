using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    public abstract class RuntimeListSet<TItemType> : RuntimeSet<List<TItemType>, TItemType>
    {
        private void OnEnable()
        {
            Items = new List<TItemType>();
        }

        public override void Add(TItemType item)
        {
            if (Items.Contains(item)) return;
            Items.Add(item);
            OnItemAdded.Invoke(item);
        }

        public override TItemType Remove(TItemType item)
        {
            if (!Items.Contains(item)) return item;
            Items.Remove(item);
            OnItemRemoved.Invoke(item);
            return item;
        }
    }
}