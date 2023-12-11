using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    private Renderer m_Renderer;
    private BoxCollider m_Collider;

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DissAppearPlatform());
    }

    IEnumerator DissAppearPlatform()
    {
        yield return new WaitForSeconds(4);
        m_Renderer.enabled = false;
        m_Collider.enabled = false;
        StartCoroutine(ReAppearPlatform());
    }

    IEnumerator ReAppearPlatform()
    {
        yield return new WaitForSeconds(3);
        m_Renderer.enabled = true;
        m_Collider.enabled = true;
    }
}
