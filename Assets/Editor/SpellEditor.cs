using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CardSpell))]
[CanEditMultipleObjects]


public class SpellEditor : Editor
{

    public SerializedProperty
        Modifier_Prop,
        ModifierDuration_Prop,
        HealAmount_Prop,
        spellType_Prop,
        SpellAtribute_Prop;

    CardSpell cardSpell;

    private void OnEnable()
    {
        Modifier_Prop = serializedObject.FindProperty("Modifier");
        ModifierDuration_Prop = serializedObject.FindProperty("ModifierDuration");

        HealAmount_Prop = serializedObject.FindProperty("HealAmount");

        spellType_Prop = serializedObject.FindProperty("spellType");
        SpellAtribute_Prop = serializedObject.FindProperty("spellAtribute");

        cardSpell = (CardSpell)target;

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(spellType_Prop);
        EditorGUILayout.PropertyField(SpellAtribute_Prop);
        

        CardSpell.SpellAtribute spellAtribute = (CardSpell.SpellAtribute)SpellAtribute_Prop.enumValueIndex + 1;

        switch (spellAtribute)
        {
            case CardSpell.SpellAtribute.Movement:
                EditorGUILayout.PropertyField(Modifier_Prop , new GUIContent("Move Modifier"));
                EditorGUILayout.PropertyField(ModifierDuration_Prop, new GUIContent("Move Modifier Duration"));
                break;
            //case CardSpell.SpellAtribute.Range:
            //    EditorGUILayout.PropertyField(Modifier_Prop, new GUIContent("Range Modifier"));
            //    EditorGUILayout.PropertyField(ModifierDuration_Prop, new GUIContent("Range Modifier Duration"));
            //    break;
            case CardSpell.SpellAtribute.Damage:
                EditorGUILayout.PropertyField(Modifier_Prop, new GUIContent("Damage Modifier"));
                EditorGUILayout.PropertyField(ModifierDuration_Prop, new GUIContent("Damage Modifier Duration"));
                break;
            case CardSpell.SpellAtribute.Heal:
                EditorGUILayout.PropertyField(HealAmount_Prop);
                break;

        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();

    }
}
