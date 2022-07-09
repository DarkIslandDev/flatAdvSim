using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance

    public static GameManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Многовато конечно Game манагеров для сцены, найди их все и оставь только один!");
            Load();
            return;
        }
        instance = this;
    }

    #endregion

    public Transform camera;

    void Start()
    {
        camera = Camera.main.transform;
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 0.0f;
            Debug.Log("время всё");
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 1.0f;
            Debug.Log("время не всё");
        }
        
        ChangeBuildingState();
    }

    private void Load()
    {
        
    }

    public void ChangeBuildingState()
    {
        // if(Input.GetKeyDown(KeyCode.Z))
        // {
        //     BuildManager.instance.EnableGridBuildingOnClick();
        // }
        //
        // if(Input.GetKeyDown(KeyCode.X))
        // {
        //     BuildManager.instance.DisableGridEditorOnClick();
        // }
    }
//     [Header("деньги")]
//     public double coin;
//     public double crystal;
//     public double energy;
//     [Space]
//     public int countFood;
//     public int foodCookCost;
    
//     [Header("Готовка")]
//     public float cooking;
//     public float standardTimeCooking;
//     public float bonusTimeCooking;
//     public int foodCount;
//     public int maxFoodCount;
//     public int food;
//     public int recipeCount;
//     [Space]
//     public Text coinText;
//     public Text energyText;
//     public Text crystalText;
//     public Transform foodPanel;
//     public GameObject[] foodPrefabs;
//     [Range(0, 1)]public int bonus;
//     public float bonusTime;
//     public GameObject bonusButton;
    


//     public void Awake()
//     {
//         foodPanel = GameObject.FindWithTag("foodPanel").transform;
//         // bonusButton = GameObject.FindWithTag("bonusButton");
        
//     }

//     public void Update()
//     {
        // this.coinText.text = FormatNums.FormatNum(coin);
//         this.energyText.text = FormatNums.FormatNum(energy);
//         this.crystalText.text = FormatNums.FormatNum(crystal);

//         Cook();
//         // BonusAdd();
//         AddRecipesToCooking();
//     }   

//     public void Cook()
//     {
//         switch(bonus)
//         {
//             case 0:
//                 CookWithoutBonus();
//                 break;
//             case 1:
//                 CookWithBonus();
//                 break;
//         }
//     }

//     public void CookWithoutBonus()
//     {
//         cooking -= Time.deltaTime;
//         if(cooking < 0)
//         {
//             cooking = standardTimeCooking;
//             foodCount++;
//             CreateFood();
//         }
//     }
//     public void CookWithBonus()
//     {
//         cooking -= Time.deltaTime;
//         if(cooking < 0)
//         {
//             cooking = bonusTimeCooking;
//             foodCount++;
//             CreateFood();
//         }

//         bonusTime -=Time.deltaTime;
//         if(bonusTime < 0)
//         {
//             bonus = 0;
//             bonusTime = 60;
//         }
//     }

    // public void CreateFood()
    // {
    //     food = Random.Range(0, foodPrefabs.Length - recipeCount);
        // Instantiate(foodPrefabs[food], foodPanel.transform);
        
        // switch(food)
        // {
        //     case 0:
        //         Instantiate(foodPrefabs[0], foodPanel.transform);
        //         break;
        //     case 1:
        //         Instantiate(foodPrefabs[1], foodPanel.transform);
        //         break;
        //     case 2:
        //         Instantiate(foodPrefabs[2], foodPanel.transform);
        //         break;
        // }
    // }

//     public void AddRecipesToCooking()
//     {
//         recipeCount = 4;
//     }

//     // public void BonusAdd()
//     // {
//     //     if(foodCount <= 0 && bonus == 0)
//     //     {
//     //         bonusButton.SetActive(true);
//     //     }
//     //     else
//     //     {
//     //         bonusButton.SetActive(false);
//     //     }

//     //     if(bonus == 1)
//     //     {
//     //         bonusButton.SetActive(false);
//     //     }
//     // }
}
