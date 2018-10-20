using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockManager : MonoBehaviour
{
    public Block blockPrefab;
    public Color startBlockColor = Color.red;
    public int rows = 5;
    public int cols = 5;

    [SerializeField] private List<Block> blockList = new List<Block>();
    private Stack<Block> blockStack = new Stack<Block>();
    private LineManager lineManager;

    private Vector2[,] positions = new Vector2[5, 5]
    {
        {new Vector2(-1.8f, 1.8f), new Vector2(-0.9f, 1.8f),new Vector2(0f, 1.8f),new Vector2(0.9f, 1.8f),new Vector2(1.8f, 1.8f)},
        {new Vector2(-1.8f, 0.9f), new Vector2(-0.9f, 0.9f),new Vector2(0f, 0.9f),new Vector2(0.9f, 0.9f),new Vector2(1.8f, 0.9f)},
        {new Vector2(-1.8f, 0f), new Vector2(-0.9f, 0f),new Vector2(0f, 0f),new Vector2(0.9f, 0f),new Vector2(1.8f, 0f)},
        {new Vector2(-1.8f, -0.9f), new Vector2(-0.9f, -0.9f),new Vector2(0f, -0.9f),new Vector2(0.9f, -0.9f),new Vector2(1.8f, -0.9f)},
        {new Vector2(-1.8f, -1.8f), new Vector2(-0.9f, -1.8f),new Vector2(0f, -1.8f),new Vector2(0.9f, -1.8f),new Vector2(1.8f, -1.8f)}
    };

    //public Block[] StackArray;

    #region Getters and Setters
    /*
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
    */
    public void AddToStack(Block block)
    {
        blockStack.Push(block);
        block.ChangeBlockState(Block.BlockState.Checked);
        print(string.Format("{0} is CHECKED", block.name));
        block.SetBlockIndexInStack(GetStackLength() - 1);
        //StackArray = blockStack.ToArray();
    }
    public Block RemoveFromStack()
    {
        Block removedBlock = null;

        if (blockStack.Count > 0)
        {
            removedBlock = blockStack.Pop();
            removedBlock.ChangeBlockState(Block.BlockState.Unchecked);
            removedBlock.SetBlockIndexInStack(-1);
        }

        else
            Debug.Log("Stack is Empty");

        if (removedBlock != null)
            print(string.Format("{0} is UNCHECKED", removedBlock.name));
        //StackArray = blockStack.ToArray();

        return removedBlock;
    }
    public bool IsBlockInStack(Block block)
    {
        if (blockStack.Count > 0 && blockStack.Contains(block))
            return true;
        else
            return false;
    }
    public int GetBlockIndexInStack(Block block)
    {
        int index = 0;
        if (blockStack.Count > 0)
        {
            foreach (Block _block in blockStack)
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
            Debug.Log("Stack is Empty");
            return -1;
        }

    }
    public Block GetLastBlockInStack()
    {
        foreach (Block block in blockStack)
        {
            return block;
        }
        return null;
    }
    public int GetStackLength()
    {
        return blockStack.Count;
    }
    public void ClearStack()
    {
        blockStack.Clear();
    }
    public bool isStackEmpty()
    {
        if (blockStack.Count == 0)
            return true;
        else
            return false;
    }
    public bool IsLevelCompleted()
    {
        // level is completed when all the blocks are checked
        if (blockStack.Count == blockList.Count)
            return true;
        else
            return false;
    }
    #endregion

    private void Awake()
    {
        lineManager = FindObjectOfType<LineManager>();
    }

    private void Start()
    {
        // Spawn all blocks
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                SpawnBlocks(r, c);
            }
        }

        // starting block
        int startBlockIndex = Random.Range(0, blockList.Count);
        blockList[startBlockIndex].SetAsStartBlock();
        lineManager.SetPositionIndex(blockStack.Count - 1);
        lineManager.SetNextPosition(blockList[startBlockIndex].transform.position);
    }

    public void OnLevelCompleted()
    {
        print("Level Complete");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private Block SpawnBlocks(int row, int col)
    {
        Block block = Instantiate(blockPrefab, positions[row, col], Quaternion.identity);
        block.SetCheckBlockColor(startBlockColor);
        block.transform.GetChild(0).gameObject.SetActive(false);
        blockList.Add(block);
        block.gameObject.name = string.Format("Row{0}-Col{1}", row + 1, col + 1);
        block.transform.SetParent(transform);
        block.SetPositionInMatrix(new Vector2Int(row, col));
        return block;
    }

    public void RemoveAllBlocksAfterIndex(int index)
    {
        if (index < blockStack.Count)
        {
            //int noOfBlocksToDelete = blockStack.Count - index;
            int noOfBlocksToDeleted = 0;
            for (int i = blockStack.Count - 1; i != index; i--)
            {
                noOfBlocksToDeleted++;
                RemoveFromStack();
            }

            print(string.Format("{0} blocks removed. New Length is {1}", noOfBlocksToDeleted, GetStackLength()));
        }
        else
            Debug.LogError("Invalid Index");
    }
}
