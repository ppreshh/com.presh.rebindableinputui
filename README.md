## Rebindable Input UI

This is a plug-n-play UI implementation for rebindable KBM and Controller/Gamepad controls using Unity's InputSystem. The implementation most closely resembles Rocket League's controls/bindings UI.

![image](https://github.com/ppreshh/Rebindable-Input/assets/17578313/faea4549-8e6d-4473-95dd-74150e3e64ac)

**Current Drawbacks:**

- Composite controls are not rebindable; they will still be displayed in the UI with different formatting
- There isn't any duplicate binding check

**Setup:**

1. Create your InputActionAsset with the controls you would like for your game
2. Create two control schemes in the InputActionAsset "KeyboardMouse" and "Gamepad", and mark the bindings in the InputActionAsset's InputActionMap accordingly
3. Open the 'SampleUIScene' in the package
4. In the hierarchy, click on InputManager and add the InputActionAsset you've created to it's InputActionAsset reference slot in the inspector. Also add the name of the InputActionMap that you want to be rebindable.
5. Start the scene to see the UI in action
