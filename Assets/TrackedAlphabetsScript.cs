using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedAlphabetsScript : MonoBehaviour
{
    private ARTrackedImageManager _aRTrackedImageManager;

    [SerializeField]
    private GameObject[] myPlaceablePrefabs;

    private Dictionary<string, GameObject> mySpawnedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        _aRTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in myPlaceablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            mySpawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }
    private void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    private void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

     
    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            mySpawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = mySpawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach(GameObject spawned in mySpawnedPrefabs.Values)
        {
            if(spawned.name != name)
            {
                spawned.SetActive(false);
            }
        }
    }
}
