using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DungeonCreator : MonoBehaviour
{
	public GameObject[] dungeonParts;
	private int minimumDungeonSize;
    public GameObject wall;
    private float difficulty;

    public GameObject goalPrefab;
    public GameObject[] trapPrefabs;
    public GameObject playerPrefab;

    private List<MountPoint> opens = new List<MountPoint>();
    private int roomIndex = 2;
    private List<GameObject> rooms;
    private List<GameObject> trapPlaces;
    private List<GameObject> goalPlaces;
    private List<GameObject> startPlaces;

    public void Generate()
	{
        difficulty = Application.difficulty;
        minimumDungeonSize = Application.size * 10;

        trapPlaces = new List<GameObject>();
        goalPlaces = new List<GameObject>();
        startPlaces = new List<GameObject>();

        rooms = new List<GameObject>();
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

            if (collideWithOtherRoom(dungeonPart.GetComponent<BoxCollider>().bounds))
            {
                DestroyImmediate(dungeonPart);
                dungeonPart = Instantiate(wall) as GameObject;
                dungeonPart.name = String.Format("part-{0}", i);
                dungeonPart.transform.parent = transform;
                index = getRandomIndex(dungeonPart.GetComponent<DungeonPart>().mountPoints);
                mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
                mp = mpTransform.GetComponent<MountPoint>();
                tr = getTranslateAndRotation(currentMountPoint.direction, mp.direction);

                dungeonPart.transform.Rotate(tr.rotation);
                updateMountPointDirections(dungeonPart.GetComponent<DungeonPart>(), tr.rotation);
                mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
                mp = mpTransform.GetComponent<MountPoint>();
                dungeonPart.transform.position = currentMountPoint.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));
            }
            else
            {
                missingParts--;
            }
            addToOpensExept(dungeonPart.GetComponent<DungeonPart>().mountPoints, index);
            rooms.Add(dungeonPart);
            collectTrapsAndGoals(dungeonPart);
            i++;
		}
        
        while (opens.Count > 0)
        {
            MountPoint currentMountPoint = getRandomOpen();

            // Completo con habitaciones, si no entran pongo una pared.
            GameObject dungeonPart = Instantiate(dungeonParts[roomIndex]) as GameObject;
            // GameObject dungeonPart = Instantiate(wall) as GameObject;
            dungeonPart.name = String.Format("part-{0}", i);
            dungeonPart.transform.parent = transform;

            int index = getRandomIndex(dungeonPart.GetComponent<DungeonPart>().mountPoints);
            addToOpensExept(dungeonPart.GetComponent<DungeonPart>().mountPoints, index);
            Transform mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            MountPoint mp = mpTransform.GetComponent<MountPoint>();
            TranslateAndRotation tr = getTranslateAndRotation(currentMountPoint.direction, mp.direction);

            dungeonPart.transform.Rotate(tr.rotation);
            updateMountPointDirections(dungeonPart.GetComponent<DungeonPart>(), tr.rotation);
            mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
            mp = mpTransform.GetComponent<MountPoint>();
            dungeonPart.transform.position = currentMountPoint.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));
            Bounds bounds = dungeonPart.GetComponent<BoxCollider>().bounds;
            if (collideWithOtherRoom(bounds))
            {
                DestroyImmediate(dungeonPart);
                dungeonPart = Instantiate(wall);
                dungeonPart.name = String.Format("part-{0}", i);
                dungeonPart.transform.parent = transform;
                index = getRandomIndex(dungeonPart.GetComponent<DungeonPart>().mountPoints);
                addToOpensExept(dungeonPart.GetComponent<DungeonPart>().mountPoints, index);
                mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
                mp = mpTransform.GetComponent<MountPoint>();
                tr = getTranslateAndRotation(currentMountPoint.direction, mp.direction);

                dungeonPart.transform.Rotate(tr.rotation);
                updateMountPointDirections(dungeonPart.GetComponent<DungeonPart>(), tr.rotation);
                mpTransform = dungeonPart.GetComponent<DungeonPart>().mountPoints[index];
                mp = mpTransform.GetComponent<MountPoint>();
                dungeonPart.transform.position = currentMountPoint.transform.position + new Vector3(tr.translationScale.x * Math.Abs(mp.translation.x), tr.translationScale.y * mp.translation.y, tr.translationScale.z * Math.Abs(mp.translation.z));
            }
            rooms.Add(dungeonPart);
            collectTrapsAndGoals(dungeonPart);
            i++;
        }
        setGoal();
        setTraps();
        setPlayer();
	}

    private void setPlayer()
    {
        int index = UnityEngine.Random.Range(0, startPlaces.Count - 1);
        GameObject startPlace = startPlaces[index];
        (Instantiate(playerPrefab, startPlace.transform.position + new Vector3(0, 0.25f, 0), playerPrefab.transform.rotation) as GameObject).transform.parent = startPlace.transform;
    }

    private void setGoal()
    {
        int index = UnityEngine.Random.Range(0, goalPlaces.Count - 1);
        GameObject goalPlace = goalPlaces[index];
        (Instantiate(goalPrefab, goalPlace.transform.position + new Vector3(0, 0.25f, 0), goalPrefab.transform.rotation) as GameObject).transform.parent = goalPlace.transform;
    }

    private void setTraps()
    {
        int trapsAmount = (int) Math.Floor(difficulty * trapPlaces.Count);
        for (int i = 0; i < trapsAmount; i++)
        {
            int index = UnityEngine.Random.Range(0, trapPlaces.Count - 1);
            GameObject trapPlace = trapPlaces[index];

            int trap = UnityEngine.Random.Range(0, trapPrefabs.Length);
            GameObject trapPrefab = trapPrefabs[trap];

            (Instantiate(trapPrefab, trapPlace.transform.position + new Vector3(0, 0.25f, 0), trapPrefab.transform.rotation) as GameObject).transform.parent = trapPlace.transform;

            trapPlaces.RemoveAt(index);
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
	
    private bool collideWithOtherRoom(Bounds bounds)
    {
        foreach (GameObject room in rooms)
        {
            if (bounds.Intersects(room.GetComponent<BoxCollider>().bounds))
                return true;
        }
        return false;
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
        rooms.Add(firstGO);
        collectTrapsAndGoals(firstGO);
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

    private void collectTrapsAndGoals(GameObject go)
    {
        foreach (Transform child in go.transform)
        {
            if (child.CompareTag("TrapPlace"))
            {
                trapPlaces.Add(child.gameObject);
            }
            else if (child.CompareTag("GoalPlace"))
            {
                goalPlaces.Add(child.gameObject);
            }
            else if (child.CompareTag("StartPlace"))
            {
                startPlaces.Add(child.gameObject);
            }
        }
    }

}
