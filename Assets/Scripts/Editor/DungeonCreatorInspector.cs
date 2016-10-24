using System;
using System.Collections;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonCreator))]
public class DungeonCreatorInspector : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		DungeonCreator dungeonCreator = GameObject.Find("Dungeon Creator").GetComponent<DungeonCreator>();

		if (GUILayout.Button("Generate"))
		{
			dungeonCreator.Generate();
		}
		
		if (GUILayout.Button("Remove All"))
		{
			dungeonCreator.RemoveAll();
		}
	}

}
