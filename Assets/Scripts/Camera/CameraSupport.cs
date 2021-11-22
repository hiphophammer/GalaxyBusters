/*
 * @file CameraSupport.cs
 *
 * 
 * 
 * @author Kelvin Sung, revised by Shakeel Khan
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraSupport : MonoBehaviour
{
    // Private member variables.
    private Camera mTheCamera;      // This is our reference to the camera we'll be
                                    // supporting.
    private Bounds mWorldBound;     // Computed bound from the camera.

    // This provides an easy way of determining where our bounds are within the camera
    // bounds.
    public enum WorldBoundStatus
    {
        Outside = 0,
        CollideLeft = 1,
        CollideRight = 2,
        CollideTop = 4,
        CollideBottom = 8,
        Inside = 16
    };

    /// <summary>
    /// This method is called once at the beginning of the game, before Start().
    /// 
    /// Here we retrieve the camera and store it's bounds.
    /// 
    /// The camera may be disabled by some in Start(), so we should initialize in Awake().
    /// </summary>
    void Awake()
    {
        // Grab our camera.
        mTheCamera = gameObject.GetComponent<Camera>();
        Debug.Assert(mTheCamera != null);

        Debug.Log("CameraSupport: Awake: " + gameObject.name);

        // Setup our bounds.
        mWorldBound = new Bounds();
        UpdateWorldWindowBound();
    }

    /// <summary>
    /// This is called once per frame, and all we're doing is updating our camera bounds.
    /// </summary>
    void Update()
    {
        // Update our world window bound.
        UpdateWorldWindowBound();
    }

    /// <summary>
    /// Retrieves the bounds of the attached camera.
    /// </summary>
    /// <returns>The bounds of the attached camera.</returns>
    public Bounds GetWorldBound()
    {
        return mWorldBound;
    }

    /// <summary>
    /// Determines where the provided bounds intersect within the specified region of the
    /// bounds of the attached camera.
    /// </summary>
    /// <param name="objBound">The bounds to check.</param>
    /// <param name="region">The region of the camera bounds to check.</param>
    /// <returns>An enum specifying which bounds are being intersected with.</returns>
    public WorldBoundStatus CollideWorldBound(Bounds objBound, float region = 1.0f)
    {
        WorldBoundStatus status = WorldBoundStatus.Outside;
        Bounds camBounds = new Bounds(transform.position, region * mWorldBound.size);

        // Determine which bounds (if any) the given bounds are intersecting.
        if (BoundsIntersectInXY(camBounds, objBound))
        {
            if (objBound.max.x > camBounds.max.x)
            {
                status |= WorldBoundStatus.CollideRight;
            }

            if (objBound.min.x < camBounds.min.x)
            {
                status |= WorldBoundStatus.CollideLeft;
            }

            if (objBound.max.y > camBounds.max.y)
            {
                status |= WorldBoundStatus.CollideTop;
            }

            if (objBound.min.y < camBounds.min.y)
            {
                status |= WorldBoundStatus.CollideBottom;
            }

            // Intersects and no bounds touch ==> Inside!
            if (status == WorldBoundStatus.Outside)
            {
                status = WorldBoundStatus.Inside;
            }
        }

        return status;
    }

    /// <summary>
    /// Computes the bounds of the attached camera.
    /// </summary>
    private void UpdateWorldWindowBound()
    {
        // Make sure we have a proper reference to the camera.
        if (null != mTheCamera)
        {
            // Compute the bounds.
            float maxY = mTheCamera.orthographicSize;
            float maxX = mTheCamera.orthographicSize * mTheCamera.aspect;
            float sizeX = 2 * maxX;
            float sizeY = 2 * maxY;

            // Update the center.
            Vector3 center = mTheCamera.transform.position;
            center.z = 0.0f;
            mWorldBound.center = center;

            mWorldBound.size = new Vector3(sizeX, sizeY, 1.0f);     // Z is arbitrary!
        }
    }

    /// <summary>
    /// This checks whether the bounds of one object is at least partially within the
    /// bounds of another.
    /// </summary>
    /// <param name="b1">The bounds the other must be in.</param>
    /// <param name="b2">These are the bounds that we must check are within the other
    /// bounds.</param>
    /// <returns>A boolean indicating whether the bounds of b2 are partially within
    /// the bounds of b2.</returns>
    private bool BoundsIntersectInXY(Bounds b1, Bounds b2)
    {
        // We can't use the intersect() or contains() methods of the Bounds class since
        // we're not using the z-axis, so we must manually check the x and y axes.
        return (b1.min.x < b2.max.x) && (b1.max.x > b2.min.x) &&
               (b1.min.y < b2.max.y) && (b1.max.y > b2.min.y);
    }
}