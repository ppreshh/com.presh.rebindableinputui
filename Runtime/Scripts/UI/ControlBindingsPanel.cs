using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RebindableInputUI
{
    public class ControlBindingsPanel : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private bool m_ShowCompositeBindings = false;

        [Header("UI Elements")]
        [SerializeField] private ControlBindingButton m_ControlBindingButtonPrefab;
        [SerializeField] private Transform m_ControlBindingButtonsParentTransform;
        [SerializeField] private Button m_ResetDefaultsButton;

        private List<ControlBindingButton> m_ControlBindingButtons = new();

        private ControlBindingButton m_CurrentControlBindingButtonToRebind;

        private void Start()
        {
            RebindingManager.Instance.OnBindingsResetToDefault += RebindingManager_OnBindingsResetToDefault;

            m_ResetDefaultsButton.onClick.AddListener(() =>
            {
                RebindingManager.Instance.ResetBindingsToDefault();
            });

            RefreshBindingButtons();
        }

        private void OnDestroy()
        {
            RebindingManager.Instance.OnBindingsResetToDefault -= RebindingManager_OnBindingsResetToDefault;

            m_ResetDefaultsButton.onClick.RemoveAllListeners();

            ClearControlBindingButtons();
        }

        private void RebindingManager_OnBindingsResetToDefault()
        {
            RefreshBindingButtons();
        }

        public void RefreshBindingButtons()
        {
            ClearControlBindingButtons();

            foreach (var action in RebindingManager.Instance.InputActionMap.actions)
            {
                var kbmBindingIndex = action.GetBindingIndex(RebindingManager.KeyboardAndMouseControlSchemeName);
                var gmpdBindingIndex = action.GetBindingIndex(RebindingManager.GamepadControlSchemeName);
                if ((action.bindings[kbmBindingIndex].isPartOfComposite || action.bindings[gmpdBindingIndex].isPartOfComposite) && !m_ShowCompositeBindings) continue;

                var controlBindingButton = Instantiate(m_ControlBindingButtonPrefab, m_ControlBindingButtonsParentTransform);

                controlBindingButton.Init(action);

                controlBindingButton.OnClicked += ControlBindingButton_OnClicked;

                m_ControlBindingButtons.Add(controlBindingButton);
            }
        }

        private void ClearControlBindingButtons()
        {
            foreach (var button in m_ControlBindingButtons)
            {
                button.OnClicked -= ControlBindingButton_OnClicked;
                Destroy(button.gameObject);
            }
            m_ControlBindingButtons.Clear();
        }

        private void ControlBindingButton_OnClicked(object sender, EventArgs e)
        {
            if (RebindingManager.Instance.IsListeningForAnyButton) return;

            m_CurrentControlBindingButtonToRebind = sender as ControlBindingButton;

            RebindingManager.Instance.OnAnyButtonPressed += Controls_OnAnyButtonPressed;
            RebindingManager.Instance.StartListeningForAnyButton();
        }

        private void Controls_OnAnyButtonPressed(object sender, RebindingManager.AnyButtonPressedEventArgs e)
        {
            if (!(e.InputDevice is Keyboard || e.InputDevice is Mouse || e.InputDevice is Gamepad)) return;

            string controlScheme = default;
            if (e.InputDevice is Keyboard || e.InputDevice is Mouse) controlScheme = RebindingManager.KeyboardAndMouseControlSchemeName;
            else if (e.InputDevice is Gamepad) controlScheme = RebindingManager.GamepadControlSchemeName;

            RebindingManager.Instance.OnAnyButtonPressed -= Controls_OnAnyButtonPressed;

            RebindingManager.Instance.StopListeningForAnyButton();
            RebindingManager.Instance.ReplaceBinding(controlScheme, m_CurrentControlBindingButtonToRebind.InputAction, e.ControlPath);

            m_CurrentControlBindingButtonToRebind.Refresh();
        }
    }
}