using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundGradiant : MonoBehaviour
{
    [SerializeField] private Material material;
    
    //[Header("Colors")]
    //[SerializeField] private Color topColor1;
    //[SerializeField] private Color topColor2;
    //[SerializeField] private Color bottomColor1;
    //[SerializeField] private Color bottomColor2;

    [Header("Attributes")]
    [SerializeField] private float color1FadeDuration;
    [SerializeField] private float color2FadeDuration;
    [SerializeField] private float transitionDuration;
    public float TransitionDuration => transitionDuration;

    private LootScriptableObject.LootRegions currentRegion;
    private LootScriptableObject.LootTimes currentTime;
    [SerializeField] private BackgroundGradientRegion[] regionColors =
    {
        new (LootScriptableObject.LootRegions.Ocean),
        new (LootScriptableObject.LootRegions.River),
        new (LootScriptableObject.LootRegions.Pond)
    };

    private float fadeLength;

    private Coroutine transitionCoroutine;
    private bool transition;
    private float transitionTimer;
    private Color[] transitionColors;

    private void Awake()
    {
        fadeLength = Random.Range(0, 100);
    }

    private void Update()
    {
        fadeLength += Time.deltaTime;

        Color[] colors = transition ? transitionColors : GetFinalColors();
        ApplyLerp("_Color1", color1FadeDuration, colors[0], colors[1]);
        ApplyLerp("_Color2", color2FadeDuration, colors[2], colors[3]);
    }

    private Color[] GetFinalColors()
    {
        return ApplyTime(GetRawColors());
    }

    private Color[] GetRawColors()
    {
        foreach (BackgroundGradientRegion regionColor in regionColors)
        {
            if (regionColor.region == currentRegion)
            {
                return (Color[]) regionColor.colors.Clone();
            }
        }
        
        return null;
    }

    // Lighten/darken the gradient depending on time
    private Color[] ApplyTime(Color[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            switch (currentTime)
            {
                case LootScriptableObject.LootTimes.Day:
                    colors[i] += Color.white * 0.05f;
                    break;
                case LootScriptableObject.LootTimes.Night:
                    colors[i] -= Color.white * 0.2f;
                    break;
            }
        }

        return colors;
    }

    private void StartTransition()
    {
        if(transitionCoroutine != null) 
            StopCoroutine(transitionCoroutine);
        
        transitionCoroutine = StartCoroutine(ApplyTransition(GetFinalColors()));
    }

    private IEnumerator ApplyTransition(Color[] colors)
    {
        Color[] startingColors = (Color[]) colors.Clone();
        transitionColors = new Color[colors.Length];
        transitionTimer = 0.0f;
        transition = true;
        float t;

        while (( t = transitionTimer / transitionDuration) < 1)
        {
            transitionTimer += Time.deltaTime;
            for (int i = 0; i < colors.Length; i++)
            {
                transitionColors[i] = Color.Lerp(startingColors[i], GetFinalColors()[i], t);
            }

            yield return null;
        }

        transition = false;
    }

    private void ApplyLerp(string color, float fadeDuration, Color color1, Color color2)
    {
        float colorT = fadeLength%fadeDuration / fadeDuration * 2;
        if (colorT > 1) colorT = 1+(1-colorT);

        Color newColor = Color.Lerp(color1, color2, colorT);
        material.SetVector(color, newColor);
    }

    public void ChangeRegion(LootScriptableObject.LootRegions region)
    {
        StartTransition();
        
        SetRegion(region);
    }
    
    public void ChangeTime(LootScriptableObject.LootTimes time)
    {
        StartTransition();

        SetTime(time);
    }

    public void SetRegion(LootScriptableObject.LootRegions region)
    {
        currentRegion = region;
    }

    public void SetTime(LootScriptableObject.LootTimes time)
    {
        currentTime = time;
    }
}


[System.Serializable]
public class BackgroundGradientRegion
{
    [ReadOnly]
    public LootScriptableObject.LootRegions region;
    public Color[] colors = new Color[4];

    public BackgroundGradientRegion(LootScriptableObject.LootRegions region)
    {
        this.region = region;
    }
}
