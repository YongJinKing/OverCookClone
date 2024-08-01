using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGarbageObjectParent
{
    public KitchenObject GetGarbage();
    
    public void ConvertAndSetBottomGarbage(KitchenObject kitchenObject);
    
    public Transform GetBottomPoint();
    
    public bool HasGarbageOnTheBottom();
    

    
}
