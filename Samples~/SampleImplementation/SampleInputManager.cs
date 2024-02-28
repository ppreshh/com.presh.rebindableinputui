using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RebindableInputUI
{
    /// <summary>
    /// This is a sample InputManager demonstrating how to setup your inputs alongside the RebindingManager implementation.
    /// </summary>
    public class SampleInputManager : MonoBehaviour
    {
        public static SampleInputManager Instance { private set; get; }

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;
        private InputAction m_DashAction;
        private InputAction m_PrimaryAttackAction;
        private InputAction m_SecondaryAttackAction;
        private InputAction m_LockOnAction;

        public Vector2 MoveValue { get { return m_MoveAction.ReadValue<Vector2>(); } }

        public event Action OnJumped;
        public event Action OnDashed;
        public event Action OnPrimaryAttackPerformed;
        public event Action OnSecondaryAttackPerformed;
        public event Action OnLockOnPerformed;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            m_MoveAction = RebindingManager.Instance.InputActionMap.FindAction("Move");
            m_JumpAction = RebindingManager.Instance.InputActionMap.FindAction("Jump");
            m_DashAction = RebindingManager.Instance.InputActionMap.FindAction("Dash");
            m_PrimaryAttackAction = RebindingManager.Instance.InputActionMap.FindAction("PrimaryAttack");
            m_SecondaryAttackAction = RebindingManager.Instance.InputActionMap.FindAction("SecondaryAttack");
            m_LockOnAction = RebindingManager.Instance.InputActionMap.FindAction("LockOn");

            m_JumpAction.performed += JumpAction_performed;
            m_DashAction.performed += DashAction_performed;
            m_PrimaryAttackAction.performed += PrimaryAttackAction_performed;
            m_SecondaryAttackAction.performed += SecondaryAttackAction_performed;
            m_LockOnAction.performed += LockOnAction_performed;
        }

        private void OnDestroy()
        {
            m_JumpAction.performed -= JumpAction_performed;
            m_DashAction.performed -= DashAction_performed;
            m_PrimaryAttackAction.performed -= PrimaryAttackAction_performed;
            m_SecondaryAttackAction.performed -= SecondaryAttackAction_performed;
            m_LockOnAction.performed -= LockOnAction_performed;
        }

        private void JumpAction_performed(InputAction.CallbackContext obj)
        {
            OnJumped?.Invoke();
        }

        private void DashAction_performed(InputAction.CallbackContext obj)
        {
            OnDashed?.Invoke();
        }

        private void PrimaryAttackAction_performed(InputAction.CallbackContext obj)
        {
            OnPrimaryAttackPerformed?.Invoke();
        }

        private void SecondaryAttackAction_performed(InputAction.CallbackContext obj)
        {
            OnSecondaryAttackPerformed?.Invoke();
        }

        private void LockOnAction_performed(InputAction.CallbackContext obj)
        {
            OnLockOnPerformed?.Invoke();
        }

        public void EnablePlayerControls()
        {
            RebindingManager.Instance.InputActionMap.Enable();
        }

        public void DisablePlayerControls()
        {
            RebindingManager.Instance.InputActionMap.Disable();
        }
    }
}