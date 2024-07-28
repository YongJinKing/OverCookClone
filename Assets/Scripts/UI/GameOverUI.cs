using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeDeliveredText;

    



    private void Start() 
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }
    private void Update() 
    {
        recipeDeliveredText.text = DeliveryManager.Instance.GetSucessfulRecipesAmount().ToString();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsGameOver())
        {            
            Show();
            recipeDeliveredText.text = DeliveryManager.Instance.GetSucessfulRecipesAmount().ToString();

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
