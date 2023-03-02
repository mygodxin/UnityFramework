using System.Xml.Linq;
using System;
using UnityEngine;
using System.Linq;

public interface IRandomObject
{
}

public class DefaultRandomObject<T> : IRandomObject
{
    public T obj;
    int RandomNumber;
    public DefaultRandomObject(T obj, int RandomNumber)
    {
        this.RandomNumber = RandomNumber;
    }
}
public class RandomCollections<T>: IRandomObject
{
    private T[] randomObjects;
}

