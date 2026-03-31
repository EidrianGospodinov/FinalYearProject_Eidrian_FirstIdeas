using UnityEngine;
using Zenject;

public class CurrencyManager : IInitializable
{
    public int CurrentCurrency { get; private set; }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
    }

    public bool TrySpend(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            return true;
        }

        return false;
    }

    public void Initialize()
    {
        Debug.Log("CurrencyManager has been initialized by zenject!");
        
    }
}