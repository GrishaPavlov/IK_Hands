using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    [CreateAssetMenu]
    public class PlayerInfo : ScriptableObject
    {
        [SerializeField] private HandInfo leftHandInfo;
        [SerializeField] private HandInfo rightHandInfo;

        public GameObject LeftHandObject => leftHandInfo.HoldingObject;
        public GameObject RightHandObject => rightHandInfo.HoldingObject;
        public bool IsHoldingSomething => leftHandInfo.HoldingObject || rightHandInfo.HoldingObject;
    }
}