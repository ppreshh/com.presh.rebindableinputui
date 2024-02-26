using UnityEngine;
using UnityEngine.InputSystem;

namespace RebindableInputUI
{
    [CreateAssetMenu(fileName = "GamepadIconLibrary", menuName = "ScriptableObjects/GamepadIconLibrary")]
    public class GamepadIconLibrary : ScriptableObject
    {
        [SerializeField] private GamepadIcons m_XboxIcons;
        [SerializeField] private GamepadIcons m_Ds4Icons;

        public bool TryGetSprite(string deviceLayoutName, string controlPath, out Sprite sprite)
        {
            sprite = default;

            if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
                return false;

            if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
            {
                sprite = m_Ds4Icons.GetSprite(controlPath);
                return true;
            }
            else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
            {
                sprite = m_XboxIcons.GetSprite(controlPath);
                return true;
            }
            else
                return false;
        }
    }
}