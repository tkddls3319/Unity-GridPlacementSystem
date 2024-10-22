using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> placeGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = GameObject.Instantiate(prefab);
        newObject.transform.position = position;
        placeGameObjects.Add(newObject);
        return placeGameObjects.Count - 1;
    }
}
