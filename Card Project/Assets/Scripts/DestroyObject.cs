using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public int DestroyTime = 2;

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }
    IEnumerator AutoDestroy() {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);
    }
}
