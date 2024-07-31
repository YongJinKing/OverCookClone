using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IkitchenObjectParent//Counter들이 가지고 상속받고 있는 부모 클래스
{
    public static event EventHandler OnAnyObjectPlacedHere; //Counter위에 물건 올리면 실행되는 이벤트
    [SerializeField]private Transform counterTopPoint;

    [SerializeField]private Transform counterBottomPoint;

    protected KitchenObject kitchenObjectOnTheTop;
    protected KitchenObject kitchenObjectOnTheBottom;
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
        this.kitchenObjectOnTheTop = kitchenObject;
        if(kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObjectOnTheTop()//Counter위에 있는 치킨오브젝트를 받아옴
    {
        return kitchenObjectOnTheTop;
    }

    public void ClearKitchenObjectOnTheTop() //Counter위에 있는 오브젝트를 삭제함
    {
        kitchenObjectOnTheTop = null;
    }

    public bool HasKitchenObjectOnTheTop() // Counter위에 오브젝트가 있는지 확인함
    {
        return kitchenObjectOnTheTop != null;
    }


    public void SetBottomGarbage(KitchenObject kitchenObject)
    {
        this.kitchenObjectOnTheBottom = kitchenObject;
        /* if(kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, System.EventArgs.Empty);
        } */
    }
    public Transform GetBottomPoint() // 바닥 포인트
    {
        return counterBottomPoint;
    }
    
}
