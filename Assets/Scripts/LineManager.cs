using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private BlockManager blockManager;
    private LineRenderer lineRenderer;
    private int positionIndex = 0;

    #region
    public void SetPositionIndex(int index)
    {
        positionIndex = index;
        lineRenderer.positionCount = positionIndex + 1;
    }
    public void SetInitialPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void RemoveLastPosition()
    {
        lineRenderer.positionCount--;
        positionIndex--;
    }
    public void RemoveAllPositionTill(int index)
    {
        for (int i = positionIndex; i > index; i--)
        {
            lineRenderer.positionCount--;
            positionIndex--;
        }

    }
    #endregion

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        blockManager = FindObjectOfType<BlockManager>();
    }

    public void SetNextPosition(Vector3 pos)
    {
        pos.z = -1;
        lineRenderer.SetPosition(positionIndex, pos);
    }




}
