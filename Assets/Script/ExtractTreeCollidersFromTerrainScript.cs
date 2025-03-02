using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ExtractTreeCollidersFromTerrainScript : MonoBehaviour
{
    [ContextMenu("Extract")]
    public void Extract()
    {
        Debug.Log("ExtractTreeCollidersFromTerrain::Extract");
        Terrain terrain = GetComponent<Terrain>();
        Transform[] transforms = terrain.GetComponentsInChildren<Transform>();
        for(int i=1;i<transforms.Length;i++)
        {
            DestroyImmediate(transforms[i].gameObject);
        }
        Debug.Log("Tree orototypes cout:" + terrain.terrainData.treePrototypes.Length);
        for(int i=0;i<terrain.terrainData.treePrototypes.Length;i++)
        {
            TreePrototype tree = terrain.terrainData.treePrototypes[i];

            TreeInstance[] instances = terrain.terrainData.treeInstances.Where(x => x.prototypeIndex == i).ToArray();

            for(int j=0;j<instances.Length;j++)
            {
                instances[i].position = Vector3.Scale(instances[j].position, terrain.terrainData.size);
                instances[j].position += terrain.GetPosition();
                NavMeshObstacle navmeshobstacle = tree.prefab.GetComponent<NavMeshObstacle>();
                if (navmeshobstacle)
                {
                    Debug.LogWarning("Tree with prototype[" + i + "]instance[" + j + "]did not have a NavmeshObstacle component,skipping! ");
                    continue;
                }

                Vector3 primitivescale = navmeshobstacle.size;
                if(navmeshobstacle.shape == NavMeshObstacleShape.Capsule)
                {
                    primitivescale = navmeshobstacle.radius * Vector3.one;
                }
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                obj.name = tree.prefab.name + j;
                if (terrain.preserveTreePrototypeLayers) obj.layer = tree.prefab.layer;
                else obj.layer = terrain.gameObject.layer;
                obj.transform.localScale = primitivescale;
                obj.transform.position = instances[i].position;
                obj.transform.parent = terrain.transform;
                obj.isStatic = true;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
