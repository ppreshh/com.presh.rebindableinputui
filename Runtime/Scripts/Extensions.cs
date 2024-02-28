using UnityEngine.InputSystem;

namespace RebindableInputUI
{
    public static class Extensions
    {
        public static bool IsComposite(this InputAction inputAction)
        {
            var kbmBindingIndex = inputAction.GetBindingIndex(RebindingManager.KeyboardAndMouseControlSchemeName);
            var gamepadBindingIndex = inputAction.GetBindingIndex(RebindingManager.GamepadControlSchemeName);
            if (kbmBindingIndex >= 0)
                if (inputAction.bindings[kbmBindingIndex].isPartOfComposite) return true;
            if (gamepadBindingIndex >= 0)
                if (inputAction.bindings[gamepadBindingIndex].isPartOfComposite) return true;

            return false;
        }
    }
}