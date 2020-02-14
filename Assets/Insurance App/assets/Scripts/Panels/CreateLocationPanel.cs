using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateLocationPanel : MonoBehaviour, iPanel
{
    public Text caseNumberText;
    public RawImage mapImage;
    public InputField notesText;
    double xCoord = 1.3147;
    double yCoord = 103.8454;
    WWW www;
    bool canContinue = false;

    private void OnEnable()
    {
        caseNumberText.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;

        if (Input.location.isEnabledByUser)
        {
            Debug.Log("GPS IS enabled");
            StartCoroutine(getGPS());
            return;
        }
        //else
        //{
        //    Debug.LogError("GPS not enabled");
        //    StartCoroutine(DownloadMap());
        //}

        StartCoroutine(DownloadMap());

    }

    public void ProcessInfo()
    {
        if (!canContinue)
            return;

        UIManager.Instance.activeCase.locationNotes = notesText.text;
        UIManager.Instance.activeCase.location = xCoord + ", " + yCoord;
        UIManager.Instance.activeCase.date = DateTime.Today.ToString("d");

        UIManager.Instance.createPhotoPanel.SetActive(true);
    }

    IEnumerator DownloadMap()
    {
        //https://maps.googleapis.com/maps/api/staticmap?center=1.290270,103.851959&zoom=19&size=700x400&key=AIzaSyCC454iRbht9C3yz7OxD78VWynSrdcqRwI

        string baseURL = "https://maps.googleapis.com/maps/api/staticmap?center=";
        string zoomAndSize = "&zoom=19&size=700x400&key=";
        string apiKey = "AIzaSyCC454iRbht9C3yz7OxD78VWynSrdcqRwI";

        string finalURL = baseURL + xCoord + "," + yCoord + zoomAndSize + apiKey;
        Debug.Log("GPS Map URL: " + finalURL);

        www = new WWW(finalURL);
        yield return www;
        Debug.Log("GPS Map returned");
        mapImage.texture = www.texture;

        canContinue = true;
    }

    
    IEnumerator getGPS()
    {
        // Start service before querying location
        Debug.Log("GPS enabled and starting");
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.LogError("GPS Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("GPS Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            xCoord = Input.location.lastData.latitude;
            yCoord = Input.location.lastData.longitude;
            Debug.Log("GPS Location: " + xCoord + " " + yCoord + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            StartCoroutine(DownloadMap());

        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();


        //yield return StartCoroutine(DownloadMap(xCoord, yCoord));

    }



}
