using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    

    [SerializeField]private KitchenObjectSO kitchenObjectSO;
    

    public override void Interact(Player player)
    {
       if(!HasKitchenObject())//ClearCounter에 오브젝트가 없을때
       {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //플레이어가 아무것도 안가지고 있을때
            }
       }
       else
       {
            if(player.HasKitchenObject())
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    
                }
                else
                {
                    //플레이어가 접시를 들고있지 않을때
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //카운터에 접시가 있음
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
       }
        
        
    }
    
}
