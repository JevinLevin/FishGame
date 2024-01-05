using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private OfflineTime offlineTime;
    public BackgroundGradiant BackgroundManager { get; private set; }
    
    [field: Header("Colors")]
    [field: SerializeField] public Color CommonColor{get; private set;}
    [field: SerializeField] public Color RareColor{get; private set;}
    [field: SerializeField] public Color EpicColor{get; private set;}
    [field: SerializeField] public Color LegendaryColor{get; private set;}
    [field: SerializeField] public Color TransColor{get; private set;}
    
    [field: SerializeField] public Color YesColor{get; private set;}
    [field: SerializeField] public Color NoColor{get; private set;}


    private bool firstTick = true;

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

        BackgroundManager = GetComponent<BackgroundGradiant>();
    }

    private void LateUpdate()
    {
        // Once this is ran, the first game tick will have ended
        firstTick = false;
    }

    public Inventory GetInventory()
    {
        return playerInventory;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void DisplayOfflineTime(TimeSpan timeSpan)
    {
        offlineTime.Display(timeSpan);
    }
    
    public bool IsFirstTick()
    {
        return firstTick;
    }

}