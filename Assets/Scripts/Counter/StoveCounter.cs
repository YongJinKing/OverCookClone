using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress, ICanPlaceKitchenObject
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    protected override void Start() 
    {
        base.Start();
        state = State.Idle;
    }
    private void Update() 
    {
        if(HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle :
                    break;
                case State.Frying :
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                    });
                    if(fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroyKitchenObject();

                        KitchenObject.SpawKitchenObject(fryingRecipeSO.output, this);
                        
                        state = State.Fried;
                        
                        burningTimer = 0f;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                    }
                    break;
                case State.Fried :
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = burningTimer / burningRecipeSO.burningTimerMax,
                    });
                    if(burningTimer > burningRecipeSO.burningTimerMax)
                    {
                            
                        GetKitchenObject().DestroyKitchenObject();
                        KitchenObject.SpawKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0,
                    });
                    }
                    break;
                case State.Burned :
                    break;
            }  
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

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    
                    state = State.Frying;

                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
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

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                            state = state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                                progressNormalized = 0,
                        });
                        
                    }
                    
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                    state = state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = 0,
                });
            }
       }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO FryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return FryingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if(fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
        
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach(BurningRecipeSO bruningRecipeSO in burningRecipeSOArray)
        {
            if(bruningRecipeSO.input == inputKitchenObjectSO)
            {
                return bruningRecipeSO;
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

            fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    
            state = State.Frying;

            fryingTimer = 0f;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
                state = state
            });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
            });
            
        }
        else
        {
            kitchenObject.ConvertObject(this);
        }
        
    }

}
