using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface used on scripts that need to save and load data
public interface ISave
{
    void LoadData(SaveGameData saveGameData);
    void SaveData (SaveGameData saveGameData);
}
