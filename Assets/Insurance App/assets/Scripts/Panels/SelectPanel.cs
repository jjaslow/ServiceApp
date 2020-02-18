using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour, iPanel
{
    public Text infoText;

    private void Start()
    {
        infoText.text = UIManager.Instance.activeCase.name + "\n" + UIManager.Instance.activeCase.date;
    }

    public void ProcessInfo()
    {
        UIManager.Instance.createOverviewPanel.SetActive(true);
    }

}
