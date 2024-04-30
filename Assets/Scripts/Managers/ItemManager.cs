using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ItemManager : MonoBehaviour    
{
    #region Singleton
    public static ItemManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializedDictionary("Name", "Object")]
    public SerializedDictionary<string, GameObject> objectList;
    public SerializedDictionary<string, int> objectMaxStackValues;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private GameObject GetObject(string key)
    {
        return objectList[key];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"> The Name of the Object to Spawn</param>
    /// <param name="spawnTransform">The Transform of the </param>
    public void SpawnObject(string key, Transform spawnTransform)
    {
        GameObject spawnedObject = Instantiate(GetObject(key)) as GameObject;

        spawnedObject.transform.position = spawnTransform.position;

        spawnedObject.name = key;
    }
}
