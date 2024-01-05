using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject notificationObject;
    [SerializeField] private RectTransform panelSpawn;
    private Notification currentNotification;
    private List<Notification> notifications = new();
    [SerializeField] private float notificationDuration;
    [SerializeField] private float notificationLimit;
    
    [Header("Shift Animation")] 
    [SerializeField] private float shiftLength;
    [SerializeField] private float shiftDistance;
    [SerializeField] private Easing.EaseType shiftEaseType;

    private void Start()
    {
        GameManager.Instance.GetInventory().AddItemEvent += AddItem;

    }

    private void ShiftItems()
    {
        panelSpawn.anchoredPosition -= new Vector2(0,shiftDistance);
        Vector2 anchoredPosition = panelSpawn.anchoredPosition;
        Animations.Instance.PlayAnimation(this, newValue => panelSpawn.anchoredPosition = newValue,  shiftLength, anchoredPosition, (Vector3)anchoredPosition+new Vector3(0,shiftDistance,0), Easing.FindEaseType(shiftEaseType), Animations.LoopType.Single);
    }

    private void AddItem( LootScriptableObject item, int itemCount)
    {

        currentNotification = Instantiate(notificationObject, panelSpawn.transform).GetComponent<Notification>();

        currentNotification.Spawn(notificationDuration, item, itemCount);
        
        notifications.Add(currentNotification);

        if (notifications.Count >= notificationLimit)
        {
            
            // Remove the top of the list to make space for new notification
            notifications[0].Remove();

        }

    }

    public void RemoveItem()
    {
        notifications.RemoveAt(0);
    }

    public void DespawnItem()
    {
        ShiftItems();
        

    }
    
}
