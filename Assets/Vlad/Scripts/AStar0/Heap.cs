using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * Min Heap implementation as an array, where, for each node:
 * - parent's index    = (n - 1) / 2
 * - child left index  = 2n + 1
 * - child right index = 2n + 2
 */

public class Heap<T> where T : IHeapItem<T> {
    T[] items;
    int currentItemCount;

    public int Count { 
        get {
            return currentItemCount;
        }
    }

    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    void SortUp(T item) {
        int parentIndex = (item.HeapIndex - 1) / 2;
        
        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            } else {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void SortDown(T item) {
        while (true) {
            int childLeftIdx = item.HeapIndex * 2 + 1;
            int childRightIdx = item.HeapIndex * 2 + 2;
            int swapIdx;

            if (childLeftIdx < currentItemCount) {
                swapIdx = childLeftIdx;
                
                if (childRightIdx < currentItemCount) {
                    if (items[childLeftIdx].CompareTo(items[childRightIdx]) < 0) {
                        swapIdx = childRightIdx;
                    }
                }

                if (item.CompareTo(items[swapIdx]) < 0) {
                    Swap(item, items[swapIdx]);
                } else {
                    return;
                }
            } else {
                return;
            }
        }
    }

    public List<T> ToList() {
        return items.ToList();
    }

    public T RemoveFirst() {
        T first = items[0];

        items[0] = items[currentItemCount - 1];
        items[0].HeapIndex = 0;
        currentItemCount--;

        SortDown(items[0]);

        return first;
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    void Swap(T itemA, T itemB) {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int temp = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = temp;
    }
}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex { get; set; }

}