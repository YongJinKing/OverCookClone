using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IkitchenObjectParent
{
    public static Player Instance { get; private set; }
    

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelecteedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelecteedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
        //전송할 이벤트 프로퍼티 설정
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private GameInput gameInput;


    
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnInteractThrowAction += GameInput_OnInteractThrow;
    }

    private void GameInput_OnInteractThrow(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlayingActive()) return;
        if(HasKitchenObjectOnTheTop())
        {
            ThrowKitchenObject();
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlayingActive()) return;
        if(selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlayingActive()) return;

        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        
    }
    private void Awake()
    {
        
        if(Instance != null)
        {
            Debug.LogError("플레이어 프로포티 하나 더존재");
            //싱글톤 체크
        }
        Instance = this;
        //싱글톤
        gameInput = GameObject.Find("GameInput").GetComponent<GameInput>();
    }
    private void Update()  
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
           
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
    
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //ClearCounter스크립트를 가지고 있으면 실행
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        
        
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if(!canMove)//앞에 장애물이 있을떄
        {
            //플레이어 기준 앞방향 이동불가시
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            
            if(canMove)
            {
                //X축 빈공간 확인
                moveDir = moveDirX;
            } 
            else
            {
                
                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if(canMove)
                {
                    //Z축 빈공간 확인
                    moveDir = moveDirZ;
                }
            }

        }
        if(canMove)
        {
            transform.position += moveDir * moveDistance;
        }
            

        isWalking = moveDir != Vector3.zero;
        //moveDir이 0이 아닐때 isWalking On
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void ThrowKitchenObject()
    {
        //kitchenObject.ThrowKitchenObject(transform.forward);
        //몸방향으로 날리면 조준이 힘든 느낌이여서 이렇게 해봄
        kitchenObject.ThrowKitchenObject(lastInteractDir);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelecteedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, System.EventArgs.Empty);
        }
    }
    public BaseCounter GetSelectedCounter()
    {
        return selectedCounter;
    }
    public KitchenObject GetKitchenObjectOnTheTop()
    {
        return kitchenObject;
    }

    public void ClearKitchenObjectOnTheTop() 
    {
        kitchenObject = null;
    }

    public bool HasKitchenObjectOnTheTop()
    {
        return kitchenObject != null;
    }
    
}
