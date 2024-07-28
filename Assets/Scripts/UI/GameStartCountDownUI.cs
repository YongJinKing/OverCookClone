using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountDownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countDonwText;



    private void Start() 
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }
    private void Update() 
    {
        countDonwText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountDownToStartTimer()).ToString();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if(KitchenGameManager.Instance.IsCountDownToStartActive())
        {
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

