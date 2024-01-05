using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

// File read and writing system, simplified from youtube tutorial https://www.youtube.com/watch?v=aUi9aijvpgs
public class SavingManager : MonoBehaviour
{
    public static SavingManager Instance {get; private set;}

    [SerializeField] private bool disableSaving = false;
    [SerializeField] private string saveFileName;
    
    public SaveGameData saveGameData;
    private List<ISave> savingObjects;
    
    private string dataDirPath = "";
    private string saveDirPath = "";
    
    private void Awake()
    {
        // Singleton behaviour
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
        //
        
        if(disableSaving)
        {
            Debug.LogWarning("SAVING IS DISABLED");
        }

        dataDirPath = Application.persistentDataPath;
        saveDirPath = Path.Combine(dataDirPath, "playerdata");


    }
    
    // Initialise new game by creating new game data
    private void NewGame()
    {
        saveGameData = new();
    }

    // Load game data into game objects
    private void LoadGame()
    {
        if(disableSaving)
        {
            return;
        }

        saveGameData = LoadFile(); 

        // If theres no current save data create new game
        if(saveGameData == null)
        {
            Debug.LogWarning("No save data found, starting new game");
            NewGame(); 
        }

        // Load into objects
        foreach(ISave savingObject in savingObjects)
        {
            savingObject.LoadData(saveGameData);
        }
    }

    // Save current game from game objects
    private void SaveGame()
    {
        if(disableSaving)
        {
            return;
        }
        
        // If no data is saved
        if(saveGameData == null)
        {
            return;
        }
        
        // Save all game data
        foreach(ISave savingObject in savingObjects)
        {
            savingObject.SaveData(saveGameData);
        }
        
        saveGameData.lastLogin = DateTime.Now;

        // Write the save data to file
        SaveFile(saveGameData);
    }
    
    // Load data from text file
    private SaveGameData LoadFile()
    {
        // Get the full path to save the file (including file name)
        string fullPath = GetFullPath(saveFileName);
        
        SaveGameData loadedData = null;
        
        // Only run the code if the file actually exists
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                // File reading
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
            
                loadedData = JsonConvert.DeserializeObject<SaveGameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error with save file, starting new save " + e);
                return new SaveGameData();
            }
        }
        return loadedData;
    }

    // Save object data to text file
    public void SaveFile(SaveGameData data) 
    {
        // Get the full path to save the file (including file name)
        string fullPath = GetFullPath(saveFileName);
        
        // Creates the file incase it doesnt exist yet
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        // Convert data to string
       string dataToStore = JsonConvert.SerializeObject(data,formatting:Formatting.Indented, 
           // This option prevents the 'self referencing loop' error caused by serializing certain structs such as a vector
       new JsonSerializerSettings
       {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
       });

       // Write data to the file
       using (FileStream stream = new FileStream(fullPath, FileMode.Create))
       {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
       }
        
    }
    
    // Runs scene loaded function when the game starts
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stores all objects in game that need to be saved
        savingObjects = FindAllSavingObjects();

        // Load data into them
        LoadGame();
    }
    
    // Save the game when it closes
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    // Uses the interface to find all savable objects
    private List<ISave> FindAllSavingObjects()
    {
        IEnumerable<ISave> findSavingObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<ISave>();

        return new List<ISave>(findSavingObjects);
    }
    
    // Combines folder path with file name
    private string GetFullPath(string dataFileName)
    {
        return Path.Combine(saveDirPath, dataFileName);
    }

}
