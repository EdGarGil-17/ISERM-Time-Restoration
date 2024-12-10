using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    //UI Variables
    public GameObject menuPage;
    public GameObject settingsPage;
    public TMP_InputField userIDInputField;
    private string currentUserID = "";
    public GameObject keyboard;
    public GameObject errorText;

    //Player Prefs KEYS
    private const string ScrollSpeedKey = "_ScrollSpeed";
    private const string StimulusThicknessKey = "_StimulusThickness";
    private const string StimulusDistanceKey = "_StimulusDistance";
    private const string StimulusPositionKey = "_StimulusPosition";
    private const string StimulusFrequencyKey = "_StimulusFrequency";
    private const string StimulusAlphaKey = "_StimulusTransparency";

    //Stimulus Speed vairables
    public TMP_Text speedText; //Display for the speed value
    private float scrollSpeed;
    private const int MinSpeed = 1;
    private const int MaxSpeed = 10;
    public Button increaseSpeedButton;

    //Stimulus Thickness Variables
    public TMP_Text thicknessText; //Display for the speed value
    private int stimThickness;
    private const int MinThick = 1;
    private const int MaxThick = 10;

    //Stimulus Distance Variables
    public Slider distanceSlider; //Reference to the slider
    public TMP_Text valueDistanceText; //To show the slider value

    //Stimulus Possiiton Variables
    public Slider possSlider; //Reference to the slider
    public TMP_Text valueDistancePossText; //To show the slider value

    //Stimulus Possiiton Variables
    public Slider alphaSlider; //Reference to the slider
    public TMP_Text valueAlphaText; //To show the slider value

    //Stimulus Frequency
    public TMP_Dropdown frequencyDropdown;

    public void Awake()
    {
        
    }

    private void Start()
    {

        if (Singleton.Instance.gameStarted == true)
        {
            userIDInputField.text = Singleton.Instance.userID;
        }
        else
        {
            //Start with default settings and empty inout field
            ApplyDefaultSettings();
            userIDInputField.text = "";
        }

        menuPage.SetActive(true);
        settingsPage.SetActive(false);

        // Add listeners for sliders and dropdowns
        distanceSlider.onValueChanged.AddListener(OnSliderValueChanged);
        possSlider.onValueChanged.AddListener(OnPossSliderValueChanged);
        alphaSlider.onValueChanged.AddListener(OnAlphaSliderValueChanged);
        frequencyDropdown.onValueChanged.AddListener(OnFrequencyChanged);

        /**
        //Load the saved speed or use a default value
        scrollSpeed = PlayerPrefs.GetFloat(userIDInputField.text + ScrollSpeedKey, 5f); //Default is 5
        stimThickness = PlayerPrefs.GetInt(userIDInputField.text + StimulusThicknessKey, 5); //Default is 5
        float savedValue = PlayerPrefs.GetFloat(userIDInputField.text + StimulusDistanceKey, 1.0f); //Default is 1.0
        distanceSlider.value = savedValue;
        float savedPossValue = PlayerPrefs.GetFloat(userIDInputField.text + StimulusPositionKey, 0.0f); //Default is 1.0
        possSlider.value = savedPossValue;
        float savedAlphaValue = PlayerPrefs.GetFloat(userIDInputField.text + StimulusAlphaKey, 1f); //Default is 0
        alphaSlider.value = savedAlphaValue;
        int savedFrequency = PlayerPrefs.GetInt(userIDInputField.text + StimulusFrequencyKey, 0); //Default to 0 (Low)
        frequencyDropdown.value = savedFrequency;

        UpdateSpeedText();
        UpdateThicknessText();
        UpdateValueDisplayText(savedValue);
        // Add a listener to update the value as the slider moves
        distanceSlider.onValueChanged.AddListener(OnSliderValueChanged);
        UpdatePossValueDisplayText(savedPossValue);
        // Add a listener to update the value as the slider moves
        possSlider.onValueChanged.AddListener(OnPossSliderValueChanged);
        UpdateAlphaValueDisplayText(savedAlphaValue);
        // Add a listener to update the value as the slider moves
        alphaSlider.onValueChanged.AddListener(OnAlphaSliderValueChanged);
        // Add listener to save the selected value when the dropdown is changed
        frequencyDropdown.onValueChanged.AddListener(OnFrequencyChanged);
        **/

        // Clear existing listeners to avoid duplicates
        //increaseSpeedButton.onClick.RemoveAllListeners();
        //decreaseSpeedButton.onClick.RemoveAllListeners();

        // Assign button listeners
        //increaseSpeedButton.onClick.AddListener(IncreaseSpeed);
        //decreaseSpeedButton.onClick.AddListener(DecreaseSpeed);
    }

    #region ScrollSpeed

    public void IncreaseSpeed()
    {
        if (scrollSpeed < MaxSpeed)
        {
            Debug.Log("Increase button pressed");
            scrollSpeed += 1;
            SaveSpeed();
            UpdateSpeedText();
        }
    }

    public void DecreaseSpeed()
    {
        if (scrollSpeed > MinSpeed)
        {
            scrollSpeed -= 1;
            SaveSpeed();
            UpdateSpeedText();
        }
    }

    private void SaveSpeed()
    {
        if (!string.IsNullOrEmpty(currentUserID))
        {
            PlayerPrefs.SetFloat(currentUserID + ScrollSpeedKey, scrollSpeed);
            PlayerPrefs.Save();
        }
    }

    private void UpdateSpeedText()
    {
        speedText.text = scrollSpeed.ToString(); //Display speed with 1 decimal
    }

    #endregion

    #region StimulusThickness

    public void IncreaseThickness()
    {
        if (stimThickness < MaxThick)
        {
            stimThickness += 1;
            SaveThickness();
            UpdateThicknessText();
        }
    }

    public void DecreaseThickness()
    {
        if (stimThickness > MinThick)
        {
            stimThickness -= 1;
            SaveThickness();
            UpdateThicknessText();
        }
    }

    private void SaveThickness()
    {
        if (!string.IsNullOrEmpty(currentUserID))
        {
            PlayerPrefs.SetInt(currentUserID + StimulusThicknessKey, stimThickness);
            PlayerPrefs.Save();
        }
    }

    private void UpdateThicknessText()
    {
        thicknessText.text = stimThickness.ToString(); //Display speed with 1 decimal
    }

    #endregion

    #region StimulusDistance

    private void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(currentUserID + StimulusDistanceKey, value);
        PlayerPrefs.Save();
        UpdateValueDisplayText(value);
    }

    private void UpdateValueDisplayText(float value)
    {
        valueDistanceText.text = $"{value:F1}"; // Format to one decimal place
    }

    #endregion

    #region StimulusPosition

    private void OnPossSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(currentUserID + StimulusPositionKey, value);
        PlayerPrefs.Save();
        UpdatePossValueDisplayText(value);
    }

    private void UpdatePossValueDisplayText(float value)
    {
        valueDistancePossText.text = $"{value:F1}"; // Format to one decimal place
    }

    #endregion

    #region StimulusFrequency

    private void OnFrequencyChanged(int value)
    {
        PlayerPrefs.SetInt(currentUserID + StimulusFrequencyKey, value);
        PlayerPrefs.Save();
    }

    #endregion

    #region StimulusAlpha

    private void OnAlphaSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(currentUserID + StimulusAlphaKey, value);
        PlayerPrefs.Save();
        UpdateAlphaValueDisplayText(value);
    }

    private void UpdateAlphaValueDisplayText(float value)
    {
        valueAlphaText.text = value.ToString(); // Format to one decimal place
    }

    #endregion

    public void StartButton()
    {
        //Check if the user input field is not null and has valid input
        if (!string.IsNullOrWhiteSpace(userIDInputField?.text))
        {
            UpdateUserID(userIDInputField.text);
            Singleton.Instance.userID = currentUserID;
            Debug.Log("This is the stored ID: " + Singleton.Instance.userID);
            Singleton.Instance.gameStarted = true;
            Singleton.CSVManager.CreateReport();
            errorText.SetActive(false);
            //Load the next scene
            SceneManager.LoadScene(1);
        }
        else
        {
            errorText.SetActive(true);
        }
    }

    public void SettingsButton()
    {
        //Check if the user input field is not null and has valid input
        if (!string.IsNullOrWhiteSpace(userIDInputField?.text))
        {
            UpdateUserID(userIDInputField.text);
            errorText.SetActive(false);
            menuPage.SetActive(false);
            settingsPage.SetActive(true);
        }
        else
        {
            errorText.SetActive(true);
        } 
    }

    public void ConfirmButton()
    {
        menuPage.SetActive(true);
        settingsPage.SetActive(false);
    }

    private void ApplyDefaultSettings()
    {
        scrollSpeed = 5f;
        stimThickness = 5;
        distanceSlider.value = 1.0f;
        possSlider.value = 0.0f;
        alphaSlider.value = 100f;
        frequencyDropdown.value = 0;

        //Update UI
        UpdateSpeedText();
        UpdateThicknessText();
        UpdateValueDisplayText(distanceSlider.value);
        UpdatePossValueDisplayText(possSlider.value);
        UpdateAlphaValueDisplayText(alphaSlider.value);
    }

    public void UpdateUserID(string newID)
    {
        if (!string.IsNullOrEmpty(newID))
        {
            currentUserID = newID;

            //Load settings tied to this user ID
            scrollSpeed = PlayerPrefs.GetFloat(currentUserID + ScrollSpeedKey, 5f);
            stimThickness = PlayerPrefs.GetInt(currentUserID + StimulusThicknessKey, 5);
            distanceSlider.value = PlayerPrefs.GetFloat(currentUserID + StimulusDistanceKey, 1.0f);
            possSlider.value = PlayerPrefs.GetFloat(currentUserID + StimulusPositionKey, 0.0f);
            alphaSlider.value = PlayerPrefs.GetFloat(currentUserID + StimulusAlphaKey, 1f);
            frequencyDropdown.value = PlayerPrefs.GetInt(currentUserID + StimulusFrequencyKey, 0);

            //Update UI
            UpdateSpeedText();
            UpdateThicknessText();
            UpdateValueDisplayText(distanceSlider.value);
            UpdatePossValueDisplayText(possSlider.value);
            UpdateAlphaValueDisplayText(alphaSlider.value);
        }
        else
        {            
            ApplyDefaultSettings();
        }
    }

    public void ShowKeyboard()
    {
        keyboard.SetActive(true);
    }

    public void HideKeyboard()
    {
        keyboard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
