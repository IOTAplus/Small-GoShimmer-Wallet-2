using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectFix : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(resetScrollPos());
    }

    IEnumerator resetScrollPos()
    {
        yield return null; // Waiting just one frame is probably good enough, yield return null does that
        gameObject.GetComponent<Scrollbar>().value = 1;
    }
}
