using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    private LootScriptableObject lootObject;

    private Vector2 direction;
    private Vector2 startPos;
    private bool moving;
    private Camera cam;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    [SerializeField] private Vector2 speedBounds;
    private float speed;

    [Header("Hover Animation")] 
    private bool mouseOver;
    private bool hovering;
    private Vector2 defaultHoverScale;
    private IAnimation hoverAnimation;
    [SerializeField] private float hoverScale;
    [SerializeField] private float hoverScaleLength;
    [SerializeField] private Easing.EaseType hoverScaleType;
    
    [Header("ClickAnimation")]
    [SerializeField] private float clickScale;
    [SerializeField] private float clickScaleLength;
    [SerializeField] private Easing.EaseType clickScaleType;
    [SerializeField] private float fadeLength;
    [SerializeField] private Easing.EaseType fadeEaseType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        cam = Camera.main;

        minX = cam!.ViewportToWorldPoint(new Vector2(-0.1f, -0.1f)).x;
        minY = cam!.ViewportToWorldPoint(new Vector2(-0.1f, -0.1f)).y;
        maxX = cam!.ViewportToWorldPoint(new Vector2(1.1f, 1.1f)).x;
        maxY = cam!.ViewportToWorldPoint(new Vector2(1.1f, 1.1f)).y;

        defaultHoverScale = spriteRenderer.transform.localScale;

        speed = Random.Range(speedBounds.x, speedBounds.y);
    }

    private void Update()
    {
        
        if((!mouseOver || EventSystem.current.IsPointerOverGameObject()) && hovering )
            OnMouseExit();
        mouseOver = false;
        
        float step = speed * Time.deltaTime;
        transform.position += new Vector3(direction.x,direction.y,0) * step;
        
        if(moving && !CheckBounds(transform.position))
            Remove();
        

    }

    private void Start()
    {
        // Creates random values
        float randomPos = Random.Range(0.0f, 1.0f);
        float randomSide = Random.Range(0, 2);
        bool isX = Random.value > 0.5f;
        // Randomizes the vector to be randomly along the edge of the screen
        Vector2 startVector = isX ? new Vector2(randomSide, randomPos) : new Vector2(randomPos, randomSide);
        startPos = Camera.main!.ViewportToWorldPoint(isX ? new Vector2(randomSide, randomPos) : new Vector2(randomPos, randomSide));
        
        // Mid point which the fish travels in the direction of
        Vector2 midPoint = cam.ViewportToWorldPoint(new Vector2(0.5f + Random.Range(-0.25f,0.25f),0.5f + Random.Range(-0.25f,0.25f)));
        
        direction = (midPoint - startPos).normalized;
        transform.position = startPos;

        // Flip sprite based on direction
        spriteRenderer.flipX = direction.x > 0;
        
        moving = true;
    }

    public void Setup(LootScriptableObject loot)
    {
        lootObject = loot;
        
        spriteRenderer.sprite = loot.LootSprite;
    }

    private bool CheckBounds(Vector2 position)
    {
        return !(position.x < minX) && !(position.x > maxX) &&
               !(position.y < minY) && !(position.y > maxY);
    }

    private void Collect()
    {
        GameManager.Instance.GetInventory().AddToDictionary(lootObject.LootID);
        
        Remove();
    }
    
    private void Remove()
    {
        moving = false;
        
        // Animations
        var localScale = transform.localScale;
        Animations.Instance.PlayAnimation(this, newScale =>{if (gameObject.activeSelf){transform.localScale = newScale;}}, clickScaleLength, localScale, localScale*clickScale, Easing.FindEaseType(clickScaleType), Animations.LoopType.SingleYoYo);
        Color spriteColor = spriteRenderer.color;
        Animations.Instance.PlayAnimation(this, newValue => spriteRenderer.color = new Color(spriteColor.r,spriteColor.g,spriteColor.b,newValue),  fadeLength, spriteColor.a, 0.0f, Easing.FindEaseType(fadeEaseType), Animations.LoopType.Single, Kill);
    }

    private void Kill()
    {
        Destroy(gameObject);
    }

    public void OnMouseOver()
    {
        mouseOver = true;
        
        // Dont run multiple times
        if (hovering) return;
        
        // Dont run if clicked
        if (!moving) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        hovering = true;
        
        var localScale = spriteRenderer.transform.localScale;
        Animations.Instance.StopAnimation(hoverAnimation);
        hoverAnimation = Animations.Instance.PlayAnimation(this, newScale =>{if (gameObject.activeSelf){spriteRenderer.transform.localScale = newScale;}}, hoverScaleLength, localScale, defaultHoverScale*hoverScale, Easing.FindEaseType(hoverScaleType), Animations.LoopType.Single);
    }

    public void OnMouseExit()
    {
        hovering = false;
        
        // Dont run if clicked
        if (!moving) return;
        
        var localScale = spriteRenderer.transform.localScale;
        Animations.Instance.StopAnimation(hoverAnimation);
        hoverAnimation = Animations.Instance.PlayAnimation(this, newScale =>{if (gameObject.activeSelf){spriteRenderer.transform.localScale = newScale;}}, hoverScaleLength, localScale, defaultHoverScale, Easing.FindEaseType(hoverScaleType), Animations.LoopType.Single);
    }

    public void OnMouseDown()
    {
        // Let UI block it
        if (EventSystem.current.IsPointerOverGameObject()) return;
        // Prevent double clicking
        if (!moving) return;
        
        Collect();
    }
}