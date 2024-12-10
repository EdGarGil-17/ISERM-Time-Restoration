using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardManager : MonoBehaviour
{
    public static KeyBoardManager instance;
    public Button shiftButton;
    public Button spaceButton;
    public Button deleteButton;
    private Image shiftButtonimage;
    public TMP_InputField inputField;


    private bool isShifted = false;

    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }

        spaceButton.onClick.AddListener(Space);
        deleteButton.onClick.AddListener(Delete);
        shiftButton.onClick.AddListener(Shifted);
        shiftButtonimage = shiftButton.gameObject.GetComponent<Image>();
    }



    private void Space()
    {
        inputField.text += " ";
    }

    private void Delete()
    {
        int length = inputField.text.Length - 1;
        inputField.text = inputField.text.Substring(0, length);
    }

    private void Shifted()
    {
        isShifted = !isShifted;

        if (isShifted)
        {
            shiftButtonimage.color = Color.yellow;
        }
        else
        {
            shiftButtonimage.color = Color.white;
        }
    }
}
