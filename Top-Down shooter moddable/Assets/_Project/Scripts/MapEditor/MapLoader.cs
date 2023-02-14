using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class MapLoader : MonoBehaviour
{
    public TMP_Dropdown fileDropdown;
    public string folderPath = "/Maps";
    public string fileType = "*.json";
    public string currentFile;
    public MapInfos mapInfos;
    public GameObject MapEditorPrefab;
    public GameObject MenuCanvas;
    string currentFileFullPath;
    public TMP_InputField InputField;

    
    

    void Start()
    {
        MapEditorPrefab.SetActive(false);
        // Get the path to the folder
        string fullPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + folderPath;

        // Get a list of all the files with the specified file type in the folder
        string[] files = Directory.GetFiles(fullPath, fileType);

        // Create an array of just the file names without the path
        string[] fileNames = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            fileNames[i] = Path.GetFileName(files[i]);
        }

        // Create a list of Dropdown.OptionData objects from the file names
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string fileName in fileNames)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(fileName);
            options.Add(option);
        }

        // Set the options for the dropdown
        fileDropdown.ClearOptions();
        fileDropdown.AddOptions(options);
    }
    private void Update()
    {
        currentFileFullPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + folderPath + "/" + fileDropdown.captionText.text;
        currentFile = fileDropdown.captionText.text;
        
    }

    public void LoadMap()
    {
        MapEditorPrefab.SetActive(true);
        MenuCanvas.SetActive(false);
        
        mapInfos.LoadMap(currentFileFullPath);
        currentFile = currentFileFullPath;
    }
    public void LoadNewMap()
    {
        MapEditorPrefab.SetActive(true);
        MenuCanvas.SetActive(false);
        mapInfos.LoadNewMap();
        

    }
    public void SaveMap()
    {
        mapInfos.SaveData(fileDropdown.captionText.text);
    }

    public void RenameFile()
    {
        string oldFile = currentFileFullPath;
        string newFileName = InputField.text;
        string newPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + folderPath + "/" + newFileName + ".json";
        File.Move(currentFileFullPath, newPath);
        if (File.Exists(oldFile)) File.Delete(oldFile);
    }

}
