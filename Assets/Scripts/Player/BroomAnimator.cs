using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAnimator : MonoBehaviour
{
    private const string IS_CLEANING = "isCleaning";
    private GarbageObject garbageObject;
    private Animator animator;

    private Transform broomVisual;
    private void Start() 
    {
        broomVisual = transform.GetChild(0);
        animator = broomVisual.GetComponent<Animator>();
        Player.Instance.OnSelectedGarbage += Player_OnSelectedGarbage;
        Hide();
        
    }
    private void Player_OnSelectedGarbage(object sender, Player.OnSelectedGarbageChangedEventArgs e)
    {
        garbageObject = e.selectedGarbage;
    }
    private void Update() 
    {
        if(garbageObject != null)
        {
            if(garbageObject.IsCleaning())
            {
                Show();
                
            }
            else
            {
                Hide();
            }
            animator.SetBool(IS_CLEANING,garbageObject.IsCleaning());
        }
        else
        {
            Hide();
        }
        
        
    }
    private void Hide()
    {
        broomVisual.gameObject.SetActive(false);
    }
    private void Show()
    {
        broomVisual.gameObject.SetActive(true);
    }
}
