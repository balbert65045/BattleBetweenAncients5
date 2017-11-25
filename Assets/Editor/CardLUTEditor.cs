using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CardLUT))]
[CanEditMultipleObjects]


public class CardLUTEditor : Editor
{
    public SerializedProperty
        SummonCards_Prop,
        SummonCardsActive_Prop,
        SpellCards_Prop,
        SpellCardsActive_Prop;


    CardLUT cardLUT;

    bool visibleSummon;
    bool visibleSpell;
    List<bool> SummonsVisible;
    List<bool> SpellsVisible;

    private void OnEnable()
    {
        SummonCards_Prop = serializedObject.FindProperty("SummonCards");
        SummonCardsActive_Prop = serializedObject.FindProperty("SummonCardsActive");

        SpellCards_Prop = serializedObject.FindProperty("SpellCards");
        SpellCardsActive_Prop = serializedObject.FindProperty("SpellCardsActive");

        SummonsVisible = new List<bool>();
        SpellsVisible = new List<bool>();
        cardLUT = (CardLUT)target;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        visibleSummon = EditorGUILayout.Foldout(visibleSummon, SummonCardsActive_Prop.name);
        if (visibleSummon)
        {
            EditorGUI.indentLevel++;
            SerializedProperty arraySizeProp = SummonCardsActive_Prop.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("# of SummonCards"));
            SerializedProperty arraySizeSummonProp = SummonCards_Prop.FindPropertyRelative("Array.size");
            SummonCards_Prop.arraySize = SummonCardsActive_Prop.arraySize;

            if (SummonsVisible.Count != SummonCardsActive_Prop.arraySize)
            {
                SummonsVisible.Clear();
                for (int i = 0; i < SummonCardsActive_Prop.arraySize; i++)
                {
                    SummonsVisible.Add(new bool());
                }
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < SummonCardsActive_Prop.arraySize; i++)
            {
                GUILayout.BeginHorizontal("box");
                SerializedProperty bool_Prop = SummonCardsActive_Prop.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(bool_Prop, new GUIContent("Card  " + i + " active"));

                SerializedProperty Summon_Prop = SummonCards_Prop.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(Summon_Prop, new GUIContent(""));

                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        visibleSpell = EditorGUILayout.Foldout(visibleSpell, SpellCardsActive_Prop.name);
        if (visibleSpell)
        {
            EditorGUI.indentLevel++;
            SerializedProperty arraySizeProp = SpellCardsActive_Prop.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProp, new GUIContent("# of SummonCards"));
            SerializedProperty arraySizeSpellProp = SpellCardsActive_Prop.FindPropertyRelative("Array.size");
            SpellCards_Prop.arraySize = SpellCardsActive_Prop.arraySize;

            if (SpellsVisible.Count != SpellCardsActive_Prop.arraySize)
            {
                SpellsVisible.Clear();
                for (int i = 0; i < SpellCardsActive_Prop.arraySize; i++)
                {
                    SpellsVisible.Add(new bool());
                }
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < SpellCardsActive_Prop.arraySize; i++)
            {
                GUILayout.BeginHorizontal("box");
                SerializedProperty bool_Prop = SpellCardsActive_Prop.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(bool_Prop, new GUIContent("Card  " + i + " active"));

                SerializedProperty Spell_Prop = SpellCards_Prop.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(Spell_Prop, new GUIContent(""));

                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }



        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();
    }

 }

