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
    private float cuttingTimerMax;
    

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    

    private int cuttingProgress;

    protected override void Start() 
    {
        base.Start();
        state = State.Idle;
        cuttingTimer = 0.25f;
        cuttingTimerMax = 0.25f;
    }
    private void Update() 
    {
        if(HasKitchenObject())
        {
            switch(state)
            {
                case State.Idle :
                    break;
                case State.Cutting :
                    if(Player.Instance.GetSelectedCounter() == this)
                    {
                        cuttingTimer += Time.deltaTime;
                        if(cuttingTimer >= cuttingTimerMax)
                        {
                            cuttingTimer = 0;
                            cuttingProgress++;

                            OnCut?.Invoke(this, EventArgs.Empty);
                            OnAnyCut?.Invoke(this, EventArgs.Empty);
                            
                            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                            });

                            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                            {
                                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                                GetKitchenObject().DestroyKitchenObject();

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
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //재료 손질 실행
            state = State.Cutting;
        }
    }
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())//ClearCounter에 오브젝트가 없을때
       {
            if(player.HasKitchenObject())
            {
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    state = State.Idle;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

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
            if(player.HasKitchenObject())
            {
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroyKitchenObject();
                    }
                    
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
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
            kitchenObject.SetKitchenObjectParent(this);
            cuttingProgress = 0;
            state = State.Idle;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
        }
        else
        {
            kitchenObject.ConvertObject(this);
        }
        
    }
}
