using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

using TMPro;
using System;

public class MapLoader : MonoBehaviour
{
    public TMP_Dropdown fileDropdown;
    string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName;
    public string mapFolderPath = "/Maps";
    public string fileType = "*.json";
    public string currentFile;
    public MapInfos mapInfos;
    public GameObject MapEditorPrefab;
    public GameObject MenuCanvas;
    public GameObject Grid;
    string currentFileFullPath;
    public TMP_InputField InputField;

    
    

    void Start()
    {
        path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName; ;
        string mapPath =  path  + mapFolderPath;
        Debug.Log(mapPath);
        if (!Directory.Exists(mapPath))
            Directory.CreateDirectory(mapPath);
        MapEditorPrefab.SetActive(false);
   

        // Get a list of all the files with the specified file type in the folder
        string[] files = Directory.GetFiles(mapPath, fileType);

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
        currentFileFullPath = path + mapFolderPath + "/" + fileDropdown.captionText.text;
        currentFile = fileDropdown.captionText.text;
        
    }

    public void LoadMap()
    {
        MapEditorPrefab.SetActive(true);
        //MenuCanvas.SetActive(false);
        
        mapInfos.LoadMap(currentFileFullPath);
        currentFile = currentFileFullPath;

    }
    public void LoadNewMap()
    {
        path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName; ;
        string mapPath = path + mapFolderPath;
        //MapEditorPrefab.SetActive(true);
        //MenuCanvas.SetActive(false);
        mapInfos.LoadNewMap();
        string[] files = Directory.GetFiles(mapPath, fileType);

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
    public void Back()
    {
       
    }
    public void SaveMap()
    {
        mapInfos.SaveData(fileDropdown.captionText.text);
    }

    public void RenameFile()
    {
        string oldFile = currentFileFullPath;
        string newFileName = InputField.text;
        string newPath = path + mapFolderPath + "/" + newFileName + ".json";
        File.Move(currentFileFullPath, newPath);
        if (File.Exists(oldFile)) File.Delete(oldFile);

        path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName; ;
        string mapPath = path + mapFolderPath;
        // Get a list of all the files with the specified file type in the folder
        string[] files = Directory.GetFiles(mapPath, fileType);

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

}
