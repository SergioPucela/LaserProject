using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private float interactionLength = 10f;

    public GameObject selectedMirror = null;

    // Update is called once per frame
    void Update()
    {
        InteractRaycast();
    }

    private void InteractRaycast()
    {
        Vector3 playerPosition = transform.position;
        Vector3 forwardDirection = transform.forward;

        Ray interactionRay = new Ray(playerPosition, forwardDirection);
        RaycastHit interactionHit;

        Vector3 interactionEndPoint = forwardDirection * interactionLength;
        Debug.DrawLine(playerPosition, interactionEndPoint, Color.red);

        bool hitFound = Physics.Raycast(interactionRay, out interactionHit, interactionLength);

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedMirror != null)
            {
                Debug.Log("You have deselected " + selectedMirror.name);
                selectedMirror = null;
            }
            else if (hitFound)
            {
                GameObject hitGO = interactionHit.transform.gameObject;

                if (hitGO.CompareTag("bounceLaser"))
                {
                    selectedMirror = hitGO;
                    Debug.Log("You have selected " + selectedMirror.name);
                }
            }
        }
    }
}
