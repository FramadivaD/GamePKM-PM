using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using Firebase;
using Firebase.Storage;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    public const string FirebaseBucketURL = "gs://pkmgamemath.appspot.com/";

    FirebaseStorage storage;
    StorageReference storageRef;

    public delegate void DownloadSuccessCallback(byte[] downloadedData);
    public delegate void DownloadFailedCallback();

    public delegate void FirebaseDownloadSuccessCallback(string downloadURL);
    public delegate void FirebaseDownloadFailedCallback();

    public delegate void FirebaseUploadSuccessCallback();
    public delegate void FirebaseUploadFailedCallback();

    private bool isInitialized = false;

    void Awake()
    {
        FirebaseInitialization();
    }

    private void FirebaseInitialization()
    {
        if (!isInitialized)
        {
            storage = FirebaseStorage.DefaultInstance;
            storageRef = storage.GetReferenceFromUrl(FirebaseBucketURL);

            isInitialized = true;
        }
    }

    public void GetDownloadUrl(string filename, FirebaseDownloadSuccessCallback onSuccess = null, FirebaseDownloadFailedCallback onFailed = null)
    {
        FirebaseInitialization();

        StorageReference data = storageRef.Child(filename);

        data.GetDownloadUrlAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Download data failed");
                onFailed?.Invoke();
            } else
            {
                Debug.Log("Download data success");
                onSuccess?.Invoke(Convert.ToString(task.Result));
            }
        });
    }

    public void UploadData(string filename, byte[] data, FirebaseUploadSuccessCallback onSuccess = null, FirebaseUploadFailedCallback onFailed = null)
    {
        FirebaseInitialization();

        StorageReference uploadRef = storageRef.Child(filename);
        uploadRef.PutBytesAsync(data).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log("Upload data failed");
                onFailed?.Invoke();
            }
            else
            {
                Debug.Log("Upload data success");
                onSuccess?.Invoke();
            }
        });
    }

    public void DownloadData(string filename, DownloadSuccessCallback onSuccess = null, DownloadFailedCallback onFailed = null)
    {
        GetDownloadUrl(filename, (string url) => {
            StartCoroutine(DownloadingData(url, onSuccess, onFailed));
        } ,null);
    }

    private IEnumerator DownloadingData(string url, DownloadSuccessCallback onSuccess = null, DownloadFailedCallback onFailed = null)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.data);
        } else
        {
            onFailed?.Invoke();
        }
    }
}
