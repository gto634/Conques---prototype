using UnityEngine;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour
{
    private Camera cam;
    public MapGenerator mapGenerator; 

    [Header("Sensibilité")]
    public float seuilDistance = 0.1f;

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            DetectarClic(ray);
        }
    }

    private void DetectarClic(Ray ray)
    {
        if (mapGenerator == null || mapGenerator.nodeGrid == null) return;

        CornerNode cornerClique = null;
        EdgeNode edgeClique = null;
        float meilleureDist = seuilDistance;

        foreach (CornerNode corner in mapGenerator.nodeGrid.allCorners)
        {
            float distance = DistancePointRayon(corner.worldPosition, ray);
            if (distance < meilleureDist)
            {
                meilleureDist = distance;
                cornerClique = corner;
                edgeClique = null;
            }
        }

        foreach (EdgeNode edge in mapGenerator.nodeGrid.allEdges)
        {
            float distance = DistancePointRayon(edge.worldPosition, ray);
            if (distance < meilleureDist)
            {
                meilleureDist = distance;
                edgeClique = edge;
                cornerClique = null;
            }
        }

        if (cornerClique != null)
        {
            Debug.Log($"Clic sur Corner (runtime) - Build possible : {cornerClique.CanBuild()}");
            if (cornerClique.CanBuild())
            {
                cornerClique.hasBuild = true;
            }
        }
        else if (edgeClique != null)
        {
            Debug.Log($"Clic sur Edge (runtime) - Build possible : {edgeClique.CanBuild()}");
            if (edgeClique.CanBuild())
            {
                edgeClique.hasRoad = true;
            }
        }
    }

    private float DistancePointRayon(Vector3 point, Ray ray)
    {
        Vector3 auRayon = point - ray.origin;
        Vector3 projection = Vector3.Project(auRayon, ray.direction);
        Vector3 perpendiculaire = auRayon - projection;
        return perpendiculaire.magnitude;
    }
}