using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconGenerator : MonoBehaviour
{
    //Current object on the scene
    private GameObject objectOnScene;
    #region buttonBooleans
    private bool isPrefabCalled = false;
    private bool isSaveRequested = false;
    private bool isUpdateClicked = false;
    private bool isResetClicked = false;
    private bool isPreviousPrefabCalled = false;
    private bool isNextPrefabCalled = false;
    private bool isTransparentSaveClicked = false;
    #endregion

    private bool anyObjectOnScene = false;
    private int prefabIndex = 0;
    [Header("Object Background Panel")]
    public GameObject objectBackground;

    [Header("Area to be cropped from the screen")]
    public int startX;
    public int startY;
    public int width;
    public int height;

    #region assignmentOfGameObjects
    [Header("Put your objects into Resources/Prefabs")]
    public List<GameObject> objectList;
    [Header("The object on the scene")]
    public Transform currentObject;
    [Header("Displays debug logs on the screen")]
    public TextMeshProUGUI warningMessage;
    [Header("Transform panel")]
    public GameObject transformPanel;
    #endregion
    #region assignmentOfInputFields
    public TMP_InputField posX;
    public TMP_InputField posY;
    public TMP_InputField posZ;
    public TMP_InputField rotX;
    public TMP_InputField rotY;
    public TMP_InputField rotZ;
    public TMP_InputField scaX;
    public TMP_InputField scaY;
    public TMP_InputField scaZ;
    #endregion   
    
    /// <summary>
    /// Load prefabs into objectList
    /// Assigns input fields to the correct vectors
    /// </summary>
    private void Awake()
    {
        objectList = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs"));

        if (objectList.Count <= 0)
        {
            Debug.Log("Your prefab list is empty. Put your objects under /Resources/Prefabs folder.");
            StartCoroutine(ShowMessage("Your prefab list is null. Put your objects under /Resources/Prefabs folder."));
        }

        else
        {
            Debug.Log("Prefabs are loaded successfully.");
            StartCoroutine(ShowMessage("Prefabs are loaded successfully."));
        }
    }

    private void Update()
    {
        if (anyObjectOnScene)
        {
            transformPanel.SetActive(true);
        }
        if(objectList.Count > 0)
        {
            if (isPrefabCalled || Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Get Prefab");

                if (anyObjectOnScene)
                {
                    Debug.Log("Save this object first");
                    StartCoroutine(ShowMessage("Save this object first"));
                }

                if (objectList != null && !anyObjectOnScene)
                {
                    if (prefabIndex < objectList.Count)
                    {
                        objectOnScene = Instantiate(objectList[prefabIndex], currentObject, false);
                        anyObjectOnScene = true;
                    }

                    else
                    {
                        Debug.Log("You returned to the beginning of the list - after save");
                        StartCoroutine(ShowMessage("You returned to the beginning of the list - after save"));
                        prefabIndex = 0;
                    }
                }

                else if (objectList == null)
                {
                    Debug.Log("List is empty. Check Resources/Prefabs");
                    StartCoroutine(ShowMessage("List is empty. Check Resources/Prefabs"));
                }
                isPrefabCalled = false;
            }

            else if (isSaveRequested || Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Save Prefab");

                if (anyObjectOnScene)
                {
                    StartCoroutine(TakeScreenshot(objectList[prefabIndex]));
                    StartCoroutine(ShowMessage("You saved object's image. Next object's loaded."));
                }
                else
                {
                    StartCoroutine(ShowMessage("There is nothing to save."));
                }

                isSaveRequested = false;
            }

            else if (isTransparentSaveClicked || Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("Save Transparent Background Prefab");

                if (anyObjectOnScene)
                {
                    StartCoroutine(TakeTransparentScreenshot(objectList[prefabIndex]));
                    StartCoroutine(ShowMessage("You saved object's image. Background is transparent. Next object's loaded."));
                }
                else
                {
                    StartCoroutine(ShowMessage("There is nothing to save."));
                }

                isTransparentSaveClicked = false;
            }

            else if (isPreviousPrefabCalled || Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Get Previous Prefab");

                if (anyObjectOnScene)
                {
                    Destroy(objectOnScene);
                    anyObjectOnScene = false;

                    if (prefabIndex > 0)
                    {
                        prefabIndex--;

                        if (prefabIndex < objectList.Count)
                        {
                            objectOnScene = Instantiate(objectList[prefabIndex], currentObject, false);
                            anyObjectOnScene = true;
                        }

                        else
                        {
                            prefabIndex = 0;
                            Debug.Log("You returned to the beginning of the list - previous");
                            StartCoroutine(ShowMessage("You returned to the beginning of the list - previous"));
                        }
                    }

                    else
                    {
                        prefabIndex = 0;
                        objectOnScene = Instantiate(objectList[prefabIndex], currentObject, false);
                        anyObjectOnScene = true;
                        Debug.Log("You returned to the beginning of the list - previous");
                        StartCoroutine(ShowMessage("You returned to the beginning of the list - previous"));
                    }
                }

                else
                {
                    Debug.Log("There is no object for a previous");
                    StartCoroutine(ShowMessage("There is no object for a previous"));
                }

                isPreviousPrefabCalled = false;
            }

            else if (isNextPrefabCalled || Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("Get Next Prefab");

                if (anyObjectOnScene)
                {
                    Destroy(objectOnScene);
                    anyObjectOnScene = false;

                    if (prefabIndex >= 0)
                    {
                        prefabIndex++;

                        if (prefabIndex < objectList.Count)
                        {
                            objectOnScene = Instantiate(objectList[prefabIndex], currentObject, false);
                            anyObjectOnScene = true;
                        }

                        else
                        {
                            prefabIndex = 0;
                            Debug.Log("You returned to the beginning of the list - next");
                            StartCoroutine(ShowMessage("You returned to the beginning of the list - next"));
                        }
                    }

                    else if (prefabIndex < 0)
                    {
                        prefabIndex = 0;
                        objectOnScene = Instantiate(objectList[prefabIndex], currentObject, false);
                        anyObjectOnScene = true;
                        Debug.Log("You returned to the beginning of the list - next");
                        StartCoroutine(ShowMessage("You returned to the beginning of the list - next"));
                    }
                }

                else
                {
                    Debug.Log("There is no object for a next");
                    StartCoroutine(ShowMessage("There is no object for a next"));
                }

                isNextPrefabCalled = false;
            }

            else if (isUpdateClicked || Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log("Transform is Updated");

                if (posX.text == "" || posY.text == "" || posZ.text == "" || rotX.text == "" || rotY.text == "" || rotZ.text == "" || scaX.text == "" || scaY.text == "" || scaZ.text == "")
                {
                    Debug.Log("Fill input fields");
                    StartCoroutine(ShowMessage("Fill input fields"));
                }

                else
                {
                    Vector3 position = new Vector3(float.Parse(posX.text), float.Parse(posY.text), float.Parse(posZ.text));
                    Quaternion rotation = new Quaternion(float.Parse(rotX.text), float.Parse(rotY.text), float.Parse(rotZ.text), 1);
                    Vector3 scale = new Vector3(float.Parse(scaX.text), float.Parse(scaY.text), float.Parse(scaZ.text));

                    currentObject.transform.position = position;
                    currentObject.transform.rotation = rotation;
                    currentObject.transform.localScale = scale;
                }

                isUpdateClicked = false;
            }

            else if (isResetClicked || Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Transform is set to initials.");

                if (anyObjectOnScene)
                {
                    currentObject.transform.position = Vector3.zero;
                    currentObject.rotation = Quaternion.identity;
                    currentObject.localScale = Vector3.one;

                    posX.text = currentObject.transform.position.x.ToString();
                    posY.text = currentObject.transform.position.y.ToString();
                    posZ.text = currentObject.transform.position.z.ToString();
                    rotX.text = currentObject.rotation.x.ToString();
                    rotY.text = currentObject.rotation.y.ToString();
                    rotZ.text = currentObject.rotation.z.ToString();
                    scaX.text = currentObject.localScale.x.ToString();
                    scaY.text = currentObject.localScale.y.ToString();
                    scaZ.text = currentObject.localScale.z.ToString();
                }

                else
                {
                    StartCoroutine(ShowMessage("There is no object on the scene"));
                    Debug.Log("There is no object on the scene");
                }

                isResetClicked = false;
            }
        }
    }

    /// <summary>
    /// Takes screenshot of the screen area specified in the Inspector window.
    /// For 1600x900 screen resolution: x: 546, y: 196, w: 512, h: 512
    /// </summary>
    /// <param name="sceneObject"> the prefab that is currently on the scene</param>
    /// <returns></returns>
    IEnumerator TakeScreenshot(GameObject sceneObject)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        Color[] pixels = texture.GetPixels(startX, startY, width, height);
        Texture2D tex = new Texture2D(width, height);

        tex.SetPixels(pixels);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        File.WriteAllBytes(Application.dataPath + "/IconStudio/CreatedIcons/"+ sceneObject.name +".png", bytes);
        Destroy(objectOnScene);
        prefabIndex++;
        anyObjectOnScene = false;
        isPrefabCalled = true;
    }

    /// <summary>
    /// Takes screenshot of the screen area specified in the Inspector window.
    /// Clears the color you want to delete. That's why please pick a color which is not in your prefab.
    /// For 1600x900 screen resolution: x: 546, y: 196, w: 512, h: 512
    /// </summary>
    /// <param name="sceneObject"> the prefab that is currently on the scene</param>
    /// <returns></returns>
    IEnumerator TakeTransparentScreenshot(GameObject sceneObject)
    {
        yield return new WaitForEndOfFrame();

        if(objectBackground.GetComponent<Image>().sprite != null)
        {
            Debug.Log("Please pick a solid color.");
            StartCoroutine(ShowMessage("Please pick a solid color."));
        }
        else
        {
            Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

            Color[] pixels = texture.GetPixels(startX, startY, width, height);
            Texture2D tex = new Texture2D(width, height);

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i] == objectBackground.GetComponent<Image>().color)
                {
                    pixels[i] = new Color(0,0,0,0);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            Destroy(tex);

            File.WriteAllBytes(Application.dataPath + "/IconStudio/CreatedTransparentIcons/" + sceneObject.name + ".png", bytes);

            Destroy(objectOnScene);
            prefabIndex++;

            anyObjectOnScene = false;
            isPrefabCalled = true;
        }
    }

    #region interfaceButtonClickControl
    public void OnGetPrefabClick()
    {
        isPrefabCalled = true;
    }

    public void OnPrefabSaveClick()
    {
        isSaveRequested = true;
    }

    public void OnPreviousPrefabClick()
    {
        isPreviousPrefabCalled = true;
    }

    public void OnUpdateClick()
    {
        isUpdateClicked = true;
    }

    public void OnResetClick()
    {
        isResetClicked = true;
    }

    public void OnNextPrefabCall()
    {
        isNextPrefabCalled = true;
    }

    public void OnTransparentSaveClick()
    {
        isTransparentSaveClicked = true;
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
