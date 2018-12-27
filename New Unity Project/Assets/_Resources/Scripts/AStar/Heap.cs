using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap <T> where T : IHeapItem<T> {

    T[] items;
    int curItemCount;

    public Heap (int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void add(T item)
    {
        item.HeapIndex = curItemCount;
        items[curItemCount] = item;
        sortUp(item);
        curItemCount++;
    }

    public T removeFirst()
    {
        T first = items[0];
        curItemCount--;
        items[0] = items[curItemCount];
        items[0].HeapIndex = 0;
        sortDown(items[0]);
        return first;
    }

    public void updateItem(T item)
    {
        sortUp(item);
    }

    public int Count
    {
        get
        {
            return curItemCount;
        }
    }

    public bool contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void sortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < curItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < curItemCount)
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;

                if (item.CompareTo(items[swapIndex]) < 0)
                    swap(item, items[swapIndex]);
                else return;
            }
            else return;
        }
    }

    void sortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while(true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void swap(T item1, T item2)
    {
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;
        int item1Index = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = item1Index;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}