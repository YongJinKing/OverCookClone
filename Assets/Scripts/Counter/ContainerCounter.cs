using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter
{
    [SerializeField]private KitchenObjectSO kitchenObjectSO;
    
    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            //플레이어가 아무것도 안가지고 있을때 실행
            KitchenObject.SpawKitchenObject(kitchenObjectSO,player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
            
        
    }
    
}
