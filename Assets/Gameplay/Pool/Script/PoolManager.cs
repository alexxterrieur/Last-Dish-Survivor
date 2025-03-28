using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance;

    public Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    public Dictionary<string, GameObject> poolPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CreatePool(string key, GameObject prefab, int poolSize)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new Queue<GameObject>();
            poolPrefabs[key] = prefab;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name + "(Clone)";
                obj.SetActive(false);
                pools[key].Enqueue(obj);
            }

            Debug.Log($"[POOL] Pool créée pour {key} avec {poolSize} objets.");
        }
        else
        {
            Debug.LogWarning($"[POOL] La pool {key} existe déjà.");
        }
    }

    public GameObject GetFromPool(string poolKey, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(poolKey))
        {
            Debug.LogError($"Le pool {poolKey} n'existe pas !");
            return null;
        }

        GameObject obj;

        if (pools[poolKey].Count == 0)
        {
            Debug.LogWarning($"Pool {poolKey} vide ! Création de nouveaux objets.");
            if (poolPrefabs.ContainsKey(poolKey))
            {
                obj = Instantiate(poolPrefabs[poolKey]);
                obj.name = poolPrefabs[poolKey].name + "(Clone)";
                pools[poolKey].Enqueue(obj);
            }
            else
            {
                Debug.LogError($"Prefab pour {poolKey} introuvable !");
                return null;
            }
        }
        else
        {
            obj = pools[poolKey].Dequeue();
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
            pools[key] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}
