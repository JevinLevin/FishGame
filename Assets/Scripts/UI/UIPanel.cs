using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform mainPanel;
    private CanvasGroup canvasGroup;
    private ButtonsUI buttonManager;

    [Header("Attributes")]
    [SerializeField] private float transitionInLength = 0.3f;
    [SerializeField] private float transitionOutLength = 0.2f;
    [SerializeField] private Easing.EaseType transitionInEase = Easing.EaseType.OutBack;
    [SerializeField] private Easing.EaseType transitionOutEase = Easing.EaseType.InSine;
    

    [Header("Events")]
    public UnityEvent OnOpen;
    public UnityEvent OnClose;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        // Moves panel to default offscreen position
        mainPanel.anchoredPosition = new Vector3(0,-500,0);
    }
    
    private void Start()
    {
        // Make sure UI panel always starts disabled
        if(GameManager.Instance.IsFirstTick()) gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            CloseUI();
    }

    public void OpenUI()
    {
        // If there is no button manager reference find one
        if(!buttonManager)         
            buttonManager = FindObjectOfType<ButtonsUI>(true);

        // Dont do anything if UI is already open
        if (buttonManager.IsUIOpen) return;
        buttonManager.OnOpenUI();
        
        UIBackgroundTint.FadeInBackgroundTint();

        gameObject.SetActive(true);
        StartCoroutine(TransitionAnimation(true, transitionInLength, Easing.FindEaseType(transitionInEase)));
    }

    public void CloseUI()
    {
        UIBackgroundTint.FadeOutBackgroundTint();
        
        buttonManager.OnCloseUI();

        StartCoroutine(TransitionAnimation(false, transitionOutLength, Easing.FindEaseType(transitionOutEase)));
    }

    private IEnumerator TransitionAnimation(bool open, float length, Easing.EaseTypeDelegate easeFunction)
    {
        // Prevent any buttons being pressed during animation
        canvasGroup.interactable = false;
        
        // Set to default category when opening
        if(open)
            OnOpen?.Invoke();
        
        Vector3 startPos = mainPanel.anchoredPosition;
        
        // Sets target position to default position
        Vector3 targetPosition = Vector3.zero;
        // If closing, change target to offscreen
        if (!open)
            targetPosition = new Vector3(0,-500,0);
        

        float animationDuration = 0.0f;
        while (animationDuration < length)
        {
            float t = animationDuration / length;
            // Move inventory
            mainPanel.anchoredPosition = Vector3.LerpUnclamped(startPos, targetPosition, easeFunction(t));
            // Fade inventory
            // Uses the open bool as the off and on fade values
            canvasGroup.alpha = Mathf.Lerp(System.Convert.ToSingle(!open), System.Convert.ToSingle(open), Easing.easeOutQuad(t));
            
            animationDuration += Time.deltaTime;
            yield return null;
        }
        
        // Disables game object if closing
        if (!open)
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
        }
        
        canvasGroup.interactable = true;
        canvasGroup.alpha = System.Convert.ToSingle(open);

    }
    
    public static IEnumerator ResetScrollBar(Scrollbar scrollbar)
    {
        yield return new WaitForEndOfFrame();
        
        scrollbar.value = 1f;
    }
}
