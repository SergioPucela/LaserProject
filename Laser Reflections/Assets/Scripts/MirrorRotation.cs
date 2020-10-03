using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MirrorRotation : MonoBehaviour
{
    public float controlSpeedFixed = 10f;
    public float controlSpeedFree = 10f;
    public float angleRotation = 45f;

    public int horizontal = 0;
    public int vertical = 0;

    private Quaternion mirrorRotation;
    private Mirror mirrorType;

    // Start is called before the first frame update
    void Start()
    {
        mirrorRotation = transform.localRotation;
        mirrorType = GetComponent<Mirror>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mirrorType.type == Mirror.MirrorType.FixedRotation)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, mirrorRotation, controlSpeedFixed * Time.deltaTime);
    }

    public void rotateMirror(int horizontal, int vertical)
    {
        if (mirrorType.type == Mirror.MirrorType.FixedRotation)
            mirrorRotation = Quaternion.Euler(new Vector3(vertical * angleRotation, -horizontal * angleRotation, 0f));
        else if (mirrorType.type == Mirror.MirrorType.FreeRotationX || mirrorType.type == Mirror.MirrorType.FreeRotationY)
        {
            transform.Rotate(new Vector3(vertical * controlSpeedFree, -horizontal * controlSpeedFree, 0f));
        }
    }

    public bool isFacingMirror(float laserDistance, LayerMask ignoreLayers)
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.forward, out hit, laserDistance, ignoreLayers))
        {
            Debug.DrawRay(transform.position, -transform.forward * laserDistance, Color.yellow);
            if (hit.transform.gameObject.CompareTag("bounceLaser"))
            {
                return true;
            }
        }
        return false;
    }
}
