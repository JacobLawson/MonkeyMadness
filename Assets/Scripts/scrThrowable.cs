using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrThrowable : MonoBehaviour
{

    public GameObject pickingMeUp;
    public GameObject heldObject;
    private Rigidbody rigidbody;

    private float initialForce = 500f;
    private float throwPower = 1500f;

    public enum State {
        THROWN,
        HELD,
        GROUNDED
    }
    public State state = State.GROUNDED;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();

        pickingMeUp = null;
        heldObject = null;

        throwPower = initialForce - ((transform.localScale.y * transform.localScale.z) * transform.localScale.x);
    }
    void Update() {
        //Handle the state
        switch (state) {
            //if thrown apply force across the player
            case State.THROWN:
                transform.position = new Vector3(pickingMeUp.transform.position.x, pickingMeUp.transform.position.y + 1f + pickingMeUp.transform.localScale.y, pickingMeUp.transform.position.z);
                rigidbody.AddForce(pickingMeUp.transform.forward * throwPower);
                state = State.GROUNDED;
                break;
            case State.HELD:
                //if held keep this player sat above the other player
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                transform.position = new Vector3(pickingMeUp.transform.position.x, pickingMeUp.transform.position.y + 0.5f + transform.localScale.y, pickingMeUp.transform.position.z);
                break;
            case State.GROUNDED:
                //Return control to the player
                pickingMeUp = null;
                break;
        }
    }

    void OnTriggerEnter(Collider col) {
        //pick up
        if (col.gameObject.tag == "Arm Mask") {
            if (pickingMeUp == null) {
                pickingMeUp = col.gameObject.GetComponent<scrDeleteArmMask>().playerOrigin;
                pickingMeUp.GetComponent<scrPlayerKeepOrientation>().heldObject = gameObject;
                Debug.Log(pickingMeUp.GetComponent<scrPlayerKeepOrientation>().heldObject.name);
                state = State.HELD;
            }
        }
        //return control when grounded
        else if (col.gameObject.tag == "Platform") {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            state = State.GROUNDED;
        }
    }
}
