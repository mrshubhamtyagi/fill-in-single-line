using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private List<Block> blockList = new List<Block>();
    private Queue<Block> blockQueue = new Queue<Block>();

    #region Getters and Setters
    public Block GetBlockFromList(int index)
    {
        if (index < blockList.Count)
            return blockList[index];
        else
        {
            Debug.LogError("Invalid Index");
            return null;
        }

    }
    public int GetBlockIndexInList(Block block)
    {
        if (blockList.Count > 0 && blockList.Contains(block))
            return blockList.IndexOf(block);
        else
        {
            Debug.LogError("Invalid Index");
            return -1;
        }
    }
    public void AddToQueue(Block block)
    {
        blockQueue.Enqueue(block);
        print(string.Format("{0} is added to the Queue", block.name));
    }
    public void RemoveFromQueue()
    {
        Block removedBlock = null;

        if (blockQueue.Count > 0)

            removedBlock = blockQueue.Dequeue();
        else
            Debug.Log("Queue is Empty");

        if (removedBlock != null)
            print(string.Format("{0} is removed from the Queue", removedBlock.name));
    }
    public bool IsBlockInQueue(Block block)
    {
        if (blockQueue.Count > 0 && blockQueue.Contains(block))
            return true;
        else
            return false;
    }
    public int GetBlockIndexInQueue(Block block)
    {
        int index = 0;
        if (blockQueue.Count > 0)
        {
            foreach (Block _block in blockQueue)
            {
                if (_block == block)
                    return index;
                else
                    index++;
            }
            Debug.LogError("Block not found");
            return -1;
        }
        else
        {
            Debug.Log("Queue is Empty");
            return -1;
        }

    }
    public int GetQueueLength()
    {
        return blockQueue.Count;
    }
    public void ClearBlockQueue()
    {
        blockQueue.Clear();
    }
    #endregion

    private void Awake()
    {
        foreach (Transform child in transform)
            blockList.Add(child.GetComponent<Block>());
    }

    private void Start()
    {
        print("Block List Count ---->>>> " + blockList.Count);
        foreach (Block block in blockList)
            block.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RemoveBlocksAfterIndex(int index)
    {
        if (index < blockQueue.Count)
        {
            int noOfBlocksToDelete = blockQueue.Count - index;
            for (int i = 0; i < index; i++)
            {
                RemoveFromQueue();
            }

            print(string.Format("{0} blocks removed. New Length is {1}", noOfBlocksToDelete, GetQueueLength()));
        }
        else
            Debug.LogError("Invalid Index");
    }


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //ClearBlockQueue();
        }
    }
}
