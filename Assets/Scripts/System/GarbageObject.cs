using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GarbageObject : KitchenObject, IHasProgress
{
    private GarbageObject selectGarbage;

    
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    private int cleaningProgress;
    private float cleaningTimer = 0.5f;
    private float cleaningTimerMax= 0.5f;
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
    }
    private void Player_OnSelectedGarbage(object sender, Player.OnSelectedGarbageChangedEventArgs e)
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
                            cleaningTimer = 0;
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = (float)cleaningProgress / GetKitchenObjectSO().cleanCount
                            });
                        }
                        if(cleaningProgress >= GetKitchenObjectSO().cleanCount)
                        {
                            cleaningProgress = 0;
                            DestroyGarbage();
                            state = State.Done;
                           
                        }
                    }
                    else
                    {
                        state = State.Idle;
                        
                    }
                    break;
                case State.Done :
                    break;
            }
        }
    }
    public void InteractClean()
    {
        state = State.Cleaning;
        
    }
    private void DestroyGarbage()
    {
        
        Transform findBaseCounter = transform;
        while(true)
        {
            if(findBaseCounter.GetComponent<BaseCounter>() == null)
            {
                findBaseCounter = findBaseCounter.parent;
            }
            else
            {
                findBaseCounter.GetComponent<BaseCounter>().ClearGarbage();
                break;
            }
        }
        Destroy(gameObject);
    }
    public bool IsCleaning()
    {
        if(state == State.Cleaning)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
   
}