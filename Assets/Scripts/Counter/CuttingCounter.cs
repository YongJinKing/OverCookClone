using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress, ICanPlaceKitchenObject
{
    public enum State
    {
        Idle,
        Cutting,
        Done,
    }
    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    private State state;
    private float cuttingTimer;
    

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    

    private int cuttingProgress;

    private void Start() 
    {
        state = State.Idle;
        cuttingTimer = 0.25f;
    }
    private void Update() 
    {
        if(HasKitchenObjectOnTheTop())
        {
            switch(state)
            {
                case State.Idle :
                    break;
                case State.Cutting :
                    if(Player.Instance.GetSelectedCounter() == this)
                    {
                        cuttingTimer += Time.deltaTime;
                        if(cuttingTimer >= 0.25f)
                        {
                            cuttingTimer = 0;
                            cuttingProgress++;

                            OnCut?.Invoke(this, EventArgs.Empty);
                            OnAnyCut?.Invoke(this, EventArgs.Empty);
                            
                            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjectOnTheTop().GetKitchenObjectSO());

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                            });

                            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                            {
                                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObjectOnTheTop().GetKitchenObjectSO());

                                GetKitchenObjectOnTheTop().DestroySelf();

                                KitchenObject.SpawKitchenObject(outputKitchenObjectSO,this);

                                state = State.Done;
                            }
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
    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObjectOnTheTop() && HasRecipeWithInput(GetKitchenObjectOnTheTop().GetKitchenObjectSO()))
        {
            //재료 손질 실행
            state = State.Cutting;
        }
    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObjectOnTheTop())//ClearCounter에 오브젝트가 없을때
       {
            if(player.HasKitchenObjectOnTheTop())
            {
                if(HasRecipeWithInput(player.GetKitchenObjectOnTheTop().GetKitchenObjectSO()))
                {
                    player.GetKitchenObjectOnTheTop().SetKitchenObjectParentOnTheTop(this);
                    cuttingProgress = 0;
                    state = State.Idle;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjectOnTheTop().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
                
            }
            else
            {
                //플레이어가 아무것도 안가지고 있을때
            }
       }
       else
       {
            if(player.HasKitchenObjectOnTheTop())
            {
                if(player.GetKitchenObjectOnTheTop().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObjectOnTheTop().GetKitchenObjectSO()))
                    {
                        GetKitchenObjectOnTheTop().DestroySelf();
                    }
                    
                }
            }
            else
            {
                GetKitchenObjectOnTheTop().SetKitchenObjectParentOnTheTop(player);
            }
       }
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
        
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if(cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
    public BaseCounter GetBaseCounter()
    {
        return this;
    }
    public void TryGetRecipe(KitchenObject kitchenObject)
    {
        if(HasRecipeWithInput(kitchenObject.GetKitchenObjectSO()))
        {
            kitchenObject.SetKitchenObjectParentOnTheTop(this);
            cuttingProgress = 0;
            state = State.Idle;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObjectOnTheTop().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
        }  
    }
}
