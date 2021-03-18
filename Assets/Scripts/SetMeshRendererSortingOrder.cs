using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMeshRendererSortingOrder : MonoBehaviour
{
    public Renderer render;
 
    public string sortingLayerName;
 
    public void Start()
    {
        render.sortingLayerName = sortingLayerName;
    }
}
