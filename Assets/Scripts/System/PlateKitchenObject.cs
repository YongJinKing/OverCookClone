using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> onIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField]private List<KitchenObjectSO> validKitchenObjectSOList;


    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() 
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)//재료확인
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))//유효재료 리스트에 해당 재료가 포함되어 있지 않을경우
        {
            //유효한 재료가 아님
            return false;
        }
        if(kitchenObjectSOList.Contains(kitchenObjectSO))// 이미 있는 재료인 경ㅇ우
        {
            return false;
        }
        else//접시 위에는 없는 재료인경우
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            onIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
                
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    
}
