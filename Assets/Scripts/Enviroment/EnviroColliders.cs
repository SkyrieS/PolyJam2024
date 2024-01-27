using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnviroColliders
{
    public ColliderSetup leftWall;
    public ColliderSetup rightWall;
    public ColliderSetup topWall;
    public ColliderSetup lowerWall;

    public ColliderSetup GetWall(WallType type)
    {
        List<ColliderSetup> colls = new List<ColliderSetup>() { leftWall, rightWall, topWall, lowerWall };
        return colls.Find(c => c.type == type);
    }

    public void Adjust(Vector2 size, float thickness)
    {
        leftWall.wall.size = new Vector2(thickness, size.y);
        leftWall.wall.transform.localPosition = new Vector3(-(size.x / 2f) - (thickness / 2f), 0f, 0f);

        rightWall.wall.size = new Vector2(thickness, size.y);
        rightWall.wall.transform.localPosition = new Vector3((size.x / 2f) + (thickness / 2f), 0f, 0f);

        topWall.wall.size = new Vector2(size.x + (thickness * 2f), thickness);
        topWall.wall.transform.localPosition = new Vector3(0f, -(size.y / 2f) - (thickness / 2f), 0f);

        lowerWall.wall.size = new Vector2(size.x + (thickness * 2f), thickness);
        lowerWall.wall.transform.localPosition = new Vector3(0f, (size.y / 2f) + (thickness / 2f), 0f);
    }

    public void DrawGizmos()
    {
        leftWall.DrawGizmos();
        rightWall.DrawGizmos();
        topWall.DrawGizmos();
        lowerWall.DrawGizmos();
    }
}

[System.Serializable]
public class ColliderSetup
{
    public BoxCollider2D wall;
    public WallType type;

    public List<Vector2> slotsPosition;

    public Vector2 GetPosition(Vector3 normalizedPos)
    {
        float x = Mathf.Lerp(-wall.size.x / 2f, wall.size.x / 2f, normalizedPos.x);
        float y = Mathf.Lerp(-wall.size.y / 2f, wall.size.y / 2f, normalizedPos.y);
        return wall.transform.TransformPoint(new Vector2(x, y));
    }

    public List<ReceiveZone> GenerateSlotsZones(ReceiveZone zone, Transform parent)
    {
        List<ReceiveZone> zones = new List<ReceiveZone>();
        foreach (var slots in slotsPosition)
        {
            ReceiveZone newZone = GameObject.Instantiate(zone, parent);
            newZone.transform.position = GetPosition(slots);

            switch (type) 
            {
                case WallType.Top:
                    newZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case WallType.Bottom:
                    newZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    break;
                case WallType.Right:
                    newZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    break;
                case WallType.Left:
                    newZone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    break;
            }


            zones.Add(newZone);
        }
        return zones;
    }

    public void DrawGizmos()
    {
        foreach (var pos in slotsPosition)
            Gizmos.DrawWireSphere(GetPosition(pos), 0.5f);
    }
}

public enum WallType
{
    Top,
    Bottom,
    Left,
    Right
}