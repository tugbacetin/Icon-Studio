using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script provides background image and color for your object
/// If you want different colors you can modify the region colors as you wish. 
/// </summary>
public class ChangeBackground : MonoBehaviour
{
    #region buttonBooleans
    private bool isColorChangeClicked = false;
    private bool isBackgroundChangeClicked = false;
    #endregion

    public Color backgroundColor;
    private int backgroundIndex = 0;

    #region gameObjectAssignments
    [Header("Object Background Panel")]
    public GameObject objectBackground;
    [Header("Resources/Backgrounds -> put your sprites in it")]
    public List<Sprite> backgroundList;
    [Header("Displays debug logs on the screen")]
    public TextMeshProUGUI warningMessage;
    #endregion

    public TMP_InputField R;
    public TMP_InputField G;
    public TMP_InputField B;

    /// <summary>
    /// Load colors and background images
    /// </summary>
    private void Awake()
    {
        backgroundList = new List<Sprite>(Resources.LoadAll<Sprite>("Backgrounds"));
        backgroundColor = new Color(0,0,0,1);
    }

    private void Update()
    {
        //Background color change randomly
        if (isColorChangeClicked || Input.GetKeyDown(KeyCode.C))
        {
            if(R.text != null || G.text != null || B.text != null)
            {
                backgroundColor = new Color(float.Parse(R.text), float.Parse(G.text), float.Parse(B.text), 1);
            }
            else
            {
                Debug.Log("Input missing RGB");
                StartCoroutine(ShowMessage("Input missing RGB"));
            }

            if (backgroundColor != null)
            {
                objectBackground.GetComponent<Image>().color = backgroundColor;
                objectBackground.GetComponent<Image>().sprite = null;
                Debug.Log("Color changed.");
                StartCoroutine(ShowMessage("Color changed."));
            }
            else
            {
                objectBackground.GetComponent<Image>().color = Color.clear;
            }

            isColorChangeClicked = false;
        }
        //Background image change randomly
        if (isBackgroundChangeClicked || Input.GetKeyDown(KeyCode.B))
        {
            if (backgroundList.Count > 0)
            {
                if(backgroundIndex < backgroundList.Count && backgroundIndex >= 0)
                {
                    objectBackground.GetComponent<Image>().color = Color.white;
                    objectBackground.GetComponent<Image>().sprite = backgroundList[backgroundIndex];
                    Debug.Log("Background changed.");
                    StartCoroutine(ShowMessage("Background changed."));
                    backgroundIndex++;
                }
                else if(backgroundIndex >= backgroundList.Count)
                {
                    backgroundIndex = 0;
                    Debug.Log("Beginning of background list.");
                    StartCoroutine(ShowMessage("Beginning of background list."));
                    objectBackground.GetComponent<Image>().color = Color.white;
                    objectBackground.GetComponent<Image>().sprite = backgroundList[backgroundIndex];
                    backgroundIndex++;
                }

            }
            else
            {
                objectBackground.GetComponent<Image>().sprite = null;
                objectBackground.GetComponent<Image>().color = Color.green;
                Debug.Log("There is no image in Resources/Backgrounds folder.");
                StartCoroutine(ShowMessage("There is no image in Resources/Backgrounds folder."));
            }

            isBackgroundChangeClicked = false;
        }
    }

    #region backgroudButtonClickCheck
    /// <summary>
    /// Change color randomly by unity colors
    /// </summary>
    public void OnColorChangeClick()
    {
        isColorChangeClicked = true;
    }

    /// <summary>
    /// Change background image. If there is no image color is green.
    /// </summary>
    public void OnBackgroundChangeClick()
    {
        isBackgroundChangeClicked = true;
    }
    #endregion

    IEnumerator ShowMessage(string message)
    {
        warningMessage.text = message;
        warningMessage.enabled = true;
        yield return new WaitForSeconds(2f);
        warningMessage.enabled = false;
    }
}