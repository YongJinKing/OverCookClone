using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ClearCounter : BaseCounter, ICanPlaceKitchenObject
{
    public override void Interact(Player player)
    {
       if(!HasKitchenObjectOnTheTop())//ClearCounter에 오브젝트가 없을때
       {
            if(player.HasKitchenObjectOnTheTop())//플레이어가 물건을 가지고 있으면
            {
                player.GetKitchenObjectOnTheTop().SetKitchenObjectParentOnTheTop(this);//플레이어가 가지고 있는 오브젝트를 해당 카운터에 상속시킴
            }
            else
            {
                //플레이어가 아무것도 안가지고 있을때
            }
       }
       else//ClearCounter에 오브젝트가 있을 때
       {
            if(player.HasKitchenObjectOnTheTop())//플레이어가 물건을 가지고 있으면
            {
                if(player.GetKitchenObjectOnTheTop().TryGetPlate(out PlateKitchenObject plateKitchenObject))//플레이어가 가지고 있는 물건이 접시인지 확인
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObjectOnTheTop().GetKitchenObjectSO()))//접시위에 재료 및 카운터 위에 있는 재료와 비교
                    {
                        GetKitchenObjectOnTheTop().DestroySelf();
                    }
                }
                else
                {
                    //플레이어가 접시를 들고있지 않을때
                    if(GetKitchenObjectOnTheTop().TryGetPlate(out plateKitchenObject))
                    {
                        //카운터에 접시가 있음
                        if(plateKitchenObject.TryAddIngredient(player.GetKitchenObjectOnTheTop().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObjectOnTheTop().DestroySelf();
                        }
                    }
                }

            }
            else
            {
                GetKitchenObjectOnTheTop().SetKitchenObjectParentOnTheTop(player);
            }
       }
    }
    public BaseCounter GetBaseCounter()
    {
        
        return this;
    }
    public void TryGetRecipe(KitchenObject kitchenObject)
    {
        
        kitchenObject.SetKitchenObjectParentOnTheTop(this);
        
    }

    
}
