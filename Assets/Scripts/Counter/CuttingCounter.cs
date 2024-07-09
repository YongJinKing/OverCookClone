using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
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

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
       }
    }
    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject())
        {
            //재료 손질 실행
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawKitchenObject(cutKitchenObjectSO,this);
            
        }
    }

}
