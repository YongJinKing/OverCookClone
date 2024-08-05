using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeDeliveredText;
    [SerializeField] private TMP_Text garbageCountText;

    



    private void Start() 
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }
    /* private void Update() 
    {
        recipeDeliveredText.text = DeliveryManager.Instance.GetSucessfulRecipesAmount().ToString();
    }
 */
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsGameOver())
        {   
            recipeDeliveredText.text = DeliveryManager.Instance.GetSucessfulRecipesAmount().ToString();
            garbageCountText.text = GameObject.Find("Counters").GetComponentsInChildren<GarbageObject>().Length.ToString();
            Show();
            

        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
