using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GarbageObject : MonoBehaviour
{
    [SerializeField] private GarbageObjectSO garbageObjectSO;

    //private IkitchenObjectParent kitchenObjectParent;


    public GarbageObjectSO GetKitchenObjectSO()
    {
        return garbageObjectSO;
    }

    /* public static GarbageObject SpawKitchenObject(GarbageObjectSO kitchenObjectSO, IkitchenObjectParent kitchenObjectParent)
    {
        Transform garbageOjectTransform = Instantiate(kitchenObjectSO.prefab);
        GarbageObject garbageObject = garbageOjectTransform.GetComponent<GarbageObject>();
        garbageObject.SetGarbageParents(kitchenObjectParent);

        return garbageObject;
    } */

    /* public void SetGarbageParents(BaseCounter baseCounter)
    {
        if(baseCounter.HasGarbageOnTheBottom())
        {
            
            //위치저장
            Vector3 position1 = baseCounter.GetGarbage().transform.position;
            Vector3 position2 = transform.position;
            //부모 관계 저장
            Transform parent1 = baseCounter.GetGarbage().transform.parent;
            Transform parent2 = transform.parent;
            //위치변경
            baseCounter.GetGarbage().transform.position = position2;
            transform.position = position1;
            //부모 관계 변경
            baseCounter.GetGarbage().transform.parent = parent2;
            transform.parent = parent1;
            
            Destroy(baseCounter.GetGarbage().gameObject);

            baseCounter.ConvertAndSetBottomGarbage(this);
        }
        else
        {
            baseCounter.ConvertAndSetBottomGarbage(this);
            transform.parent = baseCounter.GetBottomPoint();
            transform.localPosition = Vector3.zero;
        }
        
    } */
}