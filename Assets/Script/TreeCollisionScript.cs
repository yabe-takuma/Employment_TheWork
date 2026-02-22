using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollisionScript : MonoBehaviour
{
   

    public int treeIndex;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Boss")
        {
            Terrain terrain = Terrain.activeTerrain;
            List<TreeInstance> trees = new List<TreeInstance>(terrain.terrainData.treeInstances);
            trees[treeIndex] = new TreeInstance();
            terrain.terrainData.treeInstances = trees.ToArray();
            Destroy(terrain);
        }
      

    }
    public void OnTriggerExit(Collider other)
    {
       
    }
}
