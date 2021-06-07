using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

/// <summary>
/// Positions a NPC according to its FinalIK rig.
/// </summary>
public class IKPosition : MonoBehaviour
{
    /// <summary>
    /// The rig of the NPC.
    /// </summary>
    public FullBodyBipedIK ik;

    //Targets to position each parts of the NPC's rig.
    public Transform leftHandTarget, rightHandTarget, leftFootTarget, rightFootTarget, 
        leftThighTarget, rightThighTarget, leftShoulderTarget, rightShoulderTarget, bodyTarget;

    private void Update()
    {
        if (ik != null)
        {
            if (bodyTarget != null)
            {
                ik.solver.bodyEffector.position = bodyTarget.position;
                ik.solver.bodyEffector.positionWeight = 1f;
            }

            if (leftHandTarget != null)
            {
                ik.solver.leftHandEffector.position = leftHandTarget.position;
                ik.solver.leftHandEffector.rotation = leftHandTarget.rotation;
                ik.solver.leftHandEffector.positionWeight = 1f;
                ik.solver.leftHandEffector.rotationWeight = 1f;
            }

            if (rightHandTarget != null)
            {
                ik.solver.rightHandEffector.position = rightHandTarget.position;
                ik.solver.rightHandEffector.rotation = rightHandTarget.rotation;
                ik.solver.rightHandEffector.positionWeight = 1f;
                ik.solver.rightHandEffector.rotationWeight = 1f;
            }

            if (leftFootTarget != null)
            {
                ik.solver.leftFootEffector.position = leftFootTarget.position;
                ik.solver.leftFootEffector.rotation = leftFootTarget.rotation;
                ik.solver.leftFootEffector.positionWeight = 1f;
                ik.solver.leftFootEffector.rotationWeight = 1f;
            }

            if (rightFootTarget != null)
            {
                ik.solver.rightFootEffector.position = rightFootTarget.position;
                ik.solver.rightFootEffector.rotation = rightFootTarget.rotation;
                ik.solver.rightFootEffector.positionWeight = 1f;
                ik.solver.rightFootEffector.rotationWeight = 1f;
            }

            if (leftThighTarget != null)
            {
                ik.solver.leftThighEffector.position = leftThighTarget.position;
                ik.solver.leftThighEffector.positionWeight = 1f;
            }

            if (rightThighTarget != null)
            {
                ik.solver.rightThighEffector.position = rightThighTarget.position;
                ik.solver.rightThighEffector.positionWeight = 1f;
            }

            if (leftShoulderTarget != null)
            {
                ik.solver.leftShoulderEffector.position = leftShoulderTarget.position;
                ik.solver.leftShoulderEffector.rotation = leftShoulderTarget.rotation;
                ik.solver.leftShoulderEffector.positionWeight = 1f;
                ik.solver.leftShoulderEffector.rotationWeight = 1f;
            }

            if (rightShoulderTarget != null)
            {
                ik.solver.rightShoulderEffector.position = rightShoulderTarget.position;
                ik.solver.rightShoulderEffector.rotation = rightShoulderTarget.rotation;
                ik.solver.rightShoulderEffector.positionWeight = 1f;
                ik.solver.rightShoulderEffector.rotationWeight = 1f;
            }
        }
    }
}
