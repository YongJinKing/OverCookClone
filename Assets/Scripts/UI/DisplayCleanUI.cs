using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayCleanUI : MonoBehaviour
{
    private TMP_Text displayCleanText;
    private void Start() 
    {
        displayCleanText = transform.GetChild(0).GetComponent<TMP_Text>();
        Player.Instance.OnSelectedGarbage += Player_OnSelectedGarbage;
        Hide();
    }
    
    private void Player_OnSelectedGarbage(object sender, Player.OnSelectedGarbageChangedEventArgs e)
    {
        if(e.selectedGarbage != null)
        {
            Show();
            displayCleanText.text = "청소하세요";
            if(e.selectedGarbage.IsCleaning())
            {
                displayCleanText.text = "청소 중...";
            }
        }
        else
        {
            Hide();
        }
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
