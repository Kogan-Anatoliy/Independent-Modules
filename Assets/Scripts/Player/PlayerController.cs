/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;
using DemoProject.CustomInput;

namespace DemoProject.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public float maxSpeed = 5.0f;
        public float pushingForce = 1.0f;

        public InputController inputController = null;

        private Rigidbody _rigidbody = null;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(inputController.direction * inputController.value * pushingForce, ForceMode.Acceleration);
            if (_rigidbody.velocity.magnitude > maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
            }
        }
    }
}