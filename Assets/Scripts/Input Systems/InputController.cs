/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;

namespace DemoProject.CustomInput
{
    public abstract class InputController : MonoBehaviour
    {
        [Range(0.0f, 1.0f)] public float value = 0.0f;
        public Vector3 direction = Vector3.zero;

        private Vector3 _vectorCorrector = Vector3.zero;

        private void OnDisable()
        {
            SetActiveVizual(false);
        }

        public void ResetValues()
        {
            value = 0.0f;
            direction = Vector3.zero;
        }

        protected virtual void Initialize()
        {
            ResetValues();
            _vectorCorrector = new Vector3(Screen.width / 2, Screen.height / 2, 0.0f);
        }

        protected virtual void SetActiveVizual(bool active) { }

        protected Vector3 CurrentMousePosition => Input.mousePosition - _vectorCorrector;
    }
}