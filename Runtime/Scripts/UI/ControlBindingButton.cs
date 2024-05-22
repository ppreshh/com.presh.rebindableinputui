using System;
using System.Text.RegularExpressions;
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

            m_ActionText.text = Regex.Replace(m_InputAction.name, @"(\B[A-Z])", " $1");

            var kbmBindingIndex = m_InputAction.GetBindingIndex(RebindingManager.KeyboardAndMouseControlSchemeName);
            var gamepadBindingIndex = m_InputAction.GetBindingIndex(RebindingManager.GamepadControlSchemeName);

            if (kbmBindingIndex == -1)
            {
                m_KeyboardMouseBindingText.gameObject.SetActive(false);
            }
            else
            {
                if (m_InputAction.bindings[kbmBindingIndex].isPartOfComposite)
                {
                    string kbmDisplayString = default;
                    foreach (var binding in m_InputAction.bindings)
                    {
                        if (binding.groups.Equals(RebindingManager.KeyboardAndMouseControlSchemeName)) kbmDisplayString += (binding.ToDisplayString() + ", ");
                    }

                    m_KeyboardMouseBindingText.text = kbmDisplayString.Substring(0, kbmDisplayString.Length - 2);
                }
                else
                {
                    m_KeyboardMouseBindingText.text = m_InputAction.GetBindingDisplayString(kbmBindingIndex);
                }
            }

            if (gamepadBindingIndex == -1)
            {
                m_GamepadBindingText.gameObject.SetActive(false);
            }
            else
            {
                m_GamepadBindingText.text = m_InputAction.GetBindingDisplayString(gamepadBindingIndex, out string deviceLayoutName, out string controlPath);
                if (m_GamepadIconLibrary.TryGetSprite(deviceLayoutName, controlPath, out Sprite sprite))
                {
                    m_GamepadBindingText.gameObject.SetActive(false);
                    m_GamepadIconImage.sprite = sprite;
                    m_GamepadIconImage.gameObject.SetActive(true);
                }
            }

            if (m_InputAction.IsComposite())
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