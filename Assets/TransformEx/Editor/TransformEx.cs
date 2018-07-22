/* 
 * TransformEx 1.1
 * 
 * This script was made by Jason Peterson (DarkAkuma) of http://darkakuma.z-net.us/ 
*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects, CustomEditor(typeof(Transform))]
public class TransformEx : Editor
{    
    // 
    private static GUIContent positionLabelText = new GUIContent("Position", "The local position of this Game Object relative to the parent, in Unity Units.");
    private static GUIContent rotationLabelText = new GUIContent("Rotation", "The local rotation of this Game Object relative to the parent.");
    private static GUIContent scaleLabelText = new GUIContent("Scale", "The local scaling of this Game Object relative to the parent.");

    private static GUIContent worldPositionLabelText = new GUIContent("World Positions", "The world position of this object, in real units of measurment.");

    private static GUIContent positionMillimetersLabelText = new GUIContent("Millimeters", "The world position of this Game Object relative to the parent, in Millimeters.");
    private static GUIContent positionCentimetersLabelText = new GUIContent("Centimeters", "The world position of this Game Object relative to the parent, in Centimeters.");
    private static GUIContent positionInchesLabelText = new GUIContent("Inches", "The world position of this Game Object relative to the parent, in Inches.");
    private static GUIContent positionFeetLabelText = new GUIContent("Feet", "The world position of this Game Object relative to the parent, in Feet.");
    private static GUIContent positionMetersLabelText = new GUIContent("Meters", "The world position of this Game Object relative to the parent, in Meters. Also effectivly Unity units.");
    private static GUIContent positionYardsLabelText = new GUIContent("Yards", "The world position of this Game Object relative to the parent, in Yards.");
    private static GUIContent positionKilometersLabelText = new GUIContent("Kilometers", "The world position of this Game Object relative to the parent, in Kilometers.");
    private static GUIContent positionMilesLabelText = new GUIContent("Miles", "The world position of this Game Object relative to the parent, in Miles.");

    private static GUIContent meshSizeLabelText = new GUIContent("Mesh Size", "The size of the mesh(es) under this Game Object, as calculated at their maximum bounds. Given in Unity units. \r\n\r\nClick the arrow to the left to see real world units of measurment and boundry lines in the Scene view.");

    private static GUIContent realWorldMeasurmentsLabelText = new GUIContent("Real World Measurments", "The size of this Game Object's mesh(es) in real units of measurment.");

    private static GUIContent millimetersLabelText = new GUIContent("Millimeters", "The size of the Game Object in Millimeters, based on world scaling.");
    private static GUIContent centimetersLabelText = new GUIContent("Centimeters", "The size of the Game Object in Centimeters, based on world scaling.");
    private static GUIContent inchesLabelText = new GUIContent("Inches", "The size of the Game Object in Inches, based on world scaling.");
    private static GUIContent feetLabelText = new GUIContent("Feet", "The size of the Game Object in Feet, based on world scaling.");
    private static GUIContent metersLabelText = new GUIContent("Meters", "The size of the Game Object in Meters, based on world scaling.");
    private static GUIContent yardsLabelText = new GUIContent("Yards", "The size of the Game Object in Yards, based on world scaling.");
    private static GUIContent kilometersLabelText = new GUIContent("Kilometers", "The size of the Game Object in Kilometers, based on world scaling.");
    private static GUIContent milesLabelText = new GUIContent("Miles", "The size of the Game Object in Miles, based on world scaling.");

    private SerializedProperty positionProperty;
    private SerializedProperty rotationProperty;
    private SerializedProperty scaleProperty;

    private Transform transform;
    private List<Transform> transforms = new List<Transform>();

    static private bool isPositionListVisible = false;
    static private bool isMeshSizeListVisible = false;
        
    public void OnEnable()
    {
        // It's just nifty having access to the transform in a familiar variable.
        transform = (Transform)target;        
                
        // Again, it's nice having access to our list of selected transforms, in a list.
        foreach (UnityEngine.Object obj in (UnityEngine.Object[])targets)
            transforms.Add((Transform)obj);

        // Get our normal properties ready.
        positionProperty = serializedObject.FindProperty("m_LocalPosition");
        rotationProperty = serializedObject.FindProperty("m_LocalRotation");
        scaleProperty = serializedObject.FindProperty("m_LocalScale");
    }

    public override void OnInspectorGUI()
    {
        // Need to our sync serializedObject first.
        serializedObject.Update();
        
        // Position        
        PositionPropertyField(positionProperty, positionLabelText);        
        
        // Rotation
        RotationPropertyField(rotationProperty, rotationLabelText);
        
        // Scale
        ScalePropertyField(scaleProperty, scaleLabelText);

        // Real Measurment Mesh Sizes
        MeshSizePropertyFields();

        // Apply our changes.
        serializedObject.ApplyModifiedProperties();
    }

    private void PositionPropertyField(SerializedProperty property, GUIContent labelText)
    {
        // We're combining the position property and foldout property onto the same line.

        // We draw the position property first so it isnt included inside of the foldout.        
        EditorGUILayout.PropertyField(property, labelText);
                
        // Draw our little arrow for a drop down list of the position represented in other units of measurment.        
        isPositionListVisible = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), isPositionListVisible, new GUIContent(""));

        // Only draw our extra position fields if the user clicks the little arrow.
        if (isPositionListVisible)
        {
            // If multiple objects are selected, we have to compare their values and not show any value at all if they are different.
            foreach (Transform t in transforms)
            {
                if (t.position != transform.position)
                {
                    EditorGUI.showMixedValue = true;
                    break;
                }
            }

            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField(worldPositionLabelText, EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
                
            // Show our custom real unit positions.
            Vector3 currentMillimeters = EditorGUILayout.Vector3Field(positionMillimetersLabelText, transform.TransformEx().positionInMillimeters);
            Vector3 currentCentimeters = EditorGUILayout.Vector3Field(positionCentimetersLabelText, transform.TransformEx().positionInCentimeters);
            Vector3 currentInches = EditorGUILayout.Vector3Field(positionInchesLabelText, transform.TransformEx().positionInInches);
            Vector3 currentFeet = EditorGUILayout.Vector3Field(positionFeetLabelText, transform.TransformEx().positionInFeet);
            Vector3 currentMeters = EditorGUILayout.Vector3Field(positionMetersLabelText, transform.TransformEx().positionInMeters);
            Vector3 currentYards = EditorGUILayout.Vector3Field(positionYardsLabelText, transform.TransformEx().positionInYards);
            Vector3 currentKilometers = EditorGUILayout.Vector3Field(positionKilometersLabelText, transform.TransformEx().positionInKilometers);
            Vector3 currentMiles = EditorGUILayout.Vector3Field(positionMilesLabelText, transform.TransformEx().positionInMiles);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "New Position");

                // Apply any changes to the transforms of all selected objects.
                foreach (Transform t in transforms)
                {
                    // Change our transforms positions.
                    if (currentMillimeters != t.TransformEx().positionInMillimeters)         t.TransformEx().positionInMillimeters = currentMillimeters;
                    else if (currentCentimeters != t.TransformEx().positionInCentimeters)    t.TransformEx().positionInCentimeters = currentCentimeters;
                    else if (currentInches != t.TransformEx().positionInInches)              t.TransformEx().positionInInches = currentInches;
                    else if (currentFeet != t.TransformEx().positionInFeet)                  t.TransformEx().positionInFeet = currentFeet;
                    else if (currentMeters != t.TransformEx().positionInMeters)              t.TransformEx().positionInMeters = currentMeters;
                    else if (currentYards != t.TransformEx().positionInYards)                t.TransformEx().positionInYards = currentYards;
                    else if (currentKilometers != t.TransformEx().positionInKilometers)      t.TransformEx().positionInKilometers = currentKilometers;
                    else if (currentMiles != t.TransformEx().positionInMiles)                t.TransformEx().positionInMiles = currentMiles;                    
                }
            }

            EditorGUILayout.Separator();

            EditorGUI.indentLevel--;

            EditorGUI.showMixedValue = false;
        }
    }
    
    private void RotationPropertyField(SerializedProperty property, GUIContent labelText)
    {
        // If multiple objects are selected, we have to compare their values and not show any value at all if they are different.
        foreach (Transform t in transforms)
        {
            if (t.rotation != transform.rotation)
            {
                EditorGUI.showMixedValue = true;
                break;
            }
        }

        EditorGUI.BeginChangeCheck();

        // We don't use a EditorGUI.PropertyField so we can get back the new rotation if the user changes it.
        Vector3 currentRotation = EditorGUILayout.Vector3Field(labelText, transform.localRotation.eulerAngles);
        
        if (EditorGUI.EndChangeCheck())
        {   
            Undo.RecordObjects(targets, "New Rotation");

            // Set the rotations of all selected transforms to currentRotation.
            foreach (Transform t in transforms)
                t.localEulerAngles = currentRotation;

            property.serializedObject.SetIsDifferentCacheDirty();
        }

        EditorGUI.showMixedValue = false;
    }

    private void ScalePropertyField(SerializedProperty property, GUIContent labelText)
    {
        EditorGUI.BeginChangeCheck();

        // Draw our scale property.
        EditorGUILayout.PropertyField(property, labelText);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(targets, "New Scale");

            // Update it ourselves, just incase... but probably not needed.
            transform.localScale = scaleProperty.vector3Value;            
        }        
    }

    private void MeshSizePropertyFields()
    {
        // We check the object and its children for MeshRenderers, and only display mesh size properties if any are found.
        // We also only show mesh size properties if a single object is selected.        
        if (HasMeshRenders())
        {
            EditorGUILayout.Separator();

            EditorGUI.BeginChangeCheck();

            // We're combining the Mesh Size property and foldout property onto the same line.

            // We draw the Mesh Size property first so it isnt included inside of the foldout.            
            Vector3 currentUnityUnits = EditorGUILayout.Vector3Field(meshSizeLabelText, transform.TransformEx().sizeInMeters);

            // Draw our little arrow for a drop down list of the mesh size represented in the various units of measurment.
            bool isMeshSizeListVisible_ = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), isMeshSizeListVisible, new GUIContent(""));

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "New Size");

                       // Apply any changes to the transforms of all selected objects.
                foreach (Transform t in transforms)
                {
                    // Changes our transforms scale.
                    if (currentUnityUnits != t.TransformEx().sizeInMeters) t.TransformEx().sizeInMeters = currentUnityUnits;
                }
            }

            // Only draw our mesh size fields if the user clicks the little arrow.
            if (isMeshSizeListVisible_)
            {
                // If multiple objects are selected, we have to compare their values and not show any value at all if they are different.
                foreach (Transform t in transforms)
                {
                    // We only compare 1 value since they all represent the same physical size. Meters.... because why not? Its basically a Unity unit, so standard.
                    if (t.TransformEx().sizeInMeters != transform.TransformEx().sizeInMeters)
                    {
                        EditorGUI.showMixedValue = true;
                        break;
                    }
                }

                EditorGUI.indentLevel++;

                EditorGUILayout.LabelField(realWorldMeasurmentsLabelText, EditorStyles.boldLabel);                

                EditorGUI.BeginChangeCheck();                

                // Draw our Real measurment properties.
                Vector3 currentMillimeters = EditorGUILayout.Vector3Field(millimetersLabelText, transform.TransformEx().sizeInMillimeters);
                Vector3 currentCentimeters = EditorGUILayout.Vector3Field(centimetersLabelText, transform.TransformEx().sizeInCentimeters);
                Vector3 currentInches = EditorGUILayout.Vector3Field(inchesLabelText, transform.TransformEx().sizeInInches);
                Vector3 currentFeet = EditorGUILayout.Vector3Field(feetLabelText, transform.TransformEx().sizeInFeet);
                Vector3 currentMeters = EditorGUILayout.Vector3Field(metersLabelText, transform.TransformEx().sizeInMeters);
                Vector3 currentYards = EditorGUILayout.Vector3Field(yardsLabelText, transform.TransformEx().sizeInYards);
                Vector3 currentKilometers = EditorGUILayout.Vector3Field(kilometersLabelText, transform.TransformEx().sizeInKilometers);
                Vector3 currentMiles = EditorGUILayout.Vector3Field(milesLabelText, transform.TransformEx().sizeInMiles);                

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObjects(targets, "New Size");

                    // Apply any changes to the transforms of all selected objects.
                    foreach (Transform t in transforms)
                    {
                        // Changes our transforms scale.
                        if (currentMillimeters != t.TransformEx().sizeInMillimeters)        t.TransformEx().sizeInMillimeters = currentMillimeters;
                        else if (currentCentimeters != t.TransformEx().sizeInCentimeters)   t.TransformEx().sizeInCentimeters = currentCentimeters;
                        else if (currentInches != t.TransformEx().sizeInInches)             t.TransformEx().sizeInInches = currentInches;
                        else if (currentFeet != t.TransformEx().sizeInFeet)                 t.TransformEx().sizeInFeet = currentFeet;
                        else if (currentMeters != t.TransformEx().sizeInMeters)             t.TransformEx().sizeInMeters = currentMeters;
                        else if (currentYards != t.TransformEx().sizeInYards)               t.TransformEx().sizeInYards = currentYards;
                        else if (currentKilometers != t.TransformEx().sizeInKilometers)     t.TransformEx().sizeInKilometers = currentKilometers;
                        else if (currentMiles != t.TransformEx().sizeInMiles)               t.TransformEx().sizeInMiles = currentMiles;
                        else if (currentUnityUnits != t.TransformEx().sizeInMeters)         t.TransformEx().sizeInMeters = currentUnityUnits;
                        
                    }                    

                    scaleProperty.serializedObject.SetIsDifferentCacheDirty();
                }

                EditorGUI.indentLevel--;

                EditorGUI.showMixedValue = false;
            }

            // Force the gizmos to be redrawn if the user changed the option.
            if (isMeshSizeListVisible != isMeshSizeListVisible_)
            {
                isMeshSizeListVisible = isMeshSizeListVisible_;
                EditorUtility.SetDirty(target);
            }
        }
    }

    void OnSceneGUI()
    {
        if (Application.isEditor && !Application.isPlaying && isMeshSizeListVisible)
        {
            // We need meshInfo for knowing where the positions of the edges of the mesh(es) are.
            MeshInfo meshInfo = transform.GetMeshInfo();
                        
            Handles.matrix = transform.localToWorldMatrix;
            
            // Draw some grids lines to better visually display the edges of the gameObjects mesh.
            Handles.color = Color.red;
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.CenterPosition().z), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.CenterPosition().z));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.CenterPosition().z), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.CenterPosition().z));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.CenterPosition().y, meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.CenterPosition().y, meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.CenterPosition().y, meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.CenterPosition().y, meshInfo.PositiveEdgeZ()));

            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()));

            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.CenterPosition().z), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.CenterPosition().z));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.CenterPosition().z), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.CenterPosition().z));
            Handles.DrawLine(new Vector3(meshInfo.CenterPosition().x, meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.CenterPosition().x, meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.CenterPosition().x, meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.CenterPosition().x, meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));

            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));

            Handles.color = Color.blue;
            Handles.DrawLine(new Vector3(meshInfo.CenterPosition().x, meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.CenterPosition().x, meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.CenterPosition().x, meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.CenterPosition().x, meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.CenterPosition().y, meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.CenterPosition().y, meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.CenterPosition().y, meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.CenterPosition().y, meshInfo.PositiveEdgeZ()));

            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.NegativeEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.NegativeEdgeY(), meshInfo.PositiveEdgeZ()));
            Handles.DrawLine(new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.NegativeEdgeZ()), new Vector3(meshInfo.PositiveEdgeX(), meshInfo.PositiveEdgeY(), meshInfo.PositiveEdgeZ()));
        }
    }

    /// <summary>
    /// Check this transform and its children for the existance of any mesh renderers.
    /// </summary>
    /// <returns></returns>
    private bool HasMeshRenders()
    {
        Transform[] objTranforms = transform.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform nextTransform in objTranforms)
        {
            if (nextTransform.GetComponent<MeshFilter>())
                return true;                
            else if (nextTransform.GetComponent<SkinnedMeshRenderer>())
                return true;            
        }

        return false;
    }
}