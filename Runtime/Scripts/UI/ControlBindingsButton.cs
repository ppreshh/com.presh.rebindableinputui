using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RebindableInputUI
{
    public class ControlBindingButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        [SerializeField] private TextMeshProUGUI m_ActionText;
        [SerializeField] private TextMeshProUGUI m_KeyboardMouseBindingText;
        [SerializeField] private TextMeshProUGUI m_GamepadBindingText;
        [SerializeField] private Image m_GamepadIconImage;
        [SerializeField] private TextMeshProUGUI m_PromptText;
        [SerializeField] private GamepadIconLibrary m_GamepadIconLibrary;

        public event EventHandler OnClicked;

        private InputAction m_InputAction;
        public InputAction InputAction { get { return m_InputAction; } }

        private void Start()
        {
            m_Button.onClick.AddListener(() =>
            {
                ShowPrompt();
                OnClicked?.Invoke(this, EventArgs.Empty);
            });
        }

        private void OnDestroy()
        {
            m_Button.onClick.RemoveAllListeners();
        }

        public void Init(InputAction inputAction)
        {
            m_InputAction = inputAction;

            m_ActionText.text = m_InputAction.name;

            var kbmBindingIndex = m_InputAction.GetBindingIndex(InputManager.KeyboardAndMouseControlSchemeName);
            var gamepadBindingIndex = m_InputAction.GetBindingIndex(InputManager.GamepadControlSchemeName);

            if (m_InputAction.bindings[kbmBindingIndex].isPartOfComposite)
            {
                string kbmDisplayString = default;
                foreach (var binding in m_InputAction.bindings)
                {
                    if (binding.groups.Equals(InputManager.KeyboardAndMouseControlSchemeName)) kbmDisplayString += binding.ToDisplayString();
                }

                m_KeyboardMouseBindingText.text = kbmDisplayString;
            }
            else
            {
                m_KeyboardMouseBindingText.text = m_InputAction.GetBindingDisplayString(kbmBindingIndex);
            }

            m_GamepadBindingText.text = m_InputAction.GetBindingDisplayString(gamepadBindingIndex, out string deviceLayoutName, out string controlPath);
            if (m_GamepadIconLibrary.TryGetSprite(deviceLayoutName, controlPath, out Sprite sprite))
            {
                m_GamepadBindingText.gameObject.SetActive(false);
                m_GamepadIconImage.sprite = sprite;
                m_GamepadIconImage.gameObject.SetActive(true);
            }

            if (m_InputAction.bindings[kbmBindingIndex].isPartOfComposite || m_InputAction.bindings[gamepadBindingIndex].isPartOfComposite)
            {
                m_Button.interactable = false;
                m_Button.image.enabled = false;
                m_ActionText.color = Color.white;
                m_GamepadBindingText.color = Color.white;
                m_KeyboardMouseBindingText.color = Color.white;
            }
        }

        public void Refresh()
        {
            HidePrompt();

            Init(m_InputAction);
        }

        private void ShowPrompt()
        {
            m_KeyboardMouseBindingText.gameObject.SetActive(false);
            m_GamepadBindingText.gameObject.SetActive(false);
            m_GamepadIconImage.gameObject.SetActive(false);
            m_PromptText.gameObject.SetActive(true);
        }

        private void HidePrompt()
        {
            m_PromptText.gameObject.SetActive(false);
            m_KeyboardMouseBindingText.gameObject.SetActive(true);
            m_GamepadBindingText.gameObject.SetActive(true);
            m_GamepadIconImage.gameObject.SetActive(false);
        }
    }
}