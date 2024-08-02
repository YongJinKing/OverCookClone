using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GarbageObject : KitchenObject
{
    private GarbageObject selectGarbage;

    private int cleaningProgress;
    private float cleaningTimer;
    private float cleaningTimerMax= 0.25f;
    private State state;
    public enum State
    {
        Idle,
        Cleaning,
        Done,
    }
    private void Start() 
    {
        
        Player.Instance.OnSelectedGarbage += Player_OnSelectedGarbage;
        cleaningTimer = 0.25f;
    }
    private void Player_OnSelectedGarbage(object sender, Player.OnSelecteedGarbageChangedEventArgs e)
    {
        selectGarbage = e.selectedGarbage;
    }
    private void Update() 
    {
        if(selectGarbage != null)
        {
            switch(state)
            {
                case State.Idle :
                    break;
                case State.Cleaning :
                    if(selectGarbage != null)
                    {   
                        cleaningTimer += Time.deltaTime;
                        if(cleaningTimer >= cleaningTimerMax)
                        {
                            cleaningProgress++;
                        }
                        if(cleaningProgress >= GetKitchenObjectSO().cleanCount)
                        {
                            cleaningProgress = 0;
                            state = State.Done;
                        }
                    }
                    break;
                case State.Done :
                    break;
            }
        }
    }
    public void InteractClean()// 자식으로 부터 실행되는 virtual 함수
    {
        state = State.Cleaning;
        cleaningProgress = 0;
        
    }
    private void DestroyGarbage()
    {

    }
   
}