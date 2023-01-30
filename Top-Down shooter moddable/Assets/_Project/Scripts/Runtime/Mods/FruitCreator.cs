using Etienne;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitCreator : MonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField durationField;
    [SerializeField, ReadOnly] private float duration;

    [Header("FruitStages")]
    [SerializeField] private FruitStagesCreator stagesCreator;

    [Header("Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button addButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(CreateFruitMod);
        cancelButton.onClick.AddListener(ResetMod);
        nameField.onValueChanged.AddListener(CheckEmpty);
        durationField.onValueChanged.AddListener(OnlyNumbers);
        durationField.onValueChanged.AddListener(CheckEmpty);

        ResetMod();
    }

    private void OnlyNumbers(string text)
    {
        text = text.Replace('.', ',');
        text = Regex.Replace(text, @"[^\d,]", "");
        int firstIndex = text.IndexOf(',');
        if (firstIndex > 0 && firstIndex + 1 < text.Length)
        {
            int secondIndex = text.IndexOf(',', firstIndex + 1);
            if (secondIndex > 0 && secondIndex < text.Length)
            {
                text = text.Substring(0, secondIndex);
            }
        }
        text = text.Substring(0, Mathf.Min(text.Length, 4));
        durationField.SetTextWithoutNotify(text);
        float.TryParse(text, out duration);
    }

    private void CheckEmpty(string text)
    {
        confirmButton.interactable = nameField.text.Length > 0 && durationField.text.Length > 0;
    }

    private void CreateFruitMod()
    {
        string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/{Application.productName}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        SimpleFileBrowser.FileBrowser.ShowSaveDialog(SaveFile, CancelSave, SimpleFileBrowser.FileBrowser.PickMode.Files, false, path, nameField.text);
    }

    private void SaveFile(string[] paths)
    {
        string path = paths[0];
        Debug.Log($"Save Mod {nameField.text} at {path}");
        FruitData fruit = ScriptableObject.CreateInstance<FruitData>();
        fruit.name = nameField.text + "Fruit";
        SeedData seed = ScriptableObject.CreateInstance<SeedData>();
        seed.name = nameField.name + "Seed";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        string fruitJson = JsonUtility.ToJson(fruit);
        File.WriteAllText($"{path}/{fruit.name}.json", fruitJson);
    }

    private void CancelSave()
    {
        Debug.Log($"Cancel Save");
    }

    private void ResetMod()
    {
        nameField.text = "";
        durationField.text = "";
        stagesCreator.ResetMod();
    }
}
