using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object used to detect if an object should be picked up
public class scrDeleteArmMask : MonoBehaviour
{
    public GameObject playerOrigin;

    // Destroy mask for picking up other players
    void Update() {
        Destroy(gameObject, 1.25f);
    }
}
