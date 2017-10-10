using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AISpawnSystem))]
[CanEditMultipleObjects]


public class AISpawningEditor : Editor
{

    public SerializedProperty
        TileOn_Prop,
        Spawns_Prop;


   AISpawnSystem aiSpawnSystem;

   bool visible;
   List<bool> ObjectsVisible;


    //    // Use this for initialization
    void OnEnable()
    {
        Spawns_Prop = serializedObject.FindProperty("Spawns");
        TileOn_Prop = serializedObject.FindProperty("TileOn");
        ObjectsVisible = new List<bool>();
        aiSpawnSystem = (AISpawnSystem)target;
    }

    //    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(TileOn_Prop);
        visible = EditorGUILayout.Foldout(visible, Spawns_Prop.name);
        if (visible)
        {
            EditorGUI.indentLevel++;
            SerializedProperty arraySizeProp = Spawns_Prop.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("# of Spawns"));
            if (ObjectsVisible.Count != Spawns_Prop.arraySize)
            {
                ObjectsVisible.Clear();
                for (int i = 0; i < Spawns_Prop.arraySize; i++)
                {
                    ObjectsVisible.Add(new bool());
                }
            }

            for (int i = 0; i< Spawns_Prop.arraySize; i++)
            {
                EditorGUI.indentLevel++;
                ObjectsVisible[i] = EditorGUILayout.Foldout(ObjectsVisible[i], "Spawn " + i);

                if (ObjectsVisible[i])
                {
                    SerializedProperty Time_Prop = Spawns_Prop.GetArrayElementAtIndex(i).FindPropertyRelative("Time");
                    SerializedProperty Objects_Prop = Spawns_Prop.GetArrayElementAtIndex(i).FindPropertyRelative("Objects");
                    EditorGUILayout.PropertyField(Time_Prop);
                    SerializedProperty arrayObjectSizeProp = Objects_Prop.FindPropertyRelative("Array.size");
                    EditorGUILayout.PropertyField(arrayObjectSizeProp, new GUIContent("# of Objects"));
                    EditorGUI.indentLevel++;
                    for (int j = 0; j < Objects_Prop.arraySize; j++)
                    {
                        SerializedProperty Object_Prop = Objects_Prop.GetArrayElementAtIndex(j);
                        EditorGUILayout.PropertyField(Object_Prop, new GUIContent("Object " + j));
                    }
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }


        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        serializedObject.ApplyModifiedProperties();
    }

 }
