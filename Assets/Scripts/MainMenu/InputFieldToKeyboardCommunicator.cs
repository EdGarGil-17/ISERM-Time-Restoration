using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class InputFieldToKeyboardCommunicator : MonoBehaviour, IPointerClickHandler
{
    public MainMenuControl mainMenu;

    public void OnPointerClick(PointerEventData eventData)
    {

        // Prevent the virtual keyboard from showing up
        TouchScreenKeyboard.hideInput = true;


        // Activate your custom keyboard
        mainMenu.ShowKeyboard();

        // Assign the input field to the KeyboardManager
        KeyBoardManager.instance.inputField = GetComponent<TMP_InputField>();


    }
}
