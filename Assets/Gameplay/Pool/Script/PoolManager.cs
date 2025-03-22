using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set; }

    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string key, GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new Queue<GameObject>();
            prefabs[key] = prefab;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pools[key].Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError($"Aucun pool trouvé pour {key} !");
            return null;
        }

        GameObject obj;
        if (pools[key].Count > 0)
        {
            obj = pools[key].Dequeue();
        }
        else
        {
            obj = Instantiate(prefabs[key]);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            Debug.LogError($"Impossible de retourner {key} au pool !");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}
