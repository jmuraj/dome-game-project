﻿using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class UDPParser : MonoBehaviour
{
  [SerializeField]
  UDPListener listener;
  [SerializeField]
  GameObject player1;
  [SerializeField]
  GameObject player2;
  [SerializeField]
  bool printToConsole;

  string prevMessageGyro;
  string prevMessageAccelerometer;
  string prevJoystick;

  List<string> localIPs = new List<string>();

  void Start()
  {
    prevMessageGyro = "";
    prevMessageAccelerometer = "";
  }

  void Update()
  {
    if (listener.message.Length > 0)
    {
      // check for { x } - format
      if (listener.message[0] == '{' && listener.message[listener.message.Length - 1] == '}')
      {
        // parse local IP
        string localIP = listener.message.Substring(1, listener.message.IndexOf("}") - 1);

        bool alreadyExists = false;
        int hit = -1;

        for (int i = 0; i < localIPs.Count; i++)
        {
          if (localIPs[i] == localIP)
          {
            hit = i;
            alreadyExists = true;
          }
        }

        // new IPs will be added to the list of connected users
        if (!alreadyExists)
        {
          localIPs.Add(localIP);

          if (printToConsole)
            Debug.Log("Info: UDPParser: Registered new IP: " + localIP);
        }

        if (alreadyExists)
        {
          // parse gyroscope data
          int gyroPos = listener.message.IndexOf("{G(");
          if (gyroPos != -1)
          {
            // check if field has changed
            if (listener.message != prevMessageGyro)
            {
              // cut off everything except the floats
              string gyroData = listener.message.Substring(gyroPos + 3);
              gyroData = gyroData.Substring(0, gyroData.IndexOf("}") - 1);

              string[] quat = gyroData.Split(',');

              if (hit == 0)
              {
                ControllerPlayer1 player = player1.GetComponent<ControllerPlayer1>();
                player.rotation.x = -float.Parse(quat[0], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.z = -float.Parse(quat[1], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.y = -float.Parse(quat[2], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.w = float.Parse(quat[3], CultureInfo.InvariantCulture.NumberFormat);
              }
              else if (hit == 1)
              {
                ControllerPlayer2 player = player2.GetComponent<ControllerPlayer2>();
                player.rotation.x = -float.Parse(quat[0], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.z = -float.Parse(quat[1], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.y = -float.Parse(quat[2], CultureInfo.InvariantCulture.NumberFormat);
                player.rotation.w = float.Parse(quat[3], CultureInfo.InvariantCulture.NumberFormat);
              }
            }

            prevMessageGyro = listener.message;
          }

          // parse accelerometer data
          int accelerometerPos = listener.message.IndexOf("{A(");
          if (accelerometerPos != -1)
          {
            // check if field has changed
            if (listener.message != prevMessageAccelerometer)
            {
              // cut off everything except the floats
              string accelerometerData = listener.message.Substring(accelerometerPos + 3);
              accelerometerData = accelerometerData.Substring(0, accelerometerData.IndexOf("}") - 1);

              string[] vec = accelerometerData.Split(',');

              if (hit == 0)
              {
                ControllerPlayer1 player = player1.GetComponent<ControllerPlayer1>();
                player.acceleration.x = float.Parse(vec[0], CultureInfo.InvariantCulture.NumberFormat);
                player.acceleration.y = float.Parse(vec[1], CultureInfo.InvariantCulture.NumberFormat);
                player.acceleration.z = float.Parse(vec[2], CultureInfo.InvariantCulture.NumberFormat);
              }
              else if (hit == 1)
              {
                ControllerPlayer2 player = player2.GetComponent<ControllerPlayer2>();
                player.acceleration.x = float.Parse(vec[0], CultureInfo.InvariantCulture.NumberFormat);
                player.acceleration.y = float.Parse(vec[1], CultureInfo.InvariantCulture.NumberFormat);
                player.acceleration.z = float.Parse(vec[2], CultureInfo.InvariantCulture.NumberFormat);
              }
            }

            prevMessageAccelerometer = listener.message;
          }

          // pares joystick data
          int joystickPos = listener.message.IndexOf("{J(");
          if (joystickPos != -1)
          {
            // check if field has changed
            if (listener.message != prevJoystick)
            {
              string joystickData = listener.message.Substring(joystickPos + 3);
              joystickData = joystickData.Substring(0, joystickData.IndexOf("}") - 1);

              string[] vec = joystickData.Split(',');
              if (hit == 0)
              {
                ControllerPlayer1 player = player1.GetComponent<ControllerPlayer1>();
                player.joystick.x = float.Parse(vec[0], CultureInfo.InvariantCulture.NumberFormat);
                player.joystick.y = float.Parse(vec[1], CultureInfo.InvariantCulture.NumberFormat);
              }
              else if (hit == 1)
              {
                ControllerPlayer2 player = player2.GetComponent<ControllerPlayer2>();
                player.joystick.x = float.Parse(vec[0], CultureInfo.InvariantCulture.NumberFormat);
                player.joystick.y = float.Parse(vec[1], CultureInfo.InvariantCulture.NumberFormat);
              }
            }

            prevJoystick = listener.message;
          }

          // parse action button pressed
          int buttonPos = listener.message.IndexOf("{B(1)}");
          if (buttonPos != -1)
          {
            if (hit == 0)
            {
              ControllerPlayer1 player = player1.GetComponent<ControllerPlayer1>();
              player.actionPressed = true;
            }
            else if (hit == 1)
            {
              ControllerPlayer2 player = player2.GetComponent<ControllerPlayer2>();
              player.actionPressed = true;
            }
          }
          else
          {
            if (hit == 0)
            {
              ControllerPlayer1 player = player1.GetComponent<ControllerPlayer1>();
              player.actionPressed = false;
            }
            else if (hit == 1)
            {
              ControllerPlayer2 player = player2.GetComponent<ControllerPlayer2>();
              player.actionPressed = false;
            }
          }
        }
      }   
    }
  }
}
