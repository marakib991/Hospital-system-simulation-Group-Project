﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Priority
{
    // Used by class PriorityQueue<T>
    // Implements IComparable and overrides ToString (from Object)

    public class PriorityClass : IComparable
    {
        private int priorityValue;       // Ordered on priorityValue
        private String name;

        // Constructor

        public PriorityClass(int priority, String name)
        {
            this.name = name;
            priorityValue = priority;
        }

        // CompareTo (inherited from IComparable)
        // Returns >0 if the current item is greater than obj (null or otherwise)
        // Returns 0  if the current item is equal to obj (of PriorityClass)
        // Returns <0 if the current item is less than obj (of PriorityClass)

        public int CompareTo(Object obj)
        {
            if (obj != null)
            {
                PriorityClass other = (PriorityClass)obj;   // Explicit cast
                if (other != null)
                    return priorityValue - other.priorityValue;
                else
                    return 1;
            }
            else
                return 1;
        }

        // ToString (overridden from Object class)
        // Returns a string represent of an object of PriorityClass

        public override string ToString()
        {
            return name + " with priority " + priorityValue;
        }
    }
}

namespace COIS_2020H_Assignment2_DavidChan_ChengjunYin_MohammadRakib
{


    using Priority;

    public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //-----------------------------------------------------------------------------

    public interface IPriorityQueue<T> : IContainer<T> where T : IComparable
    {
        void Insert(T item);  // Insert an item to a priority queue
        void Remove();        // Remove the item with the highest priority
        T Front();            // Return the item with the highest priority
    }

    //-------------------------------------------------------------------------

    // Priority Queue
    // Implementation:  Binary heap

    public class PriorityQueue<T> where T : IComparable
    {
        public T[] A { get; private set; }         // Linear array of items (Generic)
        private int capacity;  // Maximum number of items in a priority queue
        private int count;     // Number of items in the priority queue

        // Constructor 1
        // Create an empty priority queue
        // Time complexity:  O(1)

        public PriorityQueue()
        {
            capacity = 3;
            A = new T[capacity + 1];  // Indexing begins at 1
            MakeEmpty();
        }

        // Constructor 2
        // Create a priority queue from an array of items
        // Time complexity:  O(n)

        public PriorityQueue(T[] inputArray)
        {
            int i;

            count = capacity = inputArray.Length;
            A = new T[capacity + 1];

            for (i = 0; i < capacity; i++)
            {
                A[i + 1] = inputArray[i];
            }

            BuildHeap();
        }

        // MakeEmpty
        // Reset a priority queue to empty
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            count = 0;
        }

        // Empty
        // Return true if the priority is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return count == 0;
        }

        // Size
        // Return the number of items in the priority queue
        // Time complexity:  O(1)

        public int Size()
        {
            return count;
        }

        // DoubleCapacity
        // Doubles the capacity of the priority queue
        // Time complexity:  O(n)

        private void DoubleCapacity()
        {
            T[] oldA = A;

            capacity = 2 * capacity;
            A = new T[capacity + 1];
            for (int i = 1; i <= count; i++)
            {
                A[i] = oldA[i];
            }
        }

        // PercolateUp
        // Percolate up an item from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateUp(int i)
        {
            int child = i, parent;

            // As long as child is not the root (i.e. has a parent)
            while (child > 1)
            {
                parent = child / 2;

                if (A[child].CompareTo(A[parent]) < 0)
                // If child has a higher priority than parent
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    child = parent;  // Move up child index to parent index
                }
                else
                    // Item is in its proper position
                    return;
            }
        }

        // Insert
        // Insert an item into the priority queue
        // Time complexity:  O(log n)

        public void Insert(T item)
        {
            if (count == capacity)
            {
                DoubleCapacity();
            }
            A[++count] = item;      // Place item at the next available position
            PercolateUp(count);
        }

        // PercolateDown
        // Percolate down an item from position i in a priority queue
        // Time complexity:  O(log n)

        private void PercolateDown(int i)
        {
            int parent = i, child;

            // while parent has at least one child
            while (2 * parent <= count)
            {
                // Select the child with the highest priority
                child = 2 * parent;    // Left child index
                if (child < count)  // Right child also exists
                    if (A[child + 1].CompareTo(A[child]) < 0)
                        // Right child has a higher priority than left child
                        child++;

                // If child has a higher priority than parent
                if (A[child].CompareTo(A[parent]) < 0)
                {
                    // Swap parent and child
                    T item = A[child];
                    A[child] = A[parent];
                    A[parent] = item;
                    parent = child;  // Move down parent index to child index
                }
                else
                    // Item is in its proper place
                    return;
            }
        }

        // Remove
        // Remove (if possible) the item with the highest priority
        // Otherwise throw an exception
        // Time complexity:  O(log n)

        public void Remove()
        {
            if (Empty())
                throw new InvalidOperationException("Priority queue is empty");
            else
            {
                // Remove item with highest priority (root) and
                // replace it with the last item
                A[1] = A[count--];

                // Percolate down the new root item
                PercolateDown(1);
            }
        }

        // Front
        // Return (if possible) the item with the highest priority
        // Otherwise throw an exception
        // Time complexity:  O(1)

        public T Front()
        {
            if (Empty())
                throw new InvalidOperationException("Priority queue is empty");
            else
                return A[1];  // Return the root item (highest priority)
        }

        // BuildHeap
        // Create a binary heap from an arbitrary array
        // Time complexity:  O(n)

        private void BuildHeap()
        {
            int i;

            // Percolate down from the last parent to the root (first parent)
            for (i = count / 2; i >= 1; i--)
            {
                PercolateDown(i);
            }
        }

        // HeapSort
        // Sort and return inputArray
        // Time complexity:  O(n log n)

        public void HeapSort(T[] inputArray)
        {
            int i;

            capacity = count = inputArray.Length;

            // Copy input array to A (indexed from 1)
            for (i = capacity - 1; i >= 0; i--)
            {
                A[i + 1] = inputArray[i];
            }

            // Create a binary heap
            BuildHeap();

            // Remove the next item and place it into the input (output) array
            for (i = 0; i < capacity; i++)
            {
                inputArray[i] = Front();
                Remove();
            }
        }

        // Peek
        // Return the item at position i in the priority queue
        // Time complexity: O(1)
        public T Peek(int i)
        {
            if (i < 1 || i > count)
                throw new ArgumentOutOfRangeException("Index is out of range");
            else
                return A[i];
        }

        // RemoveAt
        // Remove the item at position i in the priority queue
        // Time complexity: O(log n)
        public void RemoveAt(int i)
        {
            if (i < 1 || i > count)
                throw new ArgumentOutOfRangeException("Index is out of range");
            else
            {
                A[i] = A[count--]; // Replace with the last item
                PercolateUp(i); // Fix the heap property if necessary
                PercolateDown(i);
            }
        }
    }

}