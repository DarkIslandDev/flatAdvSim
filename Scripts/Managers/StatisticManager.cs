using UnityEngine;

public class StatisticManager : MonoBehaviour 
{
    #region Instance

    public static StatisticManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Многовато конечно build манагеров для сцены, найди их все и оставь только один!");
            return;
        }
        instance = this;
    }

    #endregion

    [Header("UI")]
    public GameObject NotEnougheMoneyPanel;
    public Transform NotEnougheMoneyTransform;
    
    [Space]
    [Header("Таймер")]
    public static float standartTimer = 10.0f;
    public static float bonusTimer = 5.0f;
    public bool isTimer;

    [Space]
    [Header("численные")]
    public static double Coin;
    public double startCoin = 500;
    public static double Energy;
    public double startEnergy;
    public static double Crystal;

    [Header("ресурсы")]
    [SerializeField] public static double Wood;
    [SerializeField] public static double Rock;
    [SerializeField] public static double Mushroom;
    
    void Start()
    {
        GlobalEventManager.UICreateNotEnoughMoneyMessege.AddListener(CreateNotEnoughMoney);
        Coin = startCoin;
        Energy = startEnergy;
    }

    void Update()
    {
    }

    public void CreateNotEnoughMoney()
    {
        Instantiate(NotEnougheMoneyPanel, NotEnougheMoneyTransform);
        // Destroy(NotEnougheMoneyPanel, 2.0f);
    }
}