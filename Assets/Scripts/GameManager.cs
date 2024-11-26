using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Player Prefs KEYS
    private const string ThicknessSettingsKey = "StimulusThickness";
    private const string DistanceSettingsKey = "StimulusDistance";
    private const string StimulusPositionSettingsKey = "StimulusPosition";
    private const string StimulusFrequencySettingsKey = "StimulusFrequency";
    private const string StimulusAlphaSettingsKey = "StimulusTransparency";

    private int stimThickness;
    private float stimDistance;
    private float stimPoss;
    private float stimAlpha;
    private int stimFreq;

    public GameObject rightStimulus;
    public GameObject leftStimulus;
    public GameObject stimulusHolder;

    public Texture2D lowFreq;
    public Texture2D midFreq;
    public Texture2D highFreq;

    private Renderer rightRenderer;
    private Renderer leftRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rightStimulus.SetActive(true);
        leftStimulus.SetActive(true);

        //Load the saved PlayerPrefs
        stimThickness = PlayerPrefs.GetInt(ThicknessSettingsKey, 5);
        stimDistance = PlayerPrefs.GetFloat(DistanceSettingsKey, 5f);
        stimPoss = PlayerPrefs.GetFloat(StimulusPositionSettingsKey, 0f);
        stimAlpha = PlayerPrefs.GetFloat(StimulusAlphaSettingsKey, 255);
        stimFreq = PlayerPrefs.GetInt(StimulusFrequencySettingsKey, 1);
        
        // Get the Renderers for the stimulus objects
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
        //Check if the B button is pressed
        if (OVRInput.GetDown(OVRInput.Button.Two)) //"Two" refers to the B button on the right controller
        {
            Debug.Log("B button pressed");
            Application.Quit();
        }

        //Check if A button and Index Trigger are pressed together
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A button and Index Trigger pressed together");
            SceneManager.LoadScene(0);
        }
    }
}
