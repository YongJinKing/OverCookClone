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

    protected virtual void Start() 
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
    public void SetBottomGarbage(KitchenObject kitchenObject)
    {
        this.garbage = kitchenObject;
        kitchenObject.transform.parent = GetBottomPoint();
        kitchenObject.transform.localPosition = Vector3.zero;
        var garbage = Instantiate(GetConvertGarbageSOWithOutput(GetConvertGarbageSOWithInput(kitchenObject)).prefab);
        SwitchObject(GetGarbage().transform, garbage.transform);
        this.garbage = garbage.GetComponent<GarbageObject>();
    }   
    public Transform GetBottomPoint() // 바닥 포인트
    {
        return counterBottomPoint;
    }
    public bool HasGarbage()
    {
        return garbage != null;
    }
    private ConvertKitchenObjectToGarbageObjectSO GetConvertGarbageSOWithInput(KitchenObject kitchenObject)
    {
        
        foreach(var convertKitchenObjectToGarbageObjectSO in convertKitchenObjectToGarbageObjectSOArray)
        {
            
            if(convertKitchenObjectToGarbageObjectSO.input == kitchenObject.GetKitchenObjectSO())
            {
                return convertKitchenObjectToGarbageObjectSO;
            }
            
        }
        return null;
    }
    public void ClearGarbage()
    {

        garbage = null;
    }
    private KitchenObjectSO GetConvertGarbageSOWithOutput(ConvertKitchenObjectToGarbageObjectSO convertKitchenObjectToGarbageObjectSO)
    {
        return convertKitchenObjectToGarbageObjectSO.output[0];
    }
    public void SwitchObject(Transform pos1, Transform pos2)//pos1은 삭제되는 오브젝트, pos2는 갱신되는 오브젝트
    {

        //위치저장
        Vector3 position1 = pos1.position;
        Vector3 position2 = pos2.position;
        //부모 관계 저장
        Transform parent1 = pos1.parent;
        Transform parent2 = pos1.parent;
        //위치변경
        pos1.position = position2;
        pos2.position = position1;
            //부모 관계 변경
        pos1.parent = parent2;
        pos2.parent = parent1;
            
        Destroy(pos1.gameObject);    
        
    }
}
