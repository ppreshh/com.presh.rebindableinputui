## Rebindable Input UI

This is a plug-n-play UI implementation for rebindable KBM and Controller/Gamepad controls using Unity's InputSystem. The implementation most closely resembles Rocket League's controls/bindings UI.

Walkthrough Video:
https://youtu.be/xPN4kGig21A

![rebindable input ui yt thumb](https://github.com/ppreshh/com.presh.rebindableinputui/assets/17578313/5108389c-fc03-4baa-b22e-d6fccc46285b)

### Current Drawbacks:

- Composite controls are not rebindable; they will still be displayed in the UI with different formatting
- There isn't any duplicate binding check

### Setup:

**Use the sample in the package as reference**
![image](https://github.com/ppreshh/com.presh.rebindableinputui/assets/17578313/db7168d5-8240-4533-b337-6d809eaf50e7)

1. Create your InputActionAsset with the controls you would like for your game
2. Create two control schemes in the InputActionAsset "KeyboardMouse" and "Gamepad", and mark the bindings in the InputActionAsset's InputActionMap accordingly (there must be a binding for both controller schemes for every action)
3. Create an Empty Gameobject in your scene named 'RebindingManager' and add the component RebindingManager
4. In this component, hook up the InputActionAsset and add the name of the ActionMap that you want to be rebindable
5. Add a UI Canvas to your scene
6. In the package's file directory find the UI prefab at Runtime/Prefas/ControlBindingsPanel.prefab and drag it into the Canvas Gameobject
7. Rescale the Canvas to your liking
8. Start the scene to see your bindings in action in the UI
