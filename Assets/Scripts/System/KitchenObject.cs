using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IkitchenObjectParent kitchenObjectParent;

    

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IkitchenObjectParent KitchenObject)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = KitchenObject;

        if(KitchenObject.HasKitchenObject())
        {
            Debug.LogError("버그임");
        }
        KitchenObject.SetKitchenObject(this);

        transform.parent = KitchenObject.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IkitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
