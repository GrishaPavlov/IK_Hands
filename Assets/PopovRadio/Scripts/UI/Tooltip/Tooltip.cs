using System;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopovRadio.Scripts.UI.Tooltip
{
    [ExecuteInEditMode]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform lineStart;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI actionTip;
        [SerializeField] private int characterWrapLimit;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float animationDuration = 2f;

        private TextMeshProUGUI[] _fields;

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

        public string DescriptionText
        {
            get => description.text;
            set => description.text = value;
        }

        public string ActionTipText
        {
            get => actionTip.text;
            set => actionTip.text = value;
        }

        private Transform _playerCamera;

        private void Awake()
        {
            _playerCamera = Camera.main?.transform;
            _fields = new[] {description, actionTip};
        }

        private void OnEnable()
        {
            PlaySpawnAnimation();
        }

        private void Update()
        {
            if (!Target) return;

            UpdatePosition();
            UpdateRotation();
            UpdateLine();

            var exceedMaxLength = _fields.Any(ugui => ugui?.text.Length > characterWrapLimit);
            layoutElement.enabled = exceedMaxLength;
        }

        public void Destroy()
        {
            PlayDestroyAnimation(() => Destroy(gameObject));
        }

        public void Disable()
        {
            PlayDestroyAnimation(() => gameObject.SetActive(false));
        }

        private void UpdatePosition()
        {
            if (!Target) return;
            transform.position = Target.position + Offset;
        }

        private void UpdateRotation()
        {
            transform.LookAt(_playerCamera);
        }

        private void UpdateLine()
        {
            lineRenderer.SetPositions(new[] {lineStart.position, Target.position});
        }

        private void PlaySpawnAnimation()
        {
            transform.localScale = Vector3.zero;
            transform
                .DOScale(new Vector3(1, 1, 1), animationDuration)
                .SetEase(Ease.OutBounce);
        }

        private void PlayDestroyAnimation(Action onComplete)
        {
            transform
                .DOScale(Vector3.zero, animationDuration - 0.5f)
                .OnComplete(() => onComplete());
        }
    }
}