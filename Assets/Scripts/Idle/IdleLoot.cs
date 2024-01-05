using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IdleLoot : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Image loadingBar;

    private UIFadeAnimation alphaAnimationScript;
    private UIScaleAnimation scaleAnimationScript;

    //[Header("Attributes")]
    //[SerializeField] private float defaultGenerateLength;
    //[SerializeField] private float defaultClickPower;
    //private float currentGenerateLength;
    //private float currentClickPower;
    private float currentGenerateLength;
    private float currentClickPower;

    [Header("Notifications")] 
    [SerializeField] private RectTransform timeNotificationSpawn;
    [SerializeField] private GameObject timeNotificationPrefab;
    [SerializeField] private float timeNotificationDuration;
    [SerializeField] private float timeNotificationDistance;


    private float generateTime;

    private void Awake()
    {
        //currentGenerateLength = CalculateGenerateLength();
        //currentClickPower = CalculateClickPower();

        alphaAnimationScript = GetComponent<UIFadeAnimation>();
        scaleAnimationScript = GetComponent<UIScaleAnimation>();
        
    }

    private void Start()
    {
        StartGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        generateTime += Time.deltaTime;

        float progress = generateTime / currentGenerateLength;

        if (progress < 1.0f)
            Generate(progress);
        else
            EndGenerate();

    }

    private void StartGenerate()
    {
        generateTime = 0.0f;
        currentGenerateLength = Player.FishingSpeedValues[Player.GetUpgradeStage(Player.UpgradeIDs.FishingSpeed)];

    }

    private void Generate(float progress)
    {
        loadingBar.fillAmount = progress;
    }

    private void EndGenerate()
    {
        loadingBar.fillAmount = 1;
            
        LootManager.Instance.GenerateGeneric();
        
        StartGenerate();
    }
    public void OnClickButton()
    {
        currentClickPower = Player.ClickingPowerValues[Player.GetUpgradeStage(Player.UpgradeIDs.ClickingPower)];
        
        generateTime += currentClickPower;

        IdleTimeNotification currentNotification = Instantiate(timeNotificationPrefab, Input.mousePosition, Quaternion.identity, timeNotificationSpawn).GetComponent<IdleTimeNotification>();
        currentNotification.Setup(currentClickPower, timeNotificationDuration, timeNotificationDistance);
    }
    
}
