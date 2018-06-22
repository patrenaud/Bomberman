using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(AutoDestroyer());
    }

    private IEnumerator AutoDestroyer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}