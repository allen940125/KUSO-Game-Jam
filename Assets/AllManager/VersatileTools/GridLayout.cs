using UnityEngine;
using System.Collections.Generic;

public class CustomGridLayout : MonoBehaviour
{
    [System.Serializable]
    public class GridCategory
    {
        public Vector3 startPosition;
        public int itemCount;
        public int columns;
    }

    public List<GridCategory> categories;
    public float cellWidth;
    public float cellHeight;

    private int previousChildCount;

    void Start()
    {
        previousChildCount = transform.childCount;
        LayoutGrid();
    }

    void Update()
    {
        if (transform.childCount != previousChildCount)
        {
            previousChildCount = transform.childCount;
            LayoutGrid();
        }
    }

    private void OnValidate()
    {
        LayoutGrid();
    }

    void LayoutGrid()
    {
        int itemIndex = 0;

        foreach (var category in categories)
        {
            for (int i = 0; i < category.itemCount; i++)
            {
                if (itemIndex >= transform.childCount)
                    return;

                Transform child = transform.GetChild(itemIndex);
                int row = i / category.columns;
                int column = i % category.columns;
                Vector3 newPosition = category.startPosition + new Vector3(column * cellWidth, -row * cellHeight, 0);
                child.localPosition = newPosition;

                // 設置 Z 軸旋轉角度
                //float rotationZ = GetRotationZ(itemIndex);
                //child.localRotation = Quaternion.Euler(0, 0, rotationZ);

                itemIndex++;
            }
        }
    }

    float GetRotationZ(int index)
    {
        // 根據索引設置旋轉角度
        switch (index % 5)
        {
            case 0: return 30f;
            case 1: return 15f;
            case 2: return 0f;
            case 3: return -15f;
            case 4: return -30f;
            default: return 0f;
        }
    }
}
