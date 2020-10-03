using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public enum MirrorType { FreeRotationX, FreeRotationY, FixedRotation }

    public MirrorType type;
}
