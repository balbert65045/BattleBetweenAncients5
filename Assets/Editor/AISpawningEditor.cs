//using UnityEngine;
//using UnityEditor;

//[CustomEditor(typeof(AISpawnSystem))]
//[CanEditMultipleObjects]


//public class AISpawningEditor : Editor {

//    public SerializedProperty
//        SpawnTurns_Prop,
//        SpawnObjects_Prop,
//        data;

//    AISpawnSystem aiSpawnSystem;

//    bool visible1;
//    bool visible2;

//    // Use this for initialization
//    void OnEnable () {
//        //SpawnTurns_Prop = serializedObject.FindProperty("SpawnTurns");
//        //SpawnObjects_Prop = serializedObject.FindProperty("SpawnObjects");
//        //data = SpawnObjects_Prop.FindPropertyRelative("rows");
     
//        //aiSpawnSystem = (AISpawnSystem)target;
//    }

//    // Update is called once per frame
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
        
//        base.OnInspectorGUI();

//        //EditorGUI.BeginChangeCheck();

//        //visible1 = EditorGUILayout.Foldout(visible1, SpawnTurns_Prop.name);
//        //if (visible1)
//        //{
//        //    EditorGUI.indentLevel++;
//        //    SerializedProperty arraySizeProp = SpawnTurns_Prop.FindPropertyRelative("Array.size");
//        //    EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("# of Spawns"));
//        //    ShowArrayProperty(SpawnTurns_Prop);
//        //    EditorGUI.indentLevel--;
//        //}
//        //int NumberofSpawnTurns = SpawnTurns_Prop.arraySize;
//        //aiSpawnSystem.SpawnObjects.rows = new ArrayLayout.rowData[NumberofSpawnTurns];

//        //visible2 = EditorGUILayout.Foldout(visible2, SpawnObjects_Prop.name);
//        //if (visible2)
//        //{

//        //    for (int j = 0; j < NumberofSpawnTurns; j++)
//        //    {
//        //        SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
             
//        //        EditorGUILayout.PropertyField(row, new GUIContent("Spawn" + (j+1)));
//        //        // EditorGUILayout.PropertyField(row.FindPropertyRelative("Array.size"));

//        //        //  Debug.Log(row.arraySize);
//        //        if (row.arraySize != 3)
//        //        {
//        //            row.arraySize = 3;
//        //        }

//        //        //   EditorGUILayout.PropertyField(arraySizeProp2, new GUIContent("# of Units"), true);
//        //        //   row.arraySize = arraySizeProp2.intValue;

//        //        for (int i = 0; i < row.arraySize; i++)
//        //        {
//        //            EditorGUILayout.PropertyField(row.GetArrayElementAtIndex(i));
//        //        }

//        //    }

//        //}
//        //    //isShowing = EditorGUILayout.Foldout(isShowing, "SpawnObjects_Prop");
//        //    //if (isShowing)
//        //    //{
//        //    //    for (int x = 1; x <= NumberofSpawnTurns; x++)
//        //    //    {
//        //    //        EditorGUILayout.BeginVertical();
//        //    //        for (int y = 1; y < 3, y++)
//        //    //        {

//        //    //        }
//        //    //        EditorGUILayout.PropertyField(SpawnObjects_Prop, new GUIContent("SpawnObjects " + i), true);
//        //    //    }
//        //    //}


//            //if (EditorGUI.EndChangeCheck())
//            //serializedObject.ApplyModifiedProperties();
      

//        //EditorGUILayout.PropertyField(SpawnObjects_Prop);
//        serializedObject.ApplyModifiedProperties();


//    }

//    //public void ShowArrayProperty2(SerializedProperty list)
//    //{
//    //    EditorGUILayout.PropertyField(list);

//    //    EditorGUI.indentLevel += 1;
//    //    for (int i = 0; i < list.arraySize; i++)
//    //    {
//    //        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
//    //        new GUIContent("Unit " + (i + 1).ToString()));
//    //    }
//    //    EditorGUI.indentLevel -= 1;
//    //}


//    //public void ShowArrayProperty(SerializedProperty list)
//    //{
//    //    EditorGUILayout.PropertyField(list);

//    //    EditorGUI.indentLevel += 1;
//    //    for (int i = 0; i < list.arraySize; i++)
//    //    {
//    //        EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i),
//    //        new GUIContent("SpawnTurn " + (i + 1).ToString()));
//    //    }
//    //    EditorGUI.indentLevel -= 1;
//    //}

//}
