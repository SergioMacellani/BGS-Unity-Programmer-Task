using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    //TODO:: Remove after make the HUD Manager
    public RectTransform interactionPanel;
    [SerializeField]
    private Transform interactionIcon;
    [SerializeField]
    private Transform vendorCharacter;

    private bool canInteract = false;
    private Vector3 vendorCharacterInitialScale;

    private void Start()
    {
        vendorCharacterInitialScale = vendorCharacter.localScale;
    }

    private void FixedUpdate()
    {
        vendorCharacter.localScale =
            new Vector3(Mathf.Sign(Camera.main.transform.position.x - vendorCharacter.position.x) * vendorCharacterInitialScale.x,
            vendorCharacterInitialScale.y,
            vendorCharacterInitialScale.z);

        if (!canInteract) return;

        interactionPanel.position = Camera.main.WorldToScreenPoint(interactionIcon.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        canInteract = true;
        interactionPanel.position = Camera.main.WorldToScreenPoint(interactionIcon.position);
        interactionPanel.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        canInteract = false;
        interactionPanel.gameObject.SetActive(false);
    }
}
