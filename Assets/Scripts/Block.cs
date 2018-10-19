using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockManager blockManager;
    private GameObject greyBlock;

    public enum BlockState
    {
        Checked,
        Unchecked
    }

    private BlockState state = BlockState.Unchecked;
    private bool isStartingBlock = false;
    private Vector2Int positionInMatrix; // x=row, y = col

    #region Getters and Setters
    public void SetAsStartBlock()
    {
        isStartingBlock = true;
        state = BlockState.Checked;
        greyBlock.SetActive(true);
        blockManager.AddToStack(this);
    }
    public void SetCheckBlockColor(Color colr)
    {
        greyBlock.GetComponent<SpriteRenderer>().color = colr;
    }
    public void SetPositionInMatrix(Vector2Int position)
    {
        positionInMatrix = position;
    }
    public Vector2Int GetBlockPositionInMatrix()
    {
        return positionInMatrix;
    }

    #endregion

    private void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        greyBlock = transform.GetChild(0).gameObject;
    }

    private void OnMouseEnter()
    {
        // we need not to do anything with the starting block
        if (isStartingBlock)
            return;

        // if this block is not adjustcent to the prev checked block
        if (!IsAdjucent())
            return;

        // change block state
        if (state == BlockState.Unchecked)
        {
            state = BlockState.Checked;
            greyBlock.SetActive(true);
            blockManager.AddToStack(this);

            // check is level is completed
            if (blockManager.IsLevelCompleted())
                blockManager.OnLevelCompleted();
        }
        else if (state == BlockState.Checked && !blockManager.IsLevelCompleted())
        {
            state = BlockState.Unchecked;
            greyBlock.SetActive(false);
            blockManager.RemoveFromStack();
        }
    }


    // checks if the block is adjustcent to prev checked block
    // we need to check this so that no dialognal blocks are checked
    private bool IsAdjucent()
    {
        bool isAdjucent = false;
        Vector2Int lastCheckedPositionInMatrix = blockManager.GetLastBlockInStack().GetBlockPositionInMatrix();

        if (lastCheckedPositionInMatrix.x - 1 == GetBlockPositionInMatrix().x
           && lastCheckedPositionInMatrix.y == GetBlockPositionInMatrix().y)
            isAdjucent = true;
        else if (lastCheckedPositionInMatrix.x + 1 == GetBlockPositionInMatrix().x
           && lastCheckedPositionInMatrix.y == GetBlockPositionInMatrix().y)
            isAdjucent = true;
        else if (lastCheckedPositionInMatrix.x == GetBlockPositionInMatrix().x
           && lastCheckedPositionInMatrix.y - 1 == GetBlockPositionInMatrix().y)
            isAdjucent = true;
        else if (lastCheckedPositionInMatrix.x == GetBlockPositionInMatrix().x
         && lastCheckedPositionInMatrix.y + 1 == GetBlockPositionInMatrix().y)
            isAdjucent = true;

        return isAdjucent;
    }
}
