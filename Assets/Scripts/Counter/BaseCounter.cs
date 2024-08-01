using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IkitchenObjectParent, IGarbageObjectParent//Counter들이 가지고 상속받고 있는 부모 클래스
{
    public static event EventHandler OnAnyObjectPlacedHere; //Counter위에 물건 올리면 실행되는 이벤트
    [SerializeField]private Transform counterTopPoint;

    [SerializeField]private Transform counterBottomPoint;

    private ConvertKitchenObjectToGarbageObjectSO[] convertKitchenObjectToGarbageObjectSOArray;

    protected KitchenObject kitchenObject;
    protected KitchenObject garbage;

    private void Start() 
    {
        convertKitchenObjectToGarbageObjectSOArray = Resources.LoadAll<ConvertKitchenObjectToGarbageObjectSO>("ScriptableObjects/ConvertKitchenObjectToGarbageObjectSO");
    }

    
    public virtual void Interact(Player player)// 자식으로 부터 실행되는 virtual 함수
    {
        
    }
    public virtual void InteractAlternate(Player player)//자식으로 부터 실행되는 virtual 함수
    {

    }
    public Transform GetKitchenObjectFollowTransform()//Counter의 탑포인트 값 반환
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)//counter에 kitchenObjcet생성
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()//Counter위에 있는 치킨오브젝트를 받아옴
    {
        return kitchenObject;
    }

    public void ClearKitchenObject() //Counter위에 있는 오브젝트를 삭제함
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject() // Counter위에 오브젝트가 있는지 확인함
    {
        return kitchenObject != null;
    }



    public KitchenObject GetGarbage()
    {
        return garbage;
    }
    public void ConvertAndSetBottomGarbage(KitchenObject kitchenObject)
    {
        this.garbage = kitchenObject;
    }
    public Transform GetBottomPoint() // 바닥 포인트
    {
        return counterBottomPoint;
    }
    public bool HasGarbageOnTheBottom()
    {
        return garbage != null;
    }
    
}
