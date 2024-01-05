using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootScriptableObject", menuName = "ScriptableObjects/Bundle")]
public class BundleScriptableObject : ScriptableObject
{
    [SerializeField] private string bundleName;
    [SerializeField] private LootItem[] input;
    [SerializeField] private LootItem[] output;
    [SerializeField] private LootScriptableObject.LootRarities bundleRarity;

    public string BundleName => bundleName;
    public LootItem[] Input => input;
    public LootItem[] Output => output;
    public LootScriptableObject.LootRarities BundleRarity => bundleRarity;

}
