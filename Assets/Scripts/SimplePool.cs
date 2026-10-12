using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pre-creates a set of inactive objects from a prefab and hands them out on request.
/// Objects return to the pool by deactivating themselves (SetActive(false)),
/// so nothing is destroyed during gameplay.
/// </summary>
public class SimplePool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 15;

    private readonly List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    /// <summary>
    /// Returns an inactive object moved to the given position and switched on.
    /// Only creates a new object if every pooled object is already in use.
    /// </summary>
    public GameObject GetFromPool(Vector3 position)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab, position, Quaternion.identity, transform);
        pool.Add(newObj);
        return newObj;
    }
}
