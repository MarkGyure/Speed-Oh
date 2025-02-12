using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform player;
    [SerializeField] private int resolution = 30; // Number of points in the line
    [SerializeField] private float timeStep = 0.05f;
    [SerializeField] private LayerMask platformMask; 
    [SerializeField] private GameObject landingCirclePrefab;

    [SerializeField] private GameObject activeLandingCircle;

    private void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawTrajectory(Vector3 initialVelocity)
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 startPosition = player.position;
        Vector3 velocity = initialVelocity;
        float t = 0;
        bool hitDetected = false;
        Vector3 landingPoint = Vector3.zero;

        for (int i = 0; i < resolution; i++)
        {
            Vector3 point = startPosition + velocity * t + 0.5f * Physics.gravity * t * t;
            points.Add(point);
            t += timeStep;

            // Check if trajectory collides with a platform
            if (Physics.Raycast(point, Vector3.down, out RaycastHit hit, 0.5f, platformMask))
            {
                hitDetected = true;
                landingPoint = hit.point;

                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

        // Spawn or update the landing circle
        if (hitDetected)
        {
            PlaceLandingCircle(landingPoint);
        }
        else if (activeLandingCircle)
        {
            activeLandingCircle.SetActive(false); // Hide if no valid landing
        }
    }

    private void PlaceLandingCircle(Vector3 position)
    {
        if (activeLandingCircle == null)
        {
            activeLandingCircle = Instantiate(landingCirclePrefab, position, Quaternion.identity);
        }
        else
        {
            activeLandingCircle.SetActive(true);
            activeLandingCircle.transform.position = position;
        }
    }

    public void ClearTrajectory()
    {
        lineRenderer.positionCount = 0;

        if (activeLandingCircle)
            activeLandingCircle.SetActive(false); // Hide landing marker
    }
}

