using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mapgenerator))]

public class MapGeneratorEditor : UnityEditor.Editor {

    public override void OnInspectorGUI()
    {
        Mapgenerator mapGen = (Mapgenerator)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
        if (GUILayout.Button("Generator"))
        {
            mapGen.GenerateMap();
        }
    }



}
