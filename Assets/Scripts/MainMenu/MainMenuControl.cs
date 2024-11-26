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

    //Player Prefs KEYS
    private const string ScrollSpeedKey = "ScrollSpeed";
    private const string StimulusThicknessKey = "StimulusThickness";
    private const string StimulusDistanceKey = "StimulusDistance";
    private const string StimulusPositionKey = "StimulusPosition";
    private const string StimulusFrequencyKey = "StimulusFrequency";
    private const string StimulusAlphaKey = "StimulusTransparency";

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

    private void Start()
    {
        menuPage.SetActive(true);
        settingsPage.SetActive(false);

        //Load the saved speed or use a default value
        scrollSpeed = PlayerPrefs.GetFloat(ScrollSpeedKey, 5f); //Default is 5
        stimThickness = PlayerPrefs.GetInt(StimulusThicknessKey, 5); //Default is 5
        float savedValue = PlayerPrefs.GetFloat(StimulusDistanceKey, 1.0f); //Default is 1.0
        distanceSlider.value = savedValue;
        float savedPossValue = PlayerPrefs.GetFloat(StimulusPositionKey, 0.0f); //Default is 1.0
        possSlider.value = savedPossValue;
        float savedAlphaValue = PlayerPrefs.GetFloat(StimulusAlphaKey, 1f); //Default is 0
        alphaSlider.value = savedAlphaValue;
        int savedFrequency = PlayerPrefs.GetInt(StimulusFrequencyKey, 0); //Default to 0 (Low)
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
        // Disable the button to prevent multiple inputs
        increaseSpeedButton.interactable = false;

        if (scrollSpeed < MaxSpeed)
        {
            Debug.Log("Increase button pressed");
            scrollSpeed += 1;
            SaveSpeed();
            UpdateSpeedText();
        }

        // Re-enable the button after processing
        increaseSpeedButton.interactable = true;
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
        PlayerPrefs.SetFloat(ScrollSpeedKey, scrollSpeed);
        PlayerPrefs.Save(); //Save to disk
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
        PlayerPrefs.SetInt(StimulusThicknessKey, stimThickness);
        PlayerPrefs.Save(); //Save to disk
    }

    private void UpdateThicknessText()
    {
        thicknessText.text = stimThickness.ToString(); //Display speed with 1 decimal
    }

    #endregion

    #region StimulusDistance

    private void OnSliderValueChanged(float value)
    {
        // Update the displayed value text
        UpdateValueDisplayText(value);

        // Save the value to PlayerPrefs
        PlayerPrefs.SetFloat(StimulusDistanceKey, value);
        PlayerPrefs.Save();
    }

    private void UpdateValueDisplayText(float value)
    {
        valueDistanceText.text = $"{value:F1}"; // Format to one decimal place
    }

    #endregion

    #region StimulusPosition

    private void OnPossSliderValueChanged(float value)
    {
        // Update the displayed value text
        UpdatePossValueDisplayText(value);

        // Save the value to PlayerPrefs
        PlayerPrefs.SetFloat(StimulusPositionKey, value);
        PlayerPrefs.Save();
    }

    private void UpdatePossValueDisplayText(float value)
    {
        valueDistancePossText.text = $"{value:F1}"; // Format to one decimal place
    }

    #endregion

    #region StimulusFrequency

    private void OnFrequencyChanged(int value)
    {
        // Save the selected frequency to PlayerPrefs
        PlayerPrefs.SetInt(StimulusFrequencyKey, value);
        PlayerPrefs.Save();

        // Debugging: Print the current selection
        Debug.Log("Selected Frequency: " + frequencyDropdown.options[value].text);
    }

    #endregion

    #region StimulusAlpha

    private void OnAlphaSliderValueChanged(float value)
    {
        // Update the displayed value text
        UpdateAlphaValueDisplayText(value);

        // Save the value to PlayerPrefs
        PlayerPrefs.SetFloat(StimulusAlphaKey, value);
        PlayerPrefs.Save();
    }

    private void UpdateAlphaValueDisplayText(float value)
    {
        valueAlphaText.text = value.ToString(); // Format to one decimal place
    }

    #endregion

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsButton()
    {
        menuPage.SetActive(false);
        settingsPage.SetActive(true);
    }

    public void ConfirmButton()
    {
        menuPage.SetActive(true);
        settingsPage.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        //Check if the B button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two)) //"Two" refers to the B button on the right controller
        {
            Debug.Log("B button pressed");
        }
    }
}
