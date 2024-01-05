using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LootScriptableObject", menuName = "ScriptableObjects/Loot")]
public class LootScriptableObject : ScriptableObject
{
    public enum LootTypes
    {
        Fish,
        Material,
        Junk
    }

    public enum LootRarities
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Transcended
    }

    [System.Flags] [System.Serializable]
    public enum LootRegions
    {
        All=0,
        Ocean=1,
        Pond=2,
        River=4
    }
    [System.Flags] [System.Serializable]
    public enum LootTimes
    {
        All=0,
        Day=1,
        Night=2
    }

    [SerializeField] private LootTypes lootType;
    [SerializeField] private LootRarities lootRarity;
    [SerializeField] private LootRegions lootRegion;
    [SerializeField] private LootTimes lootTime;
    [SerializeField] private string lootName;
    [SerializeField] private string lootId;
    [SerializeField] private Sprite lootSprite;

    // => shorthand creates a readonly property (perfect for scriptable objects
    public LootTypes LootType => lootType;
    public LootRarities LootRarity => lootRarity;
    public LootRegions LootRegion => lootRegion;
    public LootTimes LootTime => lootTime;
    public string LootName => lootName;
    public string LootID => lootId;
    public Sprite LootSprite => lootSprite;

    public static Color GetRarityColor(LootRarities lootRarity)
    {
        switch (lootRarity)
        {
            case LootRarities.Common:
                return GameManager.Instance.CommonColor;
            case LootRarities.Rare:
                return GameManager.Instance.RareColor;
            case LootRarities.Epic:
                return GameManager.Instance.EpicColor;
            case LootRarities.Legendary:
                return GameManager.Instance.LegendaryColor;
            case LootRarities.Transcended:
                return GameManager.Instance.TransColor;

            default:
                return Color.black;
        }
    }
}





// Gives each scriptable object an icon in the editor, taken from https://discussions.unity.com/t/custom-icon-on-scriptableobject/201591/5
#if UNITY_EDITOR
[CustomEditor(typeof(LootScriptableObject))]
public class LootEditor : Editor
{
    public override Texture2D RenderStaticPreview(
        string assetPath, UnityEngine.Object[] subAssets, int width, int height
    )
    {
        var item = (LootScriptableObject) target;

        if (item == null || item.LootSprite == null)
        {
            return null;
        }

        var texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(item.LootSprite.texture, texture);
        texture.filterMode = FilterMode.Point;
        return texture;
    }

}
#endif
