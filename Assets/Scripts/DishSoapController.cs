using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishSoapController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D soapCollider;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (soapCollider == null)
        {
            soapCollider = GetComponent<BoxCollider2D>();
        }

        if(player == null)
        {
            Debug.LogWarning("Plauyer not found!");
        }
    }

    public void DisableSoapSurface()
    {
        StartCoroutine(TemporarilyDisableSoap());
    }

    private IEnumerator TemporarilyDisableSoap()
    {
        soapCollider.enabled = false;
        Debug.Log("Dish Soap disabled");

        yield return new WaitForSeconds(1f);

        soapCollider.enabled = true;
        Debug.Log("Soap enabled!");
    }
}
