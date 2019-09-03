using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public bool startDestroy = false;

    void Update()
    {
        if (startDestroy) DestroyGameObject();
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
