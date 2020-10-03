using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMirrorControl : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;

    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    GameObject selected;
    MirrorRotation currentMirror;
    Mirror mirrorType; //Testing

    // Start is called before the first frame update
    void Awake()
    {
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        checkMirrorSelection();

        if(selected != null)
        {
            mirrorControl();
        }

    }

    void checkMirrorSelection()
    {
        if (playerInteraction.selectedMirror != null) //If there is a mirror selected
        {
            if(selected == null)
            {
                playerMovement.enabled = false;
                selected = playerInteraction.selectedMirror;
                currentMirror = selected.GetComponent<MirrorRotation>();
                mirrorType = selected.GetComponent<Mirror>(); //Testing
            }
        }
        else if (!playerMovement.enabled)
        {
            if(selected != null)
            {
                playerMovement.enabled = true;
                selected = null;
                currentMirror = null;
                mirrorType = null; //Testing
            }
        }
    }

    void mirrorControl()
    {
        if(mirrorType.type == Mirror.MirrorType.FixedRotation)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                x = Input.GetAxisRaw("Horizontal");
                z = 0;
                if (x == 1 && currentMirror.horizontal < 1)
                {
                    currentMirror.horizontal++;
                }
                else if (x == -1 && currentMirror.horizontal > -1)
                {
                    currentMirror.horizontal--;
                }
                currentMirror.rotateMirror(currentMirror.horizontal, currentMirror.vertical);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                z = Input.GetAxisRaw("Vertical");
                x = 0;
                if (z == 1 && currentMirror.vertical < 1)
                {
                    currentMirror.vertical++;
                }
                else if (z == -1 && currentMirror.vertical > -1)
                {
                    currentMirror.vertical--;
                }
                currentMirror.rotateMirror(currentMirror.horizontal, currentMirror.vertical);
            }
        }
        else if(mirrorType.type == Mirror.MirrorType.FreeRotationX)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = 0;

            currentMirror.rotateMirror((int)x, (int)z);
        }
        else if (mirrorType.type == Mirror.MirrorType.FreeRotationY)
        {
            x = 0;
            z = Input.GetAxisRaw("Vertical");

            currentMirror.rotateMirror((int)x, (int)z);
        }
    }
}