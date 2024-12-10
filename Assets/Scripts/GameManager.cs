using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.IO;
using System.Net;
using UnityEngine.Networking;
using System;
using System.Text;

public class GameManager : MonoBehaviour
{
    //Player Prefs KEYS
    private const string SpeedSettingKey = "_ScrollSpeed";
    private const string ThicknessSettingsKey = "_StimulusThickness";
    private const string DistanceSettingsKey = "_StimulusDistance";
    private const string StimulusPositionSettingsKey = "_StimulusPosition";
    private const string StimulusFrequencySettingsKey = "_StimulusFrequency";
    private const string StimulusAlphaSettingsKey = "_StimulusTransparency";

    private int stimThickness;
    private float stimDistance;
    private float stimPoss;
    private float stimAlpha;
    private int stimFreq;
    public string currentUserID;
    public PlaneScroller planeScrollerScript;

    public GameObject rightStimulus;
    public GameObject leftStimulus;
    public GameObject stimulusHolder;
    public GameObject settingObjects;

    public Texture2D lowFreq;
    public Texture2D midFreq;
    public Texture2D highFreq;

    private Renderer rightRenderer;
    private Renderer leftRenderer;

    //Settings objects
    //Stimulus Speed vairables
    public TMP_Text speedText; //Display for the speed value
    private float scrollSpeed;
    private const int MinSpeed = 1;
    private const int MaxSpeed = 10;
    public Button increaseSpeedButton;

    //Stimulus Thickness Variables
    public TMP_Text thicknessText; //Display for the speed value
    private int stimSettingThickness;
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

    //Email variables
    public string json;

    string api_user = "apikey";
    string api_key = "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";

    //string fromEmail = "FROM_ADDRESS_HERE";
    //string toEmail = "TO_ADDRESS_HERE";
    string subject = "Re:Vision Report";
    string body = "Re:Vision Patient Reports.";
    string xsmtpapiJSON = "{\"category\":\"unity_game_email\"}";

