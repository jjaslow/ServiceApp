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

#if UNITY_ANDROID
    public void UsedOnlyForAOTCodeGeneration()
    {
        //Bug reported on github https://github.com/aws/aws-sdk-net/issues/477
        //IL2CPP restrictions: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        //Inspired workaround: https://docs.unity3d.com/ScriptReference/AndroidJavaObject.Get.html

        AndroidJavaObject jo = new AndroidJavaObject("android.os.Message");
        int valueString = jo.Get<int>("what");
        string stringValue = jo.Get<string>("what");
    }
#endif

    AmazonS3Client S3Client;  //  IAmazonS3

    private void Start()
    {
        System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        // Initialize the Amazon Cognito credentials provider
        CognitoAWSCredentials credentials = new CognitoAWSCredentials(
            "us-east-2:606e9a0a-5eb6-402d-a840-a53e5c1dd364", // Identity pool ID
            RegionEndpoint.USEast2 // Region
        );

        S3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast2);

        /*/lIST ALL AWS BUCKETS
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

}