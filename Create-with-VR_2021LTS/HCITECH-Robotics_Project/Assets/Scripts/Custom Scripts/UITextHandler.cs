using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITextHandler : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] GameObject robot;
    [SerializeField] Button submitButton;
    [SerializeField] Button resetButton;
    private TextMeshProUGUI iText;
    private bool VERIFY_BUTTONS;
    private void Awake()
    {
        // Unity wouldn't let me get this in the inspector so I had
        // to refer to it here
        iText = text.GetComponent<TextMeshProUGUI>();
        VERIFY_BUTTONS = true;
    }
    private void Update()
    {
        if (VERIFY_BUTTONS) 
        { 
            if (!robot.GetComponent<RobotController>().moveReady || !robot.GetComponent<RobotController>().resetReady)
            {
                resetButton.interactable = false;
            }
            else
            {
                resetButton.interactable = true;
            }

            if (!robot.GetComponent<RobotController>().moveReady || !robot.GetComponent<RobotController>().submitReady)
            {
                submitButton.interactable = false;
            }
            else
            {
                submitButton.interactable = true;
            }
        }
    }

    public void UpdateStatus(bool finish, string status)
    {
        VERIFY_BUTTONS = finish;
        if (!finish)
        {
            iText.text = "move in progress\n"+status;
            iText.color = Color.red;
            iText.fontStyle = FontStyles.SmallCaps;
            resetButton.interactable = false;
            submitButton.interactable = false;
        }
        if (finish)
        { 
            iText.text = status+"\nready to interact";
            iText.color = Color.green;
            iText.fontStyle = FontStyles.SmallCaps;
        }
    }
}
