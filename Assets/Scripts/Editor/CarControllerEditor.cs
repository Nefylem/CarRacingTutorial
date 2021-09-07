using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CarController))]
public class CarControllerEditor : Editor
{
    private CarController car;
    private CarSetup setup;



    private int currentDriveIndex;
    private int currentBrakeIndex;
    private int currentAntiRollIndex;

    private string[] driveOptions;
    private string[] brakes;
    private string[] antiRoll;

    private void OnEnable()
    {
        car = ((CarController)target);
        driveOptions = new string[] { "Front", "Rear", "Both" };
        brakes = new string[] { "Front", "Rear", "Both" };
        antiRoll = new string[] { "None", "Front", "Rear", "Both" };

        if (setup == null) setup = new CarSetup(car);
    }

    public override void OnInspectorGUI()
    {
        if (car.showSetup)
        {
            if (GUILayout.Button("Back")) car.showSetup = false;
            ShowSetup();
        }
        else
        {
            if (GUILayout.Button("Setup")) car.showSetup = true;
            base.OnInspectorGUI();
        }
    }

    void ShowSetup()
    {
        GUILayout.Space(10);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Front Left");
        var leftFront = (Transform)EditorGUILayout.ObjectField(GetTransform(0, 0), typeof(Transform), true, GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.Space(50);

        GUILayout.BeginVertical();
        GUILayout.Label("Front Right ");
        var rightFront = (Transform)EditorGUILayout.ObjectField(GetTransform(0, 1), typeof(Transform), true, GUILayout.Height(25));
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Front Left");
        var leftRear = (Transform)EditorGUILayout.ObjectField(GetTransform(1, 0), typeof(Transform), true, GUILayout.Height(25));
        GUILayout.EndVertical();
        GUILayout.Space(50);

        GUILayout.BeginVertical();
        GUILayout.Label("Front Right ");
        var rightRear = (Transform)EditorGUILayout.ObjectField(GetTransform(1, 1), typeof(Transform), true, GUILayout.Height(25));
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        var driveIndex = EditorGUILayout.Popup("Drive Type", currentDriveIndex, driveOptions);
        var brakeIndex = EditorGUILayout.Popup("Brakes", currentBrakeIndex, brakes);
        var antiRollIndex = EditorGUILayout.Popup("Sway Bar", currentAntiRollIndex, antiRoll);

        GUILayout.Space(10);
        if (GUILayout.Button("Add Rigidbody")) setup.AddRigidbody();

        if (EditorGUI.EndChangeCheck())
        {
            setup.SetupWheels(leftFront, rightFront, leftRear, rightRear);
            if (driveIndex != currentDriveIndex) { setup.SetDrive(driveIndex); currentDriveIndex = driveIndex; }
            if (brakeIndex != currentBrakeIndex) { setup.SetBrakes(brakeIndex); currentBrakeIndex = brakeIndex; }
            if (antiRollIndex  != currentAntiRollIndex) { setup.SetupAntiRollIndex(antiRollIndex); currentAntiRollIndex = antiRollIndex; }
        }
    }

    private Transform GetTransform(int a, int w)
    {
        if (car.axles == null || car.axles.Length <= a) return null;
        if (car.axles[a].wheels == null || car.axles[a].wheels.Length < w) return null;

        return car.axles[a].wheels[w].wheelMesh;
    }
}
