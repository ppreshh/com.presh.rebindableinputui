using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace RebindableInputUI
{
    public class RebindingManager : MonoBehaviour
    {
        public static string GamepadControlSchemeName = "Gamepad";
        public static string KeyboardAndMouseControlSchemeName = "KeyboardMouse";

        private static readonly string m_FileName = "control-overrides.ini";

        [SerializeField] private string m_ActionMapName;
        [SerializeField] private InputActionAsset m_InputActionAsset;

        private IDisposable m_AnyButtonEventListener = null;
        public bool IsListeningForAnyButton { get { return m_AnyButtonEventListener != null; } }

        public InputActionMap InputActionMap { get { return m_InputActionAsset.FindActionMap(m_ActionMapName); } }

        public event EventHandler<AnyButtonPressedEventArgs> OnAnyButtonPressed;
        public class AnyButtonPressedEventArgs : EventArgs
        {
            public InputDevice InputDevice;
            public string ControlPath;
        }

        public event Action OnBindingsResetToDefault;

        public static RebindingManager Instance { private set; get; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            Load();

            Enable();
        }

        [ContextMenu("Save Controls")]
        public void Save()
        {
            string filePath = Path.Combine(Application.persistentDataPath, m_FileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(m_InputActionAsset.SaveBindingOverridesAsJson());
            }
        }

        [ContextMenu("Load Controls")]
        public void Load()
        {
            string filePath = Path.Combine(Application.persistentDataPath, m_FileName);

            if (!File.Exists(filePath)) return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                m_InputActionAsset.LoadBindingOverridesFromJson(reader.ReadLine());
            }
        }

        [ContextMenu("Enable Controls")]
        public void Enable()
        {
            if (m_InputActionAsset.enabled)
            {
                Debug.LogWarning("Controls already enabled.");
                return;
            }

            m_InputActionAsset.Enable();
        }

        [ContextMenu("Disable Controls")]
        public void Disable()
        {
            if (!m_InputActionAsset.enabled)
            {
                Debug.LogWarning("Controls already disabled.");
                return;
            }

            m_InputActionAsset.Disable();
        }

        public void ResetBindingsToDefault()
        {
            foreach (var action in InputActionMap.actions)
            {
                action.RemoveAllBindingOverrides();
            }

            Save();

            OnBindingsResetToDefault?.Invoke();
        }

        public void ReplaceBinding(string controlSchemeName, InputAction inputAction, string bindingPath)
        {
            inputAction.Disable();
            inputAction.RemoveBindingOverride(InputBinding.MaskByGroup(controlSchemeName));
            inputAction.ApplyBindingOverride(bindingPath, controlSchemeName);
            inputAction.Enable();

            Save();
        }

        public void StartListeningForAnyButton()
        {
            m_AnyButtonEventListener = InputSystem.onAnyButtonPress.Call(control =>
            {
                //Debug.Log($"Any button pressed: {control.device}, {control.path}");
                OnAnyButtonPressed?.Invoke(this, new AnyButtonPressedEventArgs { InputDevice = control.device, ControlPath = control.path });
            });
        }

        public void StopListeningForAnyButton()
        {
            m_AnyButtonEventListener.Dispose();
            m_AnyButtonEventListener = null;
        }
    }
}