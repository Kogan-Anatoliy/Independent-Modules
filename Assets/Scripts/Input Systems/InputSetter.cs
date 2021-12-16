/*
 * author: Anatolii Kogan
 * e-mail: kogan.1anatoli@gmail.com
 */
using UnityEngine;
using UnityEngine.UI;
using DemoProject.Player;

namespace DemoProject.CustomInput
{
    public class InputSetter : MonoBehaviour
    {
        public KeyCode switchInputButton = KeyCode.None;
        public PlayerController playerController = null;

        public Text currentInputText = null;

        private InputController[] _inputControllers = new InputController[0];

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _inputControllers = FindObjectsOfType<InputController>();
            foreach (var inputController in _inputControllers)
            {
                inputController.enabled = false;
            }

            if (_inputControllers.Length > 0)
            {
                playerController.inputController = _inputControllers[0];
                currentInputText.text = $"{playerController.inputController.GetType()} or press: {switchInputButton}";

                playerController.inputController.enabled = true;
            }
            else
            {
                currentInputText.text = "None";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(switchInputButton))
            {
                SwitchInputController();
            }
        }

        public void SwitchInputController()
        {
            playerController.inputController.enabled = false;
            for (int i = 0; i < _inputControllers.Length; i++)
            {
                if (_inputControllers[i] == playerController.inputController)
                {
                    //return next array element:
                    playerController.inputController = (i + 1 < _inputControllers.Length ? _inputControllers[i + 1] : _inputControllers[0]);

                    currentInputText.text = $"{playerController.inputController.GetType()} or press: {switchInputButton}";
                    break;
                }
            }

            playerController.inputController.enabled = true;
        }
    }
}