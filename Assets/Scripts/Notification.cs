using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    private NotificationManager notificationManager;
    private UIItemPanel itemPanel;

    private float notificationTime;
    private float maxNotificaitonTime;
    public bool Active { get; private set; }

    [Header("Spawn Animation")] 
    [SerializeField] private float spawnLength;
    [SerializeField] private float spawnDistance;
    [SerializeField] private Easing.EaseType spawnEaseType;
    
    [Header("Fade Animation")] 
    [SerializeField] private float fadeLength;
    [SerializeField] private float fadeValue;
    [SerializeField] private Easing.EaseType fadeEaseType;
    
    private void Awake()
    {
        gameObject.SetActive(false);
        Active = false;
    }


    private void Update()
    {
        if (!Active) return;
        
        notificationTime += Time.deltaTime;
        float t = notificationTime / maxNotificaitonTime;
        if (t >= 1.0f)
        {
            Remove();
        }

    }

    public void Spawn(float duration, LootScriptableObject item, int count)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        
        if(!itemPanel)itemPanel = GetComponentInChildren<UIItemPanel>();
        if(!notificationManager)notificationManager = GetComponentInParent<NotificationManager>();


        Active = true;

        notificationTime = 0.0f;
        maxNotificaitonTime = duration;
        
        itemPanel.SetupPanel(item, count);

        rectTransform.anchoredPosition -= new Vector2(0,spawnDistance);
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        Animations.Instance.PlayAnimation(this, newValue => rectTransform.anchoredPosition = newValue, spawnLength, anchoredPosition, (Vector3)anchoredPosition+new Vector3(0,spawnDistance,0), Easing.FindEaseType(spawnEaseType), Animations.LoopType.Single);
    }

    public void Remove()
    {
        notificationTime = 0;
        
        Active = false;
        
        notificationManager.RemoveItem();
        
        Animations.Instance.PlayAnimation(this, newValue => canvasGroup.alpha = newValue,  fadeLength, canvasGroup.alpha, fadeValue, Easing.FindEaseType(fadeEaseType), Animations.LoopType.Single, Despawn);
    }

    public void Despawn()
    {
        notificationManager.DespawnItem();
        
        Destroy(gameObject);
        
    }
}