    // Start is called before the first frame update
    void Start()
    {
        rightStimulus.SetActive(true);
        leftStimulus.SetActive(true);
        currentUserID = Singleton.Instance.userID;

        //Load the saved PlayerPrefs
        stimThickness = PlayerPrefs.GetInt(currentUserID + ThicknessSettingsKey, 5);
        stimDistance = PlayerPrefs.GetFloat(currentUserID + DistanceSettingsKey, 5f);
        stimPoss = PlayerPrefs.GetFloat(currentUserID + StimulusPositionSettingsKey, 0f);
        stimAlpha = PlayerPrefs.GetFloat(currentUserID + StimulusAlphaSettingsKey, 255);
        stimFreq = PlayerPrefs.GetInt(currentUserID + StimulusFrequencySettingsKey, 1);

        // Add listeners for sliders and dropdowns
        distanceSlider.onValueChanged.AddListener(OnSliderValueChanged);
        possSlider.onValueChanged.AddListener(OnPossSliderValueChanged);
        alphaSlider.onValueChanged.AddListener(OnAlphaSliderValueChanged);
        frequencyDropdown.onValueChanged.AddListener(OnFrequencyChanged);

        //Get the Renderers for the stimulus objects
        rightRenderer = rightStimulus.GetComponent<Renderer>();
        leftRenderer = leftStimulus.GetComponent<Renderer>();

        //Set Thickness for Right Stimulus
        Vector3 rightScale = rightStimulus.transform.localScale;
        rightScale.x = stimThickness / 10f; //Only modify the X value
        rightStimulus.transform.localScale = rightScale;

        //Set Distance for Right Stimulus
        Vector3 rightPosition = rightStimulus.transform.position;
        rightPosition.x = stimDistance; //Only modify the X value
        rightStimulus.transform.position = rightPosition;

        //Set Alpha for Right Stimulus
        Color rightColor = rightRenderer.material.color;

        if (stimAlpha == 0)
        {
            rightStimulus.SetActive(false);
        }
        else
        {
            rightColor.a = stimAlpha / 100;
        }

        rightRenderer.material.color = rightColor;

        //Set Thickness for Left Stimulus
        Vector3 leftScale = leftStimulus.transform.localScale;
        leftScale.x = stimThickness / 10f; //Only modify the X value
        leftStimulus.transform.localScale = leftScale;

        //Set Distance for Left Stimulus
        Vector3 leftPosition = leftStimulus.transform.position;
        leftPosition.x = -stimDistance; //Only modify the X value
        leftStimulus.transform.position = leftPosition;

        //Set Alpha for Left Stimulus
        Color leftColor = leftRenderer.material.color;
        if (stimAlpha == 0)
        {
            leftStimulus.SetActive(false);
        }
        else
        {
            leftColor.a = stimAlpha / 100;
        }
        leftRenderer.material.color = leftColor;

        //Set Position for Stimulus Holder
        Vector3 holderPosition = stimulusHolder.transform.position;
        holderPosition.z = -3 - stimPoss; //Only modify the Z value
        stimulusHolder.transform.position = holderPosition;

        //Set Frequency (Texture) for Stimuli
        Debug.Log("freq: " + stimFreq);
        Texture2D selectedTexture = stimFreq switch
        {
            0 => lowFreq,
            1 => midFreq,
            2 => highFreq,
            _ => lowFreq, //Default to lowFreq if stimFreq is invalid
        };
        rightStimulus.GetComponent<Renderer>().material.mainTexture = selectedTexture;
        leftStimulus.GetComponent<Renderer>().material.mainTexture = selectedTexture;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyUserSettings();

        //Load the saved PlayerPrefs
        stimThickness = PlayerPrefs.GetInt(currentUserID + ThicknessSettingsKey, 5);
        stimDistance = PlayerPrefs.GetFloat(currentUserID + DistanceSettingsKey, 5f);
        stimPoss = PlayerPrefs.GetFloat(currentUserID + StimulusPositionSettingsKey, 0f);
        stimAlpha = PlayerPrefs.GetFloat(currentUserID + StimulusAlphaSettingsKey, 255);
        stimFreq = PlayerPrefs.GetInt(currentUserID + StimulusFrequencySettingsKey, 1);

        //Get the Renderers for the stimulus objects
        rightRenderer = rightStimulus.GetComponent<Renderer>();
        leftRenderer = leftStimulus.GetComponent<Renderer>();

        //Set Thickness for Right Stimulus
        Vector3 rightScale = rightStimulus.transform.localScale;
        rightScale.x = stimThickness / 10f; //Only modify the X value
        rightStimulus.transform.localScale = rightScale;

        //Set Distance for Right Stimulus
        Vector3 rightPosition = rightStimulus.transform.position;
        rightPosition.x = stimDistance; //Only modify the X value
        rightStimulus.transform.position = rightPosition;

        //Set Alpha for Right Stimulus
        Color rightColor = rightRenderer.material.color;

        if (stimAlpha == 0)
        {
            rightStimulus.SetActive(false);
        }
        else
        {
            rightStimulus.SetActive(true);
            rightColor.a = stimAlpha / 100;
        }

        rightRenderer.material.color = rightColor;

        //Set Thickness for Left Stimulus
        Vector3 leftScale = leftStimulus.transform.localScale;
        leftScale.x = stimThickness / 10f; //Only modify the X value
        leftStimulus.transform.localScale = leftScale;

        //Set Distance for Left Stimulus
        Vector3 leftPosition = leftStimulus.transform.position;
        leftPosition.x = -stimDistance; //Only modify the X value
        leftStimulus.transform.position = leftPosition;

        //Set Alpha for Left Stimulus
        Color leftColor = leftRenderer.material.color;
        if (stimAlpha == 0)
        {
            leftStimulus.SetActive(false);
        }
        else
        {
            leftStimulus.SetActive(true);
            leftColor.a = stimAlpha / 100;
        }
        leftRenderer.material.color = leftColor;

        //Set Position for Stimulus Holder
        Vector3 holderPosition = stimulusHolder.transform.position;
        holderPosition.z = -3 - stimPoss; //Only modify the Z value
        stimulusHolder.transform.position = holderPosition;

        //Set Frequency (Texture) for Stimuli
        Debug.Log("freq: " + stimFreq);
        Texture2D selectedTexture = stimFreq switch
        {
            0 => lowFreq,
            1 => midFreq,
            2 => highFreq,
            _ => lowFreq, //Default to lowFreq if stimFreq is invalid
        };
        rightStimulus.GetComponent<Renderer>().material.mainTexture = selectedTexture;
        leftStimulus.GetComponent<Renderer>().material.mainTexture = selectedTexture;

        //Check if the B button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two)) //"Two" refers to the B button on the right controller
        {
            Debug.Log("B button pressed");

            string frequencyTexture = stimFreq switch
            {
                0 => "Low Frequency",
                1 => "Medium Frequency",
                2 => "High Frequency",
                _ => "Low Frequency", //Default to lowFreq if stimFreq is invalid
            };

            Singleton.CSVManager.AppendToReport(
                                new string[7]
                                {
                                    currentUserID,
                                    scrollSpeed.ToString(),
                                    stimSettingThickness.ToString(),
                                    distanceSlider.value.ToString(),
                                    possSlider.value.ToString(),
                                    alphaSlider.value.ToString(),
                                    frequencyTexture
                                }
                                );
            SendEmail();
            
        }

        //Check if A button and Index Trigger are pressed together
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A button pressed");
            SceneManager.LoadScene(0);
        }

        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Debug.Log("X button pressed");
            settingObjects.SetActive(true);
        }
    }

    public void ConfirmBUtton()
    {
        settingObjects.SetActive(false);
    }

    public void ApplyUserSettings()
    {
        //Load settings tied to this user ID
        scrollSpeed = PlayerPrefs.GetFloat(currentUserID + SpeedSettingKey, 5f);
        stimSettingThickness = PlayerPrefs.GetInt(currentUserID + ThicknessSettingsKey, 5);
        distanceSlider.value = PlayerPrefs.GetFloat(currentUserID + DistanceSettingsKey, 1.0f);
        possSlider.value = PlayerPrefs.GetFloat(currentUserID + StimulusPositionSettingsKey, 0.0f);
        alphaSlider.value = PlayerPrefs.GetFloat(currentUserID + StimulusAlphaSettingsKey, 1f);
        frequencyDropdown.value = PlayerPrefs.GetInt(currentUserID + StimulusFrequencySettingsKey, 0);

        //Update UI
        UpdateSpeedText();
        UpdateThicknessText();
        UpdateValueDisplayText(distanceSlider.value);
        UpdatePossValueDisplayText(possSlider.value);
        UpdateAlphaValueDisplayText(alphaSlider.value);
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
            PlayerPrefs.SetFloat(currentUserID + SpeedSettingKey, scrollSpeed);
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
            stimSettingThickness += 1;
            SaveThickness();
            UpdateThicknessText();
        }
    }

    public void DecreaseThickness()
    {
        if (stimThickness > MinThick)
        {
            stimSettingThickness -= 1;
            SaveThickness();
            UpdateThicknessText();
        }
    }

    private void SaveThickness()
    {
        if (!string.IsNullOrEmpty(currentUserID))
        {
            PlayerPrefs.SetInt(currentUserID + ThicknessSettingsKey, stimSettingThickness);
            PlayerPrefs.Save();
        }
    }

    private void UpdateThicknessText()
    {
        thicknessText.text = stimSettingThickness.ToString(); //Display speed with 1 decimal
    }

    #endregion

    #region StimulusDistance

    private void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(currentUserID + DistanceSettingsKey, value);
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
        PlayerPrefs.SetFloat(currentUserID + StimulusPositionSettingsKey, value);
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
        PlayerPrefs.SetInt(currentUserID + StimulusFrequencySettingsKey, value);
        PlayerPrefs.Save();
    }

    #endregion

    #region StimulusAlpha

    private void OnAlphaSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(currentUserID + StimulusAlphaSettingsKey, value);
        PlayerPrefs.Save();
        UpdateAlphaValueDisplayText(value);
    }

    private void UpdateAlphaValueDisplayText(float value)
    {
        valueAlphaText.text = value.ToString(); // Format to one decimal place
    }

    #endregion

    public void SendEmail()
    {
        StartCoroutine(SendEmailPost());
    }

    
    private IEnumerator SendEmailPost()
    {
        byte[] file1Bytes = File.ReadAllBytes(Singleton.CSVManager.GetFilePath());
        string file1Content = Convert.ToBase64String(file1Bytes);

        //var json = "{\"personalizations\":[{\"to\":[{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"}],\"subject\":\"Re:Vision Report\"}],\"content\": [{\"type\": \"text / plain\", \"value\": \"Re:Vision Patient Reports.\"}],\"from\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"},\"reply_to\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"}, \"attachments\": [{\"content\": \"" + file1Content + "\", \"type\": \"text / plain\", \"filename\": \"Report.csv\"}, {\"content\": \"" + file2Content + "\", \"type\": \"text / plain\", \"filename\": \"QuestionareReport.csv\"}, {\"content\": \"" + file3Content + "\", \"type\": \"text / plain\", \"filename\": \"HeadMovement.csv\"}, {\"content\": \"" + file4Content + "\", \"type\": \"text / plain\", \"filename\": \"EyeMovement.csv\"}]}";
        //var json = "{\"personalizations\":[{\"to\":[{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"}, {\"email\":\"regarde.perleyh@gmail.com\",\"name\":\"Perley Health\"}],\"subject\":\"Re:Vision Report\"}],\"content\": [{\"type\": \"text / plain\", \"value\": \"Re:Vision Patient Reports.\"}],\"from\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"},\"reply_to\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"}, \"attachments\": [{\"content\": \"" + file1Content + "\", \"type\": \"text / plain\", \"filename\": \"Report.csv\"}, {\"content\": \"" + file2Content + "\", \"type\": \"text / plain\", \"filename\": \"QuestionareReport.csv\"}, {\"content\": \"" + file3Content + "\", \"type\": \"text / plain\", \"filename\": \"HeadMovement.csv\"}, {\"content\": \"" + file4Content + "\", \"type\": \"text / plain\", \"filename\": \"EyeMovement.csv\"}]}";

        DateTime currentDateTime = DateTime.Now;
        string formattedDateTime = currentDateTime.ToString("dd MMMM yyyy HH_mm_ss");

        json = "{\"personalizations\":[{\"to\":[{\"email\":\"strasburgpsychiatric@gmail.com\",\"name\":\"INSERM\"}],\"subject\":\"User Settings Report\"}],\"content\": [{\"type\": \"text / plain\", \"value\": \"Re:Vision Patient Reports.\"}],\"from\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"},\"reply_to\":{\"email\":\"m.reberlab@gmail.com\",\"name\":\"ReLab\"}, \"attachments\": [{\"content\": \"" + file1Content + "\", \"type\": \"text / plain\", \"filename\": \"" + Singleton.Instance.userID + " " + formattedDateTime + " Report.csv\"}]}";

        var bytes = Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("https://tls12.api.sendgrid.com/v3/mail/send", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bytes);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Authorization", string.Format("Bearer {0}", api_key));
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Email not sent");
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Email sent successfully!");
            }

            Application.Quit();
        }
    }

}
