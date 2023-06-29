using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextHandler : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] GameObject robot;
    [SerializeField] Button button;
    private TextMeshProUGUI iText;
    private void Awake()
    {
        // Unity wouldn't let me get this in the inspector so I had
        // to refer to it here
        iText = text.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        // Control how the interaction text looks by checking the 
        // current move status of the robot
        // if it is moving everything is deactivated
        if (!robot.GetComponent<RobotController>().moveReady)
        {
            iText.text = "not interactable";
            iText.color = Color.red;
            iText.fontStyle = FontStyles.SmallCaps;
            button.interactable = false;
        }
        else
        {
            iText.text = "interactable";
            iText.color = Color.green;
            iText.fontStyle = FontStyles.SmallCaps;
            button.interactable = true;
        }
        
    }
}
