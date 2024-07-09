using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOjectPool : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] int _objectPoolSize = 30;
    GameObject[] _bulletPool;
    void Start()
    {
        _bulletPool = new GameObject[_objectPoolSize];
        for (int i = 0; i < _objectPoolSize; i++)
        {
            GameObject newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            newBullet.transform.parent = transform;
            _bulletPool[i] = newBullet;
            newBullet.name = $"Bullet {i}";
            newBullet.SetActive(false);
        }
    }

    public GameObject[] GetBulletPool()
    {
        return _bulletPool;
    }

    public GameObject GetInactiveBulletFromPool()
    {
        for (int i = 0; i < _bulletPool.Length; i++)
        {
            if (!_bulletPool[i].activeInHierarchy)
            {
                return _bulletPool[i];
            }
        }
        return null;
    }
}
