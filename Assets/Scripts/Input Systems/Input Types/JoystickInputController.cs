/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;

namespace DemoProject.CustomInput
{
    public class JoystickInputController : InputController
    {
        public RectTransform borders = null;
        public RectTransform joystickButton = null;

        private Vector3 _startPosition = Vector3.zero;             

        private float _deltaEqualOneValue = 0.0f;

        private void Awake()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            SetActiveVizual(false);

            _deltaEqualOneValue = borders.sizeDelta.x / 2;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetValues();
                SetActiveVizual(true);

                _startPosition = CurrentMousePosition;
                borders.anchoredPosition = _startPosition;
            }

            if (Input.GetMouseButton(0))
            {
                CalculateValues();
                UpdateVisual();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SetActiveVizual(false);
                ResetValues();
            }
        }

        private void CalculateValues()
        {
            value = Vector2.Distance(CurrentMousePosition, _startPosition) / _deltaEqualOneValue;
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            direction.Set((CurrentMousePosition - _startPosition).x, 0.0f, (CurrentMousePosition - _startPosition).y);
            direction = direction.normalized;
        }

        private void UpdateVisual()
        {
            joystickButton.anchoredPosition = _startPosition + (CurrentMousePosition - _startPosition).normalized * (_deltaEqualOneValue * value);
        }

        protected override void SetActiveVizual(bool active)
        {
            borders.gameObject.SetActive(active);
            joystickButton.gameObject.SetActive(active);
        }
    }
}