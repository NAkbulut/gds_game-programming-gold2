using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength) {
        var path = new HashSet<Vector2Int>();
        
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++) {
            var newPosition = previousPosition + Direction2D.getRandomDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength) {
        var corridor = new List<Vector2Int>();
        var direction = Direction2D.getRandomDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++) {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight, int maxWidth, int maxHeight, int minRoomCount, int maxRoomCount) {
        var roomsQueue = new Queue<BoundsInt>();
        var roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0 && roomsList.Count < maxRoomCount) {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth) {
                if (Random.value < 0.5f) {
                    if (room.size.y >= minHeight * 2) {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2) {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else if (room.size.x <= maxWidth && room.size.y <= maxHeight) {
                        roomsList.Add(room);
                    }
                }
                else {
                    if (room.size.x >= minWidth * 2) {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2) {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x <= maxWidth && room.size.y <= maxHeight) {
                        roomsList.Add(room);
                    }
                }
            }
            if (roomsList.Count >= minRoomCount) {
                if (Random.value < 0.1f) {
                    return roomsList;
                }
            }
        }
        return roomsList;
    }

    public static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room) {
        var ySplit = Random.Range(1, room.size.y);
        var room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        var room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    public static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room) {
        var xSplit = Random.Range(1, room.size.x);
        var room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        var room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int> {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };

    public static Vector2Int getRandomDirection() {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
