using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectorRenderer : MonoBehaviour
{
   [SerializeField] private LineRenderer lineRenderer;
   [SerializeField] private Transform ground_player;
   [SerializeField] private int resolution = 1; // Number of points in the line
   [SerializeField] private float timeStep = 0.05f; // Time between each point
   [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector3 initialVelocity)
    {
       

        if (initialVelocity.magnitude < 0.1f)
        {
           
            return;
        }

        List<Vector3> points = new List<Vector3>();
        Vector3 startPosition = ground_player.transform.position;
        Vector3 velocity = initialVelocity;
        float t = 0;

        for (int i = 0; i < 30; i++) // Increased resolution
        {
            Vector3 point = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
            points.Add(point);
            t += 0.05f;

            // Stop drawing if trajectory hits ground
            if (Physics.Raycast(point, Vector3.down, 0.1f, LayerMask.GetMask("Ground")))
            {
                break;
            }
        }

        if (points.Count > 1)
        {
            Debug.Log(" Trajectory has " + points.Count + " points.");
        }
        else
        {
            Debug.Log(" Not enough points, clearing trajectory.");
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }


    public void ClearTrajectory()
    {
        lineRenderer.positionCount = 0; // Hide the line when grounded
    }
}
