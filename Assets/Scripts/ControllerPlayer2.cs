﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer2 : MonoBehaviour
{
  [HideInInspector]
  public Quaternion rotation = new Quaternion();
  [HideInInspector]
  public Vector3 acceleration = new Vector2();

  [SerializeField]
  GameObject submarine;
  [SerializeField]
  GameObject headLight;
  [SerializeField]
  float verticalSpeed;
  [SerializeField]
  float headLightSpeed;

  [HideInInspector]
  public Vector2 joystick;
  [HideInInspector]
  public bool actionPressed;

  void Start()
  {
        
  }


  void Update()
  {
    Vector3 dir = Vector3.zero;

    dir.z = acceleration.y;

    if (dir.sqrMagnitude > 1)
      dir.Normalize();

    submarine.transform.Translate(dir * verticalSpeed * Time.deltaTime);

    Vector3 lightDir = Vector3.zero;
    lightDir.x = -joystick.y;
    lightDir.y = joystick.x;

    headLight.transform.Rotate(lightDir * headLightSpeed * Time.deltaTime);
  }
}
