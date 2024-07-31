using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class KitchenObject : MonoBehaviour
{
   
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private bool throwingKitchenObject;
    private LayerMask counterLayerMask;

    private IkitchenObjectParent kitchenObjectParent;

    private Vector3 playerDir;
    private void Start() 
    {
        throwingKitchenObject = false;
        counterLayerMask = 1 << LayerMask.NameToLayer("Counters");
    }   
    private void Update() 
    {
        if(throwingKitchenObject)
        {
            float moveSpeed = 10f;
            float moveDistance = moveSpeed * Time.deltaTime;
            float kitchenObjectRadius = 0.1f;
            float kitchenObjectHeight = 2f;
            transform.position += playerDir * moveSpeed * Time.deltaTime;
            if (Physics.CapsuleCast(transform.position,transform.position + Vector3.up * kitchenObjectHeight, kitchenObjectRadius, playerDir, out RaycastHit raycastHit, moveDistance, counterLayerMask))
            {
                if(raycastHit.transform.TryGetComponent(out ICanPlaceKitchenObject counter))
                {
                    if(!counter.GetBaseCounter().HasKitchenObjectOnTheTop())// 카운터 위에 물건이 없으면
                    {
                        counter.TryGetRecipe(this);
                        throwingKitchenObject = false;
                    }
                    else //카운터 위에 물건이 있으면
                    {
                        if(counter.GetBaseCounter().GetKitchenObjectOnTheTop().TryGetPlate(out PlateKitchenObject plateKitchenObject))//counter가 가지고 있는 물건이 접시인지 확인
                        {
                            if(plateKitchenObject.TryAddIngredient(GetKitchenObjectSO()))//접시위에 재료 및 던진 재료와 비교
                            {
                                DestroySelf();
                            }
                            
                        }// 접시가 아니면
                        else
                        {
                            SetGarbageParents(counter.GetBaseCounter());
                            throwingKitchenObject = false;
                        }
                    } 
                }
                else
                {
                    if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
                    {
                        SetGarbageParents(baseCounter);
                    }
                }
            }
        }
    }
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParentOnTheTop(IkitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObjectOnTheTop();

        }
        this.kitchenObjectParent = kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObjectOnTheTop())
        {
            Debug.LogError("버그임");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public void SetGarbageParents(BaseCounter baseCounter)
    {
        if(baseCounter != null)
        {
            baseCounter.SetBottomGarbage(null);
        }
        baseCounter.SetBottomGarbage(this);
        transform.parent = baseCounter.GetBottomPoint();
        transform.localPosition = Vector3.zero;
    }
    
    public IkitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObjectOnTheTop();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;

            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
    public static KitchenObject SpawKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParentOnTheTop(kitchenObjectParent);

        return kitchenObject;
    }
    public void ThrowKitchenObject(Vector3 playerDir)
    {
        transform.SetParent(null);
        Player.Instance.ClearKitchenObjectOnTheTop();
        this.playerDir = new Vector3(playerDir.x, 0, playerDir.z);
        throwingKitchenObject = true;
    }

    
}
