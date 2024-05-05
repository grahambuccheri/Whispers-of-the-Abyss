//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""3ff4bffe-6003-4c1d-b687-d43f9f5abe94"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ba060d59-2273-497c-a42e-27a676012a1f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""a064bd98-bdd0-4698-813f-175a4fd7bc00"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""668018e2-d706-457e-bd3a-dc4c361e0d56"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExitInteract"",
                    ""type"": ""Button"",
                    ""id"": ""4802c5bc-23a9-43ee-bd60-9ab2311343ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Steer"",
                    ""type"": ""Value"",
                    ""id"": ""dd7dfc7f-9369-4c31-968f-522f23e30e13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Lever"",
                    ""type"": ""Value"",
                    ""id"": ""840c819e-bafd-4951-b36f-50d8a96cae09"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Value"",
                    ""id"": ""d5d59aa2-0062-401c-b31d-fcf7ae416dc0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""HeightLever"",
                    ""type"": ""Value"",
                    ""id"": ""241ae7be-04ee-46df-9f58-fec1f5257012"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Hydrophone"",
                    ""type"": ""Value"",
                    ""id"": ""0ace1f8f-32c9-4c9d-aa9b-dfe0e4c1ca14"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""319a80e9-382c-4de1-8161-2613a8a62f9a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5dd3914d-d092-46f9-a6ff-dfba6b6ee944"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1e035cad-fc97-42f4-b632-b7452232d373"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5f3dd6a9-8912-423f-9add-dce33613d1cb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2fbf862a-9635-40a4-ace5-22ef3ad2b79d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""753a9bf1-4f3b-4895-b6e5-67da8f18051d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de9b223b-965d-4118-984c-6d1b33566a00"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06d7f2a0-c702-4d31-ac53-d6eddaf73aea"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitInteract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""AD"",
                    ""id"": ""5303d59e-9332-4261-953c-5ace327cb27f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""abf26529-ad26-4220-abc6-d1b67a405238"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3c94b09b-73fc-4fd3-8890-bbda69db664f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WS"",
                    ""id"": ""d817f330-26f8-47ae-9e89-b94d87459c58"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lever"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""beca4f27-d62d-4453-a712-20396e3a1f87"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""60a8267e-6cd8-4830-93a3-04b03eb7a0d5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fbf1b2a1-d72c-4467-a2c6-e86ff8dd606d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Ctrl/Shift"",
                    ""id"": ""e149d9fd-f8b2-4a83-a503-50c1e79f3f78"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeightLever"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""cc67c25c-678e-496d-99fe-1c3ef4627f0a"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeightLever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d0ee738c-0d46-4c77-873c-9d7d8bc6732c"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeightLever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""QE"",
                    ""id"": ""4b611b90-f50c-42a9-b33a-6c7cdf98e23a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hydrophone"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0943892f-acca-4063-a95b-2b9f8db9ed1d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hydrophone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a4450b06-239f-4b34-a3d7-9c21123874bc"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hydrophone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Move = m_Default.FindAction("Move", throwIfNotFound: true);
        m_Default_Look = m_Default.FindAction("Look", throwIfNotFound: true);
        m_Default_Interact = m_Default.FindAction("Interact", throwIfNotFound: true);
        m_Default_ExitInteract = m_Default.FindAction("ExitInteract", throwIfNotFound: true);
        m_Default_Steer = m_Default.FindAction("Steer", throwIfNotFound: true);
        m_Default_Lever = m_Default.FindAction("Lever", throwIfNotFound: true);
        m_Default_Click = m_Default.FindAction("Click", throwIfNotFound: true);
        m_Default_HeightLever = m_Default.FindAction("HeightLever", throwIfNotFound: true);
        m_Default_Hydrophone = m_Default.FindAction("Hydrophone", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Default
    private readonly InputActionMap m_Default;
    private List<IDefaultActions> m_DefaultActionsCallbackInterfaces = new List<IDefaultActions>();
    private readonly InputAction m_Default_Move;
    private readonly InputAction m_Default_Look;
    private readonly InputAction m_Default_Interact;
    private readonly InputAction m_Default_ExitInteract;
    private readonly InputAction m_Default_Steer;
    private readonly InputAction m_Default_Lever;
    private readonly InputAction m_Default_Click;
    private readonly InputAction m_Default_HeightLever;
    private readonly InputAction m_Default_Hydrophone;
    public struct DefaultActions
    {
        private @PlayerControls m_Wrapper;
        public DefaultActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Default_Move;
        public InputAction @Look => m_Wrapper.m_Default_Look;
        public InputAction @Interact => m_Wrapper.m_Default_Interact;
        public InputAction @ExitInteract => m_Wrapper.m_Default_ExitInteract;
        public InputAction @Steer => m_Wrapper.m_Default_Steer;
        public InputAction @Lever => m_Wrapper.m_Default_Lever;
        public InputAction @Click => m_Wrapper.m_Default_Click;
        public InputAction @HeightLever => m_Wrapper.m_Default_HeightLever;
        public InputAction @Hydrophone => m_Wrapper.m_Default_Hydrophone;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @ExitInteract.started += instance.OnExitInteract;
            @ExitInteract.performed += instance.OnExitInteract;
            @ExitInteract.canceled += instance.OnExitInteract;
            @Steer.started += instance.OnSteer;
            @Steer.performed += instance.OnSteer;
            @Steer.canceled += instance.OnSteer;
            @Lever.started += instance.OnLever;
            @Lever.performed += instance.OnLever;
            @Lever.canceled += instance.OnLever;
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @HeightLever.started += instance.OnHeightLever;
            @HeightLever.performed += instance.OnHeightLever;
            @HeightLever.canceled += instance.OnHeightLever;
            @Hydrophone.started += instance.OnHydrophone;
            @Hydrophone.performed += instance.OnHydrophone;
            @Hydrophone.canceled += instance.OnHydrophone;
        }

        private void UnregisterCallbacks(IDefaultActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @ExitInteract.started -= instance.OnExitInteract;
            @ExitInteract.performed -= instance.OnExitInteract;
            @ExitInteract.canceled -= instance.OnExitInteract;
            @Steer.started -= instance.OnSteer;
            @Steer.performed -= instance.OnSteer;
            @Steer.canceled -= instance.OnSteer;
            @Lever.started -= instance.OnLever;
            @Lever.performed -= instance.OnLever;
            @Lever.canceled -= instance.OnLever;
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @HeightLever.started -= instance.OnHeightLever;
            @HeightLever.performed -= instance.OnHeightLever;
            @HeightLever.canceled -= instance.OnHeightLever;
            @Hydrophone.started -= instance.OnHydrophone;
            @Hydrophone.performed -= instance.OnHydrophone;
            @Hydrophone.canceled -= instance.OnHydrophone;
        }

        public void RemoveCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnExitInteract(InputAction.CallbackContext context);
        void OnSteer(InputAction.CallbackContext context);
        void OnLever(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnHeightLever(InputAction.CallbackContext context);
        void OnHydrophone(InputAction.CallbackContext context);
    }
}
