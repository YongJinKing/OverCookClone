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

    public void SetKitchenObjectParent(IkitchenObjectParent kitchenObject)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();

        }
        this.kitchenObjectParent = kitchenObject;

        if(kitchenObject.HasKitchenObject())
        {
            Debug.LogError("버그임");
        }
        kitchenObject.SetKitchenObject(this);

        transform.parent = kitchenObject.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IkitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawKitchenObject(KitchenObjectSO kitchenObjectSO, IkitchenObjectParent kitcehnObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject .SetKitchenObjectParent(kitcehnObjectParent);

        return kitchenObject;
    }
}
