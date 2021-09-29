using UnityEngine;

namespace PopovRadio.Scripts.Common
{
    public abstract class AStateMachine<T> : MonoBehaviour where T : IState
    {
        protected T State;

        public void SetState(T newState)
        {
            Debug.Log("changed State to " + newState);
            State = newState;
            StartCoroutine(State.Start());
        }
    }
}