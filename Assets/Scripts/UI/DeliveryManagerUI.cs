using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Start() {
        
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSoList())
        {
            Instantiate(recipeTemplate, container);
        }
    }
}
