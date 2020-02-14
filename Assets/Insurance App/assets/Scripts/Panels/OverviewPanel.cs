using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewPanel : MonoBehaviour, iPanel
{
    public Text caseNumberText;
    public Text nameText;
    public Text dateText;
    public Text locationText;
    public Text locationNotesText;
    public Image image;
    public Text photoNotesText;

    private void OnEnable()
    {

        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
        nameText.text = "NAME: " + UIManager.Instance.activeCase.name;
        dateText.text = "DATE: " + UIManager.Instance.activeCase.date;
        locationText.text = "LOCATION:\n" + UIManager.Instance.activeCase.location;
        locationNotesText.text = "LOCATION NOTES:\n" + UIManager.Instance.activeCase.locationNotes;
        photoNotesText.text = "PHOTO NOTES:\n" + UIManager.Instance.activeCase.photoNotes;


        Texture2D tex = new Texture2D(2,2, TextureFormat.RGBA32, false);
        tex.LoadImage(UIManager.Instance.activeCase.photo);
        Sprite sp = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        image.sprite = sp;
    }

    public void ProcessInfo()
    {
        UIManager.Instance.ClearOutInputFields();

        //close all panels
        UIManager.Instance.borderPanel.SetActive(false);

        UIManager.Instance.searchPanel.SetActive(false);
        UIManager.Instance.searchSelectPanel.SetActive(false);
        UIManager.Instance.searchOverviewPanel.SetActive(false);

        UIManager.Instance.createNamePanel.SetActive(false);
        UIManager.Instance.createLocationPanel.SetActive(false);
        UIManager.Instance.createPhotoPanel.SetActive(false);
        UIManager.Instance.createOverviewPanel.SetActive(false);


    }
}

