using Slicer2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FruitSlicerEvent2 : FruitSlicerEvent
{
    public event UnityAction OnSliced;

    private void Start()
    {
        Sliceable2D slicer = GetComponent<Sliceable2D>();
        slicer.AddResultEvent(Hui);
    }

    private void Hui(Slice2D slice)
    {
        OnSliced?.Invoke();
    }
}
