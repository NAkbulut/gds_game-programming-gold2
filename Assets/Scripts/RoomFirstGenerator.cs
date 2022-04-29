using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstGenerator : SimpleRandomWalkGenerator
{
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20;
    [SerializeField] private int dungeonHeight = 20;
    [SerializeField] [Range(0, 10)] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms() {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(
            new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        
        var floorPositions = new HashSet<Vector2Int>();
        if (randomWalkRooms) {
            floorPositions = CreateRoomsRandomly(roomsList);
        }
        else {
            floorPositions = CreateSimpleRooms(roomsList);
        }
        var roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList) {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floorPositions.UnionWith(corridors);
        tilemapVisualizer.Visualize(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList) {
        var floorPositions = new HashSet<Vector2Int>();
        foreach (var room in roomList) {
            for (int col = offset; col < room.size.x - offset; col++) {
                for (int row = offset; row < room.size.y - offset; row++) {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floorPositions.Add(position);
                }
            }
        }
        return floorPositions;
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList) {
        var floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++) {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(simpleRandomWalkData, roomCenter);
            foreach (var position in roomFloor) {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset)
                && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset)) {
                    floorPositions.Add(position);
                }
            }
        }
        return floorPositions;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters) {
        var corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0 ) {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters) {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters) {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if (currentDistance < distance) {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int target) {
        var corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while (position.y != target.y) {
            if (target.y > position.y) {
                position += Vector2Int.up;
            }
            else if (target.y < position.y) {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != target.x) {
            if (target.x > position.x) {
                position += Vector2Int.right;
            }
            else if (target.x < position.x) {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }

}
