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

    

    private BaseCounter selectedCounter;

    protected IkitchenObjectParent kitchenObjectParent;
    
    private Vector3 playerDir;
    private void Start() 
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        counterLayerMask = 1 << LayerMask.NameToLayer("Counters");
        throwingKitchenObject = false;
    } 
    private void Update() 
    {
        if(throwingKitchenObject)
        {
            float moveSpeed = 10f;
            float moveDistance = moveSpeed * Time.deltaTime;
            float kitchenObjectRadius = 0.5f;
            float kitchenObjectHeight = 2f;
            transform.position += playerDir * moveSpeed * Time.deltaTime;
            if (Physics.CapsuleCast(transform.position - new Vector3(0,1.5f,0),transform.position + Vector3.up * kitchenObjectHeight, kitchenObjectRadius, playerDir, out RaycastHit raycastHit, moveDistance, counterLayerMask))
            {
                //Debug.Log("부딪힘");
                if(raycastHit.transform.TryGetComponent(out ICanPlaceKitchenObject counter))//카운터 확인
                {
                    if(!counter.GetBaseCounter().HasKitchenObject())// 카운터 위에 물건이 없으면
                    {
                        counter.TryGetRecipe(this);
                        //해당 카운터랑 상호작용이 가능한 재료일 때
                    }
                    else //카운터 위에 물건이 있으면
                    {
                        if(counter.GetBaseCounter().GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))//counter가 가지고 있는 물건이 접시인지 확인
                        {
                            if(plateKitchenObject.TryAddIngredient(GetKitchenObjectSO()))//접시위에 재료 및 던진 재료와 비교
                            {
                                DestroyKitchenObject();
                            }
                            else//재료가 일치하지 않으면
                            {
                                ConvertObject(counter.GetBaseCounter());
                            }
                            
                        }// 접시가 아니면
                        else
                        {
                            ConvertObject(counter.GetBaseCounter());
                        }
                    } 
                }
                else//카운터 위에 물건을 올릴수가 없을 때
                {
                    if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
                    {
                        ConvertObject(baseCounter);
                    }
                }
                throwingKitchenObject = false;
            }
            
        }
    }
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void ConvertObject(BaseCounter baseCounter)
    {
        if(baseCounter.HasGarbage())
        {
            baseCounter.SwitchObject(baseCounter.GetGarbage().transform, transform);
            baseCounter.SetBottomGarbage(this);
        }
        else
        {
            baseCounter.SetBottomGarbage(this);
        }
    }

    public void SetKitchenObjectParent(IkitchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();

        }
        this.kitchenObjectParent = kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("버그임");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    
    public IkitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroyKitchenObject()
    {
        kitchenObjectParent.ClearKitchenObject();
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
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }
    public void ThrowKitchenObject(Vector3 playerDir)
    {
        if(selectedCounter)
        {
            selectedCounter.Interact(Player.Instance);
        }
        else
        {
            transform.SetParent(null);
            Player.Instance.ClearKitchenObject();
            this.playerDir = new Vector3(playerDir.x, 0, playerDir.z);
            throwingKitchenObject = true;
        }
    }
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelecteedCounterChangedEventArgs e)
    {
        selectedCounter = e.selectedCounter;
    }

    
}
