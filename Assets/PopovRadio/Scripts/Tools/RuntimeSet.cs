using System;
using UnityEngine;
using UnityEngine.Events;

namespace PopovRadio.Scripts.Tools
{
    public abstract class RuntimeSet<TContainerType, TItemType> : ScriptableObject
    {
        public TContainerType Items;

        public UnityEvent<TItemType> OnItemAdded;
        public UnityEvent<TItemType> OnItemRemoved;

        public abstract void Add(TItemType item);

        public abstract TItemType Remove(TItemType item);
    }
}