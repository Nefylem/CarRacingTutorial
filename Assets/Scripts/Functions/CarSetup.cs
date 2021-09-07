using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSetup 
{
    private CarController home;
    private GameObject target;
    private Transform colliderParent;

    public CarSetup(CarController home)
    {
        this.home = home;
        target = home.gameObject;
    }

    public void SetupWheels(Transform leftFront, Transform rightFront, Transform leftRear, Transform rightRear)
    {
        SetupAxles();
        SetupColliderParent();

        if (leftFront != home.axles[0].wheels[0].wheelMesh &&!leftFront.name.Contains("Collider"))
        {
            home.axles[0].wheels[0].wheelMesh = leftFront;
            home.axles[0].wheels[0].wheelCollider = SetupWheelCollider(leftFront);
        }

        if (rightFront != home.axles[0].wheels[1].wheelMesh && !rightFront.name.Contains("Collider"))
        {
            home.axles[0].wheels[1].wheelMesh = rightFront;
            home.axles[0].wheels[1].wheelCollider = SetupWheelCollider(rightFront);
        }

        if (leftRear != home.axles[1].wheels[0].wheelMesh && !leftRear.name.Contains("Collider"))
        {
            home.axles[1].wheels[0].wheelMesh = leftRear;
            home.axles[1].wheels[1].wheelCollider = SetupWheelCollider(leftRear);
        }

        if (rightRear != home.axles[1].wheels[1].wheelMesh && !rightRear.name.Contains("Collider"))
        {
            home.axles[1].wheels[1].wheelMesh = rightRear;
            home.axles[1].wheels[1].wheelCollider = SetupWheelCollider(rightRear);
        }
    }

    private void SetupAxles()
    {
        if (home.axles == null || home.axles.Length < 2)
        {
            home.axles = new AxleObject[2];

            home.axles[0].drive = true;
            home.axles[0].steer = true;
            home.axles[0].brakes = true;
        }

        for (int a = 0; a < 2; ++a)
        {
            if (home.axles[a].wheels == null || home.axles[a].wheels.Length < 2)
            {
                home.axles[a].wheels = new WheelObject[2];
                home.axles[a].leftWheel = 0;
                home.axles[a].rightWheel = 0;
            }
        }
    }

    private void SetupColliderParent()
    {
        if (colliderParent != null) return;

        colliderParent = target.transform.Find("Wheel Colliders");
        if (colliderParent != null) return;

        colliderParent = new GameObject("Wheel Colliders").transform;
        colliderParent.SetParent(target.transform);
    }


    private WheelCollider SetupWheelCollider(Transform wheel)
    {
        var wheelCollider = colliderParent.Find($"{wheel.name} collider")?.GetComponent<WheelCollider>();
        if (wheelCollider == null)
        {
            var wheelObj = colliderParent.Find($"{wheel.name} collider");
            if (wheelObj == null) wheelObj = new GameObject($"{wheel.name} collider").transform;

            wheelObj.SetParent(colliderParent);
            wheelObj.position = wheel.position;

            wheelCollider = wheelObj.gameObject.AddComponent<WheelCollider>();
        }

        var radius = 0f;
        if (wheel.childCount == 0)
        {
            radius = CalculateWheelRadius(wheel);
        }
        else 
        { 
            foreach (Transform child in wheel)
            {
                var newRadius = CalculateWheelRadius(child);
                if (newRadius > radius)
                    radius = newRadius;
            }
        }

        wheelCollider.radius = radius;
        wheelCollider.center = new Vector3(0, wheel.transform.localPosition.y * 0.5f, 0);

        return wheelCollider;
    }

    private float CalculateWheelRadius(Transform wheel)
    {
        var meshFilter = wheel.GetComponent<MeshFilter>();
        if (meshFilter == null) return 0f;

        var bounds = meshFilter.sharedMesh.bounds;

        float x = bounds.extents.x * wheel.lossyScale.x;
        float y = bounds.extents.y * wheel.lossyScale.y;
        float z = bounds.extents.z * wheel.lossyScale.z;

        if (x < y && x < z) return FindRadius(y, z);
        if (y < x && y < z) return FindRadius(x, z);
        return FindRadius(x, y);

    }

    private float FindRadius(float x, float y)
    {
        if (x > y) return x;
        return y;
    }

    public void SetDrive(int index)
    {
        SetupAxles();

        for (int a = 0; a < 2; ++a)
        {
            home.axles[a].drive = a == index || index == 2;
        }
    }

    public void SetBrakes(int index)
    {
        SetupAxles();

        for (int a = 0; a < 2; ++a)
        {
            home.axles[a].brakes = a == index || index == 2;
        }
    }

    public void SetupAntiRollIndex(int index)
    {
        SetupAxles();

        for (int a = 0; a < 2; ++a)
        {
            if (index == 0)
            {
                home.axles[a].antiRoll = false;
                continue;
            }
            home.axles[a].antiRoll = a == (index + 1) || index == 3;
        }
    }

    public void AddRigidbody()
    {
        home.rb = target.GetComponent<Rigidbody>();
        if (home.rb == null) home.rb = target.gameObject.AddComponent<Rigidbody>();

        home.rb.mass = home.settings.mass;
        home.rb.drag = 0.001f;
        home.rb.angularDrag = 0.05f;
    }
}
