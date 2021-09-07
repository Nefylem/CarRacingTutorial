using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] public CarSettings settings;
    [SerializeField] public AxleObject[] axles;

    public WheelCollider[] wheelCollider;
    public Transform[] wheels;

    public bool showSetup;

    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var accelerate = 0f;
        var steering = 0f;

        if (Input.GetKey(KeyCode.W)) accelerate = 1f;
        if (Input.GetKey(KeyCode.A)) steering = -1f;
        if (Input.GetKey(KeyCode.D)) steering = 1f;

        for (int w = 0; w < 2; ++w)
        {
            wheelCollider[w].motorTorque = accelerate * 1000;
            wheelCollider[w].steerAngle = steering * 30f;
        }

        for (int w = 0; w < 4; ++w)
        {
            wheelCollider[w].GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheels[w].position = position;
            wheels[w].rotation = rotation;
        }
    }

}
