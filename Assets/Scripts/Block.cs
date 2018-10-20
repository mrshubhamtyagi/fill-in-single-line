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
    private int blockIndexInStack = -1;

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
    public void ChangeBlockState(BlockState blockState)
    {
        state = blockState;
        if (state == BlockState.Checked)
            greyBlock.SetActive(true);
        else if (state == BlockState.Unchecked)
            greyBlock.SetActive(false);

    }
    public void SetBlockIndexInStack(int value)
    {
        blockIndexInStack = value;
    }
    #endregion

    private void Awake()
    {
        blockManager = FindObjectOfType<BlockManager>();
        greyBlock = transform.GetChild(0).gameObject;
    }

    private void OnMouseDown()
    {
        print(string.Format("{0} --- index -----> {1}", gameObject.name, blockIndexInStack));
    }

    private void OnMouseEnter()
    {
        // if level on completed
        if (blockManager.IsLevelCompleted())
        {
            blockManager.OnLevelCompleted();
            return;
        }

        // remove last added block if reached start block
        if (isStartingBlock && blockManager.GetStackLength() > 1)
        {
            print("StartBlock");
            blockManager.RemoveAllBlocksAfterIndex(blockIndexInStack);
            return;
        }
        else
        {
            // change block state
            if (state == BlockState.Unchecked && IsAdjucent())
            {
                blockManager.AddToStack(this);

                // check is level is completed
                if (blockManager.IsLevelCompleted())
                    blockManager.OnLevelCompleted();
            }
            else if (state == BlockState.Checked)
            {
                //Block removedBlock = blockManager.RemoveFromStack();
                blockManager.RemoveAllBlocksAfterIndex(blockIndexInStack);
            }
        }
    }

    // checks if the block is adjustcent to prev checked block
    // we need to check this so that no dialognal blocks are checked
    private bool IsAdjucent()
    {
        bool isAdjucent = false;
        if (blockManager.GetStackLength() > 0 && !blockManager.IsLevelCompleted())
        {
            Vector2Int lastCheckedPositionInMatrix = blockManager.GetLastBlockInStack().GetBlockPositionInMatrix();
            // for col value - X
            if (lastCheckedPositionInMatrix.x - 1 == GetBlockPositionInMatrix().x
               && lastCheckedPositionInMatrix.y == GetBlockPositionInMatrix().y)
                isAdjucent = true;
            else if (lastCheckedPositionInMatrix.x + 1 == GetBlockPositionInMatrix().x
               && lastCheckedPositionInMatrix.y == GetBlockPositionInMatrix().y)
                isAdjucent = true;
            // for row value - Y 
            else if (lastCheckedPositionInMatrix.x == GetBlockPositionInMatrix().x
               && lastCheckedPositionInMatrix.y - 1 == GetBlockPositionInMatrix().y)
                isAdjucent = true;
            else if (lastCheckedPositionInMatrix.x == GetBlockPositionInMatrix().x
             && lastCheckedPositionInMatrix.y + 1 == GetBlockPositionInMatrix().y)
                isAdjucent = true;
        }
        return isAdjucent;
    }
}
