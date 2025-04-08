using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int Money { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        Debug.Log($"Got {amount} coins. Total: {Money}");
    }

    public bool TrySpend(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log($"Spended {amount} coins. Total: {Money}");
            return true;
        }

        Debug.Log("Not enough.");
        return false;
    }

    public void LoadMoney(int amount)
    {
        Money = amount;
    }
}