using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour, iPanel
{
    public InputField caseNumberInput;

    private void Start()
    {
        UIManager.Instance.activeCase = new Case();
    }

    public void ProcessInfo()
    {
        if (!string.IsNullOrEmpty(caseNumberInput.text) && int.TryParse(caseNumberInput.text, out int outResult))
        {
            AWSManager.Instance.FindCaseInAWSAsync(outResult);
        }
        else
        {
            Debug.Log("Please enter a case number to search for");
            return;
        }
    }

}
