using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{

    [Header("UI")]
    [SerializeField]
    private TMP_InputField nameInputField = null;
    [SerializeField]
    private Button continueBtn = null;

    public static string displayName { get; private set; }
    private const string playerPrefsNameKey = "PlayerName";

    private void Start() => SetInputField();

    private void SetInputField()
    {
        if (!PlayerPrefs.HasKey(playerPrefsNameKey))
            return;

        string defaultName = PlayerPrefs.GetString(playerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
                
    }

    public void SetPlayerName(string name)
    {
        continueBtn.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        displayName = nameInputField.text;
        PlayerPrefs.SetString(playerPrefsNameKey, displayName);
    }
}
