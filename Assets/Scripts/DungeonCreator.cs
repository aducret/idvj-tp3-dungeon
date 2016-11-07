using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DungeonCreator : MonoBehaviour
{
	public GameObject[] dungeonParts;
	public int minimumDungeonSize = 10;
    public GameObject wall;

    private List<MountPoint> opens = new List<MountPoint>();
    private int roomIndex = 2;

    public void Generate()
	{
        GameObject a = Instantiate(dungeonParts[1]) as GameObject;
        a.transform.position = new Vector3(100, 100, 100);
        a.transform.Rotate(new Vector3(0,90,0));
        opens = new List<MountPoint>();
        createFirstDungeonPart();
        int missingParts = minimumDungeonSize;
        int i = 1;
        while (missingParts > 0 )
		{
            MountPoint currentMountPoint = getRandomOpen();

            int dungeonPartIndex = getRandomIndex();
            if (opens.Count <= 1 && missingParts > 1)
            {
                int[] filter = new int[1];
                filter[0] = roomIndex;
                dungeonPartIndex = getRandomIndexWithFilter(filter);
            }
            GameObject dungeonPart = Instantiate(dungeonParts[dungeonPartIndex]) as GameObject;
			dungeonPart.name = String.Format("part-{0}", i);
			dungeonPart.transform.parent = transform;
            int index = getRandomIndex(dungeonPart.GetComponent<DungeonPart>().mountPoints);
            Transform mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            MountPoint mp = mpTransform.GetComponent<MountPoint>();
            TranslateAndRotation tr = getTranslateAndRotation(currentMountPoint.direction, mp.direction);
            
            dungeonPart.transform.Rotate(tr.rotation);
            updateMountPointDirections(dungeonPart.GetComponent<DungeonPart>(), tr.rotation);
            mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            mp = mpTransform.GetComponent<MountPoint>();
            dungeonPart.transform.position = currentMountPoint.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));

            addToOpensExept(dungeonPart.GetComponent<DungeonPart>().mountPoints, index);
            missingParts--;
            i++;
		}
        
        while (opens.Count > 0)
        {
            MountPoint currentMountPoint = getRandomOpen();

            // Completo con habitaciones.
            // GameObject dungeonPart = Instantiate(dungeonParts[roomIndex]) as GameObject;
            GameObject dungeonPart = Instantiate(wall) as GameObject;
            dungeonPart.name = String.Format("part-{0}", i);
            dungeonPart.transform.parent = transform;

            int index = getRandomIndex(dungeonPart.GetComponent<DungeonPart>().mountPoints);
            addToOpensExept(dungeonPart.GetComponent<DungeonPart>().mountPoints, index);
            Transform mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            MountPoint mp = mpTransform.GetComponent<MountPoint>();
            TranslateAndRotation tr = getTranslateAndRotation(currentMountPoint.direction, mp.direction);

            print(dungeonPart.transform.eulerAngles);
            dungeonPart.transform.Rotate(tr.rotation);
            print(dungeonPart.transform.eulerAngles);
            updateMountPointDirections(dungeonPart.GetComponent<DungeonPart>(), tr.rotation);
            mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            mp = mpTransform.GetComponent<MountPoint>();
            dungeonPart.transform.position = currentMountPoint.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));
            i++;
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
	
    private MountPoint getRandomOpen()
    {
        int index = UnityEngine.Random.Range(0, opens.Count - 1);
        MountPoint mp = opens[index];
        opens.Remove(mp);
        return mp;
    }

    private void createFirstDungeonPart()
    {
        GameObject firstGO = Instantiate(dungeonParts[getRandomIndex()]) as GameObject;
        firstGO.name = "part-0";
        firstGO.transform.parent = transform;
        firstGO.transform.position = new Vector3(0, 0, 0);

        foreach (Transform t in firstGO.GetComponent<DungeonPart>().mountPoints)
        {
            opens.Add(t.GetComponent<MountPoint>());
        }
    }

    private int getRandomIndex()
    {
        return UnityEngine.Random.Range(0, dungeonParts.Length);
    }

    private int getRandomIndexWithFilter(int[] indexesToFilter)
	{
        int index = UnityEngine.Random.Range(0, dungeonParts.Length);
        while (contains(indexesToFilter, index))
        {
            index = UnityEngine.Random.Range(0, dungeonParts.Length);
        }
        return index;
	}

    private bool contains(int [] list, int value)
    {
        foreach (int elem in list)
        {
            if (elem == value)
                return true;
        }
        return false;
    }

    private int getRandomIndex(Transform[] mps)
    {
        return UnityEngine.Random.Range(0, mps.Length);
    }

    private void addToOpensExept(Transform[] mps, int index)
    {
        for (int i = 0; i < mps.Length; i++)
        {
            if (i == index)
                continue;
            opens.Add(mps[i].GetComponent<MountPoint>());
        }
    }

    private bool isNorth(Vector3 direction)
    {
        if (direction.x == -1 && direction.y == 0 && direction.z == 0)
            return true;

        return false;
    }

    private bool isSouth(Vector3 direction)
    {
        if (direction.x == 1 && direction.y == 0 && direction.z == 0)
            return true;

        return false;
    }

    private bool isWest(Vector3 direction)
    {
        if (direction.x == 0 && direction.y == 0 && direction.z == -1)
            return true;

        return false;
    }

    private bool isEast(Vector3 direction)
    {
        if (direction.x == 0 && direction.y == 0 && direction.z == 1)
            return true;

        return false;
    }

    private TranslateAndRotation getTranslateAndRotation(Vector3 direction1, Vector3 direction2)
    {
        if (isNorth(direction1))
        {
            if (isNorth(direction2))
                return new TranslateAndRotation(new Vector3(-1, -1, 0), new Vector3(0, 180, 0));

            if (isSouth(direction2))
                return new TranslateAndRotation(new Vector3(-1, -1, 0), new Vector3(0, 0, 0));

            if (isEast(direction2))
                return new TranslateAndRotation(new Vector3(-1, -1, 0), new Vector3(0, 90, 0));

            if (isWest(direction2))
                return new TranslateAndRotation(new Vector3(-1, -1, 0), new Vector3(0, -90, 0));
        }

        if (isSouth(direction1))
        {
            if (isNorth(direction2))
                return new TranslateAndRotation(new Vector3(1, -1, 0), new Vector3(0, 0, 0));

            if (isSouth(direction2))
                return new TranslateAndRotation(new Vector3(1, -1, 0), new Vector3(0, 180, 0));

            if (isEast(direction2))
                return new TranslateAndRotation(new Vector3(1, -1, 0), new Vector3(0, -90, 0));

            if (isWest(direction2))
                return new TranslateAndRotation(new Vector3(1, -1, 0), new Vector3(0, 90, 0));
        }

        if (isEast(direction1))
        {
            if (isNorth(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, 1), new Vector3(0, -90, 0));

            if (isSouth(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, 1), new Vector3(0, 90, 0));

            if (isEast(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, 1), new Vector3(0, 180, 0));

            if (isWest(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, 1), new Vector3(0, 0, 0));
        }

        if (isWest(direction1))
        {
            if (isNorth(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, -1), new Vector3(0, 90, 0));

            if (isSouth(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, -1), new Vector3(0, -90, 0));

            if (isEast(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, -1), new Vector3(0, 0, 0));

            if (isWest(direction2))
                return new TranslateAndRotation(new Vector3(0, -1, -1), new Vector3(0, 180, 0));
        }

        Debug.Log("This should never happend (getTranslateAndRotation)");
        return new TranslateAndRotation(new Vector3(1, 1, 1), new Vector3(0, 0, 0));
    }

    private void updateMountPointDirections(DungeonPart dp, Vector3 rotation)
    {
        if (Math.Abs(rotation.y) <= 0.001)
            return;

        if (Math.Abs(rotation.y - 90) <= 0.001)
        {
            foreach (Transform mp in dp.mountPoints)
            {
                Vector3 direction = mp.GetComponent<MountPoint>().direction;
                mp.GetComponent<MountPoint>().direction = new Vector3(direction.z, 0, -1 * direction.x);
                Vector3 translation = mp.GetComponent<MountPoint>().translation;
                mp.GetComponent<MountPoint>().translation = new Vector3(translation.z, translation.y, -1 * translation.x);
            }
            return;
        }

        if (Math.Abs(rotation.y + 90) <= 0.001)
        {
            foreach (Transform mp in dp.mountPoints)
            {
                Vector3 direction = mp.GetComponent<MountPoint>().direction;
                mp.GetComponent<MountPoint>().direction = new Vector3(-1 * direction.z, 0, direction.x);
                Vector3 translation = mp.GetComponent<MountPoint>().translation;
                mp.GetComponent<MountPoint>().translation = new Vector3(-1 * translation.z, translation.y, translation.x);
            }
            return;
        }

        if (Math.Abs(rotation.y - 180) <= 0.001)
        {
            foreach (Transform mp in dp.mountPoints)
            {
                Vector3 direction = mp.GetComponent<MountPoint>().direction;
                mp.GetComponent<MountPoint>().direction = new Vector3(-1 * direction.x, 0, -1 * direction.z);
                Vector3 translation = mp.GetComponent<MountPoint>().translation;
                mp.GetComponent<MountPoint>().translation = new Vector3(-1 * translation.x, translation.y, -1 * translation.z);
            }
            return;
        }

        Debug.Log("This should never happend (updateMountPointDirections): " + rotation.y);
    }   

    public class TranslateAndRotation
    {
        public Vector3 translationScale;
        public Vector3 rotation;

        public TranslateAndRotation(Vector3 translationScale, Vector3 rotation)
        {
            this.translationScale = translationScale;
            this.rotation = rotation;
        }
    }

}
