using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CreateNamePanel : MonoBehaviour, iPanel
{
    public Text caseNumberText;
    public InputField firstName;
    public InputField lastName;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    private void Start()
    {

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

    }

    public void ProcessInfo()
    {

        if (firstName.text != "" && lastName.text != "")
        {
            //Debug.Log("not null");
            UIManager.Instance.activeCase.name = firstName.text + " " + lastName.text;
            UIManager.Instance.createLocationPanel.SetActive(true);
        }
        else
            Debug.Log("You need to enter first and last names to continue...");
    }
}
