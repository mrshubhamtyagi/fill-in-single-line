using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockManager blockManager;
    private GameObject greyBlock;

    private bool isGreyActive = false;


    private void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        greyBlock = transform.GetChild(0).gameObject;
    }

    private void OnMouseEnter()
    {
        isGreyActive = !isGreyActive;
        greyBlock.SetActive(isGreyActive);
        if (isGreyActive)
            blockManager.AddToQueue(this);
        else
            blockManager.RemoveFromQueue();
    }

}
