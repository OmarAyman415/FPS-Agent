using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera camera;
    private float xRotation = 0f;
    public float xSensetivity = 30f;
    public float ySensetivity = 30f;
    
    public void ProcessLook(Vector2 input){
        float mouseX = input.x;
        float mouseY = input.y;
        // Calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensetivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //apply this to our camera transform
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // rotate player to look left and rigth
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensetivity);


    }
}
