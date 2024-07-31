using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ICanPlaceKitchenObject
{
    public BaseCounter GetBaseCounter();
    public void TryGetRecipe(KitchenObject kitchenObject);
    
}
