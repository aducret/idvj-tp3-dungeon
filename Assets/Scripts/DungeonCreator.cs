using System;
using System.Collections;

using UnityEngine;

[ExecuteInEditMode]
public class DungeonCreator : MonoBehaviour
{
	public GameObject[] dungeonParts;
	public int dungeonSize = 10;
	
	private Transform mountTransform;
	
	public void Generate()
	{
		mountTransform = gameObject.transform;
		mountTransform.position = new Vector3(0, 0, 0);

        GameObject firstGO = Instantiate(dungeonParts[1]) as GameObject;
        firstGO.name = "part-0";
        firstGO.transform.parent = transform;
        firstGO.transform.localPosition = new Vector3(0, 0, 0);

        mountTransform = firstGO.GetComponent<DungeonPart>().mountPoints[0];

        for (int i=1; i<dungeonSize; i++)
		{
			int dungeonPartIndex = getRandomIndex();
			
			GameObject newGO = Instantiate(dungeonParts[dungeonPartIndex]) as GameObject;
			newGO.name = String.Format("part-{0}", i);
			newGO.transform.parent = transform;
            newGO.transform.localPosition = mountTransform.TransformVector(newGO.GetComponent<DungeonPart>().mountPoints[0].transform.localPosition);

            mountTransform = newGO.GetComponent<DungeonPart>().mountPoints[0];            
		}
		
	}
	
	public void RemoveAll()
	{
		DungeonPart[] rc = gameObject.GetComponentsInChildren<DungeonPart>();
		
		for (int i=0; i<rc.Length; i++)
		{
			DestroyImmediate(rc[i].gameObject);
		}
	}
	
	private int getRandomIndex()
	{
        return 1; 
		// return UnityEngine.Random.Range(0, dungeonParts.Length);
	}

}
