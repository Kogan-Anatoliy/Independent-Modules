/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;

namespace DemoProject.CustomInput
{
    public class BowInputController : InputController
    {
        public float pixelsEqualOneValue = 200.0f;
        public float stabilizationTime = 2.0f;
        public Image prompt = null;

        private Vector3 _startPosition = Vector3.zero;

        private void Awake()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();

            SetActiveVizual(false);

            prompt.rectTransform.sizeDelta = new Vector2(pixelsEqualOneValue, prompt.rectTransform.sizeDelta.y);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetActiveVizual(true);
                ResetValues();
                _startPosition = CurrentMousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                UpdateVisual();
            }

            if (Input.GetMouseButtonUp(0))
            {
                CalculateValues();
                SetActiveVizual(false);
            }

            if (value > 0)
            {
                value -= Time.deltaTime / stabilizationTime;
                if (value < 0)
                {
                    value = 0.0f;
                }
            }
        }

        private void CalculateValues()
        {
            value = Vector2.Distance(CurrentMousePosition, _startPosition) / pixelsEqualOneValue;
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            direction.Set((CurrentMousePosition - _startPosition).x, 0.0f, (CurrentMousePosition - _startPosition).y);
            direction = -direction.normalized;
        }

        private void UpdateVisual()
        {
            prompt.rectTransform.right = CurrentMousePosition - _startPosition;
            prompt.rectTransform.anchoredPosition = _startPosition + prompt.rectTransform.right * (prompt.rectTransform.sizeDelta.x / 2);
            prompt.fillAmount = Vector2.Distance(CurrentMousePosition, _startPosition) / pixelsEqualOneValue;
        }

        protected override void SetActiveVizual(bool active)
        {
            prompt.gameObject.SetActive(active);
        }
    }
}