using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Panels
    public GameObject borderPanel;

    public GameObject searchPanel;
    public GameObject searchSelectPanel;
    public GameObject searchOverviewPanel;

    public GameObject createNamePanel;
    public GameObject createLocationPanel;
    public GameObject createPhotoPanel;
    public GameObject createOverviewPanel;
    #endregion

    public Case activeCase;

    public void CreateNewCase()
    {
        activeCase = new Case();

        int caseNum = Random.Range(0, 1000);
        activeCase.caseID = caseNum.ToString();

        borderPanel.SetActive(true);
        createNamePanel.SetActive(true);
    }




    public void ClearOutInputFields()
    {
        GameObject[] fields = GameObject.FindGameObjectsWithTag("InputButtons");
        
        foreach(GameObject f in fields)
        {
            InputField i = f.GetComponent<InputField>();
            i.text = "";

            Text j = f.transform.GetChild(1).GetComponent<Text>();
            j.text = "";
        }

        if(GameObject.FindGameObjectWithTag("CreatePhotoPanel") != null)
        {
            CreatePhotoPanel cpp = GameObject.FindGameObjectWithTag("CreatePhotoPanel").GetComponent<CreatePhotoPanel>();
            cpp.ResetCameraButtomImage();
        }
        

    }


}
