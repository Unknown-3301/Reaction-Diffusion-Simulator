using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListComputeBuffer<T> where T : struct
{
    int stride;
    List<T> objects;
    public ComputeBuffer Buffer { get; private set; }

    public ListComputeBuffer(T firstElement, int stride)
    {
        this.stride = stride;

        objects = new List<T>() { firstElement };

        CreateBuffer();
    }
    public ListComputeBuffer(List<T> list, int stride)
    {
        this.stride = stride;

        objects = list;

        CreateBuffer();
    }

    void CreateBuffer()
    {
        Buffer = new ComputeBuffer(objects.Count, stride);
        Buffer.SetData(objects);
    }
    public void RecreateBuffer()
    {
        Buffer.Release();

        Buffer = new ComputeBuffer(objects.Count, stride);
        Buffer.SetData(objects);
    }

    public T GetData(int index) => objects[index];
    public void SetData(int index, T data)
    {
        objects[index] = data;
        Buffer.SetData(objects);
    }
    public void SetRange(int index, List<T> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            objects[i + index] = data[i];
        }
        Buffer.SetData(objects);
    }

    public void UpdateBuffer()
    {
        Buffer.SetData(objects);
    }

    public void AddData(T data)
    {
        objects.Add(data);
        RecreateBuffer();
    }
    public void AddRange(List<T> data)
    {
        objects.AddRange(data);
        RecreateBuffer();
    }
    public void RemoveAt(int index)
    {
        objects.RemoveAt(index);
        RecreateBuffer();
    }
    public void RemoveRange(int startIndex, int count)
    {
        objects.RemoveRange(startIndex, count);
        RecreateBuffer();
    }

    public void Destroy()
    {
        Buffer.Release();
    }
}
