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
		
		float rotY = 0.0f;
		
		for (int i=0; i<dungeonSize; i++)
		{
			int dungeonPartIndex = getRandomIndex();
			
			GameObject newGO = Instantiate(dungeonParts[dungeonPartIndex]) as GameObject;
			newGO.name = String.Format("part-{0}", i);
			newGO.transform.parent = transform;
            print(newGO.transform.localPosition);
			newGO.transform.localPosition = mountTransform.TransformPoint(newGO.GetComponent<DungeonPart>().mountPoints[0].transform.localPosition);

           //print("mountTransform: " + mountTransform.position);
           // print("newGO.transform.localPosition: " + newGO.transform.localPosition);

            newGO.transform.Rotate(new Vector3(0, rotY, 0));
			
			mountTransform = newGO.GetComponent<DungeonPart>().mountPoints[0];
			
			rotY += mountTransform.localRotation.eulerAngles.y;
            
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
		return UnityEngine.Random.Range(0, dungeonParts.Length);
	}

}
