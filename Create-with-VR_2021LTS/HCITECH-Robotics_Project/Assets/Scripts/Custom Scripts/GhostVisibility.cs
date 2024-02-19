using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVisibility : MonoBehaviour
{
    [SerializeField] GameObject[] ghostModels;

    [SerializeField] GameObject[] models;
    public bool checkVisibility = false;

    // Start is called before the first frame update
    void Start()
    {
        // checking that everything is right in the unity side
        if (ghostModels.Length == models.Length)
        {
            for (int i = 0; i < ghostModels.Length; i++)
            {
                if (ghostModels[i].name == models[i].name)
                {
                    checkVisibility = true;
                }
                else
                {
                    checkVisibility = false;
                    Debug.Log("Names aren't the same");
                    break;
                }
            }
        } 
        else
        {
            checkVisibility = false;
            Debug.Log("Sizes aren't the same");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkVisibility)
        {
            for (int i = 0; i < ghostModels.Length; i++)
            {
                if (ghostModels[i].transform.position != models[i].transform.position &&
                    ghostModels[i].transform.rotation != models[i].transform.rotation)
                {
                    ghostModels[i].gameObject.SetActive(true);
                }
                else
                {
                    ghostModels[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
