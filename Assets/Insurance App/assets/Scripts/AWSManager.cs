using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class AWSManager : MonoBehaviour
{
    public static AWSManager Instance { get; private set; }

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

    AmazonS3Client S3Client;  //  IAmazonS3

    private void Start()
    {
        System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        //AWSConfigs.RegionEndpoint = RegionEndpoint.USEast2;
        //AWSConfigsS3.UseSignatureVersion4 = true;
        //AWSConfigs.CorrectForClockSkew = true;

        // Initialize the Amazon Cognito credentials provider
        CognitoAWSCredentials credentials = new CognitoAWSCredentials(
            "us-east-2:606e9a0a-5eb6-402d-a840-a53e5c1dd364", // Identity pool ID
            RegionEndpoint.USEast2 // Region
        );

        S3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

        //lIST ALL AWS BUCKETS
        S3Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {
            if (responseObject.Exception == null)
            {
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    Debug.Log("******AWS Bucket Name LIST: " + s3b.BucketName);
                });
            }
            else
            {
                Debug.Log("*****AWS LIST Exception_JJ: " + responseObject.Exception + "\n****************************");
            }
        });
        //LIST ALL AWS BUCKETS */
        
    }


    public void SaveCase()   //stream data in prep to upload
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + Path.DirectorySeparatorChar + "case" + UIManager.Instance.activeCase.caseID + ".dat";

        using (FileStream file = File.Create(path))    // ;
        {
            try
            {
                bf.Serialize(file, UIManager.Instance.activeCase);
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                Debug.Log("*****  Failed to serialize. Reason: " + e.Message);
                //throw;
            }
            finally
            {
                Debug.Log("***** Serialization complete at: " + path + " ********");
                file.Close();
            }
        };
        

        int caseID = int.Parse(UIManager.Instance.activeCase.caseID);
        PostObjectToAWS(path);
    }


    void PostObjectToAWS(string path)
    {
        //FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        FileStream stream = File.OpenRead(path);
        
        string bucketValue = "jaslowserviceappcasefiles";
        string keyValue = "case" + UIManager.Instance.activeCase.caseID + ".dat";

        PostObjectRequest request = new PostObjectRequest()
        {
            Bucket = bucketValue,
            Key = keyValue,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = RegionEndpoint.USEast2
        };

        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("\nAWS object " + responseObj.Request.Key + " posted to bucket " + responseObj.Request.Bucket);
                stream.Close();
                File.Delete(path);
                stream.Dispose();
            }
            else
            {
                Debug.Log("****** AWS post exception!::\n");
                Debug.Log(responseObj.Exception.ToString());
                stream.Close();
                File.Delete(path);
                stream.Dispose();
            }
        });

    }



    ////////////////////
    IEnumerator PostObjectToAWSxxx(FileStream file)
    {
        yield return new WaitForSeconds(5);

        var request = new PostObjectRequest()
        {
            Bucket = "jaslowserviceappcasefiles",
            Key = "case" + UIManager.Instance.activeCase.caseID + ".dat",
            InputStream = file,
            CannedACL = S3CannedACL.Private,
            Region = RegionEndpoint.USEast2
        };


        S3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                Debug.Log("\nAWS object " + responseObj.Request.Key + " posted to bucket " + responseObj.Request.Bucket);
                file.Close();
            }
            else
            {
                Debug.Log("AWS post exception!::\n");
                Debug.Log(responseObj.Exception);
                file.Close();
            }
        });

        //yield return new WaitForSeconds(5);
        //SceneManager.LoadScene(0);
    }

}