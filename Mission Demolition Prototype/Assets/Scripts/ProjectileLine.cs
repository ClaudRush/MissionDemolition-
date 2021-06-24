using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject poi;
    private List<Vector3> points;

    public GameObject Poi
    {
        get { return (poi); }
        set
        {
            poi = value;
            if (poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    public Vector3 LastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    private void Awake()
    {
        S = this;

        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    private void FixedUpdate()
    {
        if (Poi == null)
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.CompareTag("Projectile"))
                {
                    Poi = FollowCam.POI;
                }
                else
                {
                    return;
                }
            }
        }
        AddPoint();
        if (FollowCam.POI == null)
        {
            Poi = null;
        }
    }
    public void Clear()
    {
        poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        if (poi != null)
        {
            Vector3 pt = poi.transform.position;
            if (points.Count > 0 && (pt - LastPoint).magnitude < minDist)
            {
                return;
            }
            if (points.Count == 0)
            {
                Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
                points.Add(pt + launchPosDiff);
                points.Add(pt);
                line.positionCount = 2;
                line.SetPosition(0, points[0]);
                line.SetPosition(1, points[1]);
                line.enabled = true;
            }
            else
            {
                points.Add(pt);
                line.positionCount = points.Count;
                line.SetPosition(points.Count - 1, LastPoint);
                line.enabled = true;
            }
        }
    }
}
