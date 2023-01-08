using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TGF.Events
{
    public abstract class GameEvent<T1, T2> : GameEvent
    {
        public new event Action<T1, T2> Event;

        public T1 testValue1;
        public T2 testValue2;

        public void Raise(T1 value, T2 value2)
        {
            Event?.Invoke(value, value2);
        }

        [Button(ButtonSizes.Gigantic)]
        protected override void TestRaise()
        {
            Raise(testValue1, testValue2);
        }
    }
    
    public abstract class GameEvent<T> : GameEvent
    {
        public new event Action<T> Event;

        public T testValue;

        public void Raise(T value)
        {
            Event?.Invoke(value);
        }

        [Button(ButtonSizes.Gigantic)]
        protected override void TestRaise()
        {
            Raise(testValue);
        }
    }
    
    [CreateAssetMenu(fileName = "Game Event", menuName = "GameEvents/Event", order = 0)]
    public class GameEvent : ScriptableObject
    {
        public event Action Event;
        
        public void Raise()
        {
            Event?.Invoke();
        }

        [Button(ButtonSizes.Gigantic)]
        protected virtual void TestRaise()
        {
            Raise();
        }

        [Button][PropertyOrder(5)]
        public void Clear()
        {
            Event = null;
        }
    }
}