using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootManager : MonoBehaviour
{
    
    public static LootManager Instance { get; private set; }

    // Generic list of all loot
    public static List<LootScriptableObject> lootScriptableObjects = new();

    // Split that loot into individual lists for quicker access
    public static List<LootScriptableObject> fishScriptableObjects = new();
    public static List<LootScriptableObject> materialScriptableObjects = new();
    public static List<LootScriptableObject> junkScriptableObjects = new();
    
    // All bundles
    public static List<BundleScriptableObject> bundleScriptableObjects = new();
    
    // All upgrades
    public static List<UpgradeScriptableObject> upgradeScriptableObjects = new();

    [SerializeField] private int defaultCommonWeight = 250;
    [SerializeField] private int defaultRareWeight = 100;
    [SerializeField] private int defaultEpicWeight = 50;
    [SerializeField] private int defaultLegendaryWeight = 10;
    [SerializeField] private int defaultTranscendedWeight = 1;

    private Dictionary<LootScriptableObject.LootRarities, int> lootWeights = new();
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
            GenerateGeneric();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return;
        } 
        else 
        { 
            Instance = this; 
        } 

        DontDestroyOnLoad(this.gameObject);
        
        lootScriptableObjects = new List<LootScriptableObject>();
        fishScriptableObjects = new List<LootScriptableObject>();
        materialScriptableObjects = new List<LootScriptableObject>();
        junkScriptableObjects = new List<LootScriptableObject>();
        
        // Gets all loot
        // Sorted in rarity and alphabetical order
        lootScriptableObjects = Resources.LoadAll<LootScriptableObject>(string.Empty).ToList().OrderBy(loot => loot.LootRarity).ThenBy(loot => loot.LootName).ToList();

        // Stores loot in individual lists based on type
        foreach (LootScriptableObject loot in lootScriptableObjects)
        {
            switch (loot.LootType)
            {
                case LootScriptableObject.LootTypes.Fish:
                    fishScriptableObjects.Add(loot);
                    break;
                case LootScriptableObject.LootTypes.Material:
                    materialScriptableObjects.Add(loot);
                    break;
                case LootScriptableObject.LootTypes.Junk:
                    junkScriptableObjects.Add(loot);
                    break;
            }
        }
        
        // Does the same for bundles and upgrades
        // Sorted in rarity and alphabetical order
        bundleScriptableObjects = Resources.LoadAll<BundleScriptableObject>(string.Empty).ToList().OrderBy(bundle => bundle.BundleRarity).ThenBy(bundle => bundle.BundleName).ToList();
        // Sorted by manually given index
        upgradeScriptableObjects = Resources.LoadAll<UpgradeScriptableObject>(string.Empty).ToList().OrderBy(upgrade => upgrade.UpgradeIndex).ThenBy(upgrade => upgrade.UpgradeName).ToList();
        
        // Initialise rarity weights
        lootWeights = new Dictionary<LootScriptableObject.LootRarities, int>
        {
            [LootScriptableObject.LootRarities.Common] = defaultCommonWeight,
            [LootScriptableObject.LootRarities.Rare] = defaultRareWeight,
            [LootScriptableObject.LootRarities.Epic] = defaultEpicWeight,
            [LootScriptableObject.LootRarities.Legendary] = defaultLegendaryWeight,
            [LootScriptableObject.LootRarities.Transcended] = defaultTranscendedWeight
        };
    }

    // Probably unoptimised as HELL but i dont got time to improve
    public Dictionary<LootScriptableObject, int> OfflineGenerate(int count)
    {
        Dictionary<LootScriptableObject, int> offlineLoot = new();

        for (int i = 0; i < count; i++)
        {
            List<LootScriptableObject> generatedLoot = GenerateGeneric();

            foreach (LootScriptableObject loot in generatedLoot)
            {
                if (offlineLoot.ContainsKey(loot))
                    offlineLoot[loot]++;
                else
                    offlineLoot.Add(loot,1);
            }
        }

        return offlineLoot;
    }

    // Generates a random loot of a random type
    public List<LootScriptableObject> GenerateGeneric()
    {
        // Bonus chance logic
        float bonusCatchChance = Player.BonusCatchValues[Player.GetUpgradeStage(Player.UpgradeIDs.BonusCatch)];
        int loopCount = 1;
        // Loop if the value is above 0%
        while (bonusCatchChance > 0.0f)
        {
            // Uses decimal as percentage chance of increasing loop count
            float rand = Random.Range(0.0f, 1.0f);
            if (bonusCatchChance > rand)
                loopCount++;
            
            // Decreases the chance by 100%
            // This allows for values above 100% to have a chance of generating multiple extra loops
            bonusCatchChance -= 1.0f;
        }

        List<LootScriptableObject> lootList = new();
        
        // Generate logic
        // Loops for however extra loops it needs
        do
        {
            // Calculate what type of loot to generate
            int randomType = Random.Range(1, 12);
            switch (randomType)
            {
                case <= 10:
                    lootList.Add(GenerateLoot(fishScriptableObjects));
                    break;
                case <= 11:
                    // Apply additional junk generation rate
                    float rand = Random.Range(0.0f,1.0f);
                    float junkRate = Player.JunkRateValues[Player.GetUpgradeStage(Player.UpgradeIDs.JunkRate)];
                    // Generates junk instead depending on the random chance
                    lootList.Add(GenerateLoot(rand < junkRate ? junkScriptableObjects : fishScriptableObjects));
                    break;
            }

            loopCount--;
        } while (loopCount > 0);

        foreach (LootScriptableObject loot in lootList)
        {
            GameManager.Instance.GetInventory().AddToDictionary(loot.LootID);
        }

        return lootList;


    }
    
    // Turns list of potential items into list of loot items, then chooses random item
    // This is done every time loot is rolled so that each rarities weight can dynamically change if needed in the future
    public LootScriptableObject GenerateLoot(List<LootScriptableObject> lootList)
    {
        List<LootRoll> lootRolls = new();
        
        // Stores the weight of each loot item alongside it
        foreach (LootScriptableObject loot in lootList)
        {
            // Checks if item is either in the wrong region, or wrong time
            // Unless its 0 which means all
            if ((!loot.LootRegion.HasFlag(GameManager.Instance.GetPlayer().CurrentRegion) && loot.LootRegion != 0) ||
                (!loot.LootTime.HasFlag(GameManager.Instance.GetPlayer().CurrentTime) && loot.LootTime != 0))
                continue;
            
            // Calculates weight based on raritys weight, as well as the rarities fishing luck value upgrade
            LootRoll lootRoll = new LootRoll(loot, (int)(lootWeights[loot.LootRarity] * Player.FishingLuckValues[Player.GetUpgradeStage(Player.UpgradeIDs.FishingLuck)][loot.LootRarity]));
            lootRolls.Add(lootRoll);
        }

        // Store random loot object from list
        LootScriptableObject roll = RollLoot(GetTotalWeight(lootRolls), lootRolls);
        
        // Add to inventory
        //GameManager.Instance.GetInventory().AddToDictionary(roll.loot.LootID);
        
        //Debug.Log("" + roll.loot.LootName+ " (" + roll.loot.LootType + ")");
        
        return roll;
    }

    // Returns random weighted loot from list
    private LootScriptableObject RollLoot(int totalWeight, List<LootRoll> lootRolls)
    {
        int randomWeight = Random.Range(0, totalWeight);
        int currentWeight = 0;

        // Loops through all the loot, adding their weight together
        // If the current weight is higher than the random weight, then this loot is chosen
        foreach(LootRoll lootRoll in lootRolls)
        {
            // Current weight compares the current loot with the random weight
            currentWeight += lootRoll.GetWeight();
            if(randomWeight < currentWeight)
            {
                return lootRoll.loot;
            }
        }

        // If error
        return null;
    }
    
    // Returns combined weight of all loot
    private int GetTotalWeight(IEnumerable<LootRoll> lootRolls)
    {
        return lootRolls.Sum(lootRoll => lootRoll.GetWeight());
    }
}

[Serializable]
// Used so that each loot item can have an individual variable weight
public class LootRoll
{
    public LootRoll(LootScriptableObject loot, int weight)
    {
        this.loot = loot;
        this.weight= weight; 
    }

    public LootScriptableObject loot;
    private int weight;

    public int GetWeight()
    {
        return weight;
    }
}