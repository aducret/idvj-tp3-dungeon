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
        GameObject firstGO = Instantiate(dungeonParts[getRandomIndex()]) as GameObject;
        firstGO.name = "part-0";
        firstGO.transform.parent = transform;
        firstGO.transform.position = new Vector3(0, 0, 0);

        mountTransform = getRandomMountPoint(firstGO.GetComponent<DungeonPart>().mountPoints);
        Vector3 mountTransformDirection = mountTransform.GetComponent<MountPoint>().direction;
        printCardinalPoint(mountTransformDirection);
        for (int i=1; i<dungeonSize; i++)
		{
			int dungeonPartIndex = getRandomIndex();
			
			GameObject newGO = Instantiate(dungeonParts[dungeonPartIndex]) as GameObject;
			newGO.name = String.Format("part-{0}", i);
			newGO.transform.parent = transform;

            MountPoint mp = getRandomMountPoint(newGO.GetComponent<DungeonPart>().mountPoints).GetComponent<MountPoint>();
            Vector3 mpDirection = mp.direction;
            printCardinalPoint(mpDirection);
            TranslateAndRotation tr = getTranslateAndRotation(mountTransformDirection, mpDirection);
            print(tr.translationScale);
            print(tr.rotation);
            newGO.transform.Rotate(tr.rotation);
            updateMountPointDirections(newGO.GetComponent<DungeonPart>(), tr.rotation);
            print(mp.translation);
            newGO.transform.position = mountTransform.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));
            //mountTransform = mp;
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
        //return 1; 
		return UnityEngine.Random.Range(0, dungeonParts.Length);
	}

    private Transform getRandomMountPoint(Transform[] mps)
    {
        int index = UnityEngine.Random.Range(0, mps.Length);
        print(index);
        return mps[index];
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
        if (Math.Abs(rotation.y) <= 0.00001)
            return;

        if (rotation.y == 90 )
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

        if (rotation.y == -90)
        {
            foreach (Transform mp in dp.mountPoints)
            {
                print("Entre");
                Vector3 direction = mp.GetComponent<MountPoint>().direction;
                mp.GetComponent<MountPoint>().direction = new Vector3(-1 * direction.z, 0, direction.x);
                Vector3 translation = mp.GetComponent<MountPoint>().translation;
                mp.GetComponent<MountPoint>().translation = new Vector3(-1 * translation.z, translation.y, translation.x);
            }
        }

        if (rotation.y == 180)
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

        Debug.Log("This should never happend (updateMountPointDirections)");
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

    private void printCardinalPoint(Vector3 direction)
    {
        if (isNorth(direction))
            print("North");
        if (isSouth(direction))
            print("South");
        if (isEast(direction))
            print("East");
        if (isWest(direction))
            print("West");
    }

}
