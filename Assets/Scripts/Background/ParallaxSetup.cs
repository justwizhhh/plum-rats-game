using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxSetup : MonoBehaviour
{
    public GameObject[] bgLayers;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= bgLayers.Length - 1; i++)
        {
            var layerClone0 = Instantiate(bgLayers[i]);
            layerClone0.transform.position += new Vector3(0, 0, 0);
            var layerClone1 = Instantiate(bgLayers[i]);
            layerClone1.transform.position += new Vector3(bgLayers[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x, 0, 0);
            var layerClone2 = Instantiate(bgLayers[i]);
            layerClone2.transform.position += new Vector3(bgLayers[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x * 2, 0, 0);
        }
    }
}
