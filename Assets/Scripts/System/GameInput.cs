using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractThrowAction;
    public event EventHandler OnInteractCleanAction;

    private PlayerInputAction playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractArlternate_performed;
        playerInputActions.Player.InteractThrow.performed += InteractThrow_Performed;
        playerInputActions.Player.InteractClean.performed += InteractClaen_Performed;

    }

    private void InteractClaen_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractCleanAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractThrow_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractThrowAction?.Invoke(this, EventArgs.Empty);
    }
    private void InteractArlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //InputAction에서 설정한 상호작용 키 실행
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty); //제공자 이벤트 실행
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //InputAction에서 설정한 상호작용 키 실행
        OnInteractAction?.Invoke(this, EventArgs.Empty); //제공자 이벤트 실행
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
