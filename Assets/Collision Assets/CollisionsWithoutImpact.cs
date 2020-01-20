﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script works as a fix for the weird twitching issue 
 * that occurs when the submarine collider hits one of the
 * lerp stop planes in the tunnel prefabs. Therefore I used
 * a new object that is constantly being set to the submarine's
 * position. This way the camera keeps still, but collisions
 * are still detected.
 */

public class CollisionsWithoutImpact : MonoBehaviour
{
  [SerializeField]
  GameObject submarine;

  [HideInInspector]
  public static Vector3 forward;

  Vector3 position;

  void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "LerpStopIn")
    {
      forward = collision.gameObject.transform.parent.transform.forward;
      position = collision.gameObject.transform.position;

      submarine.GetComponent<SubmarineController>().turnCamStraight = false;
      Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
      return;
    }
    else if (collision.gameObject.tag == "LerpStopOut")
    {
      if (collision.gameObject.transform.parent.GetComponent<MeshGenerator>() != null)
      {
        forward = collision.gameObject.transform.parent.GetComponent<MeshGenerator>().next.gameObject.transform.forward;
        position = collision.gameObject.transform.position;
      }

      submarine.GetComponent<SubmarineController>().turnCamStraight = false;
      Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
      return;
    }
  }

  void Update()
  {
    Debug.DrawRay(position, forward);

    transform.position = submarine.transform.position;
    transform.forward = submarine.transform.forward;
  }
}
