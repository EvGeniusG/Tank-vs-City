using System.Collections.Generic;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance { get; private set;}

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public int minStreetLength;
    public int maxStreetLength;
    public float intersectionProbability;
    public int mainRouteStreetCount;
    public float tileSize; // Размер квадрата тайла дороги

    public GameObject roadTile;  // Универсальный тайл для дорог
    public GameObject turnTile;  // Тайл для поворотов
    public GameObject tIntersectionTile;  // Тайл для Т-образных перекрестков
    public GameObject crossIntersectionTile;  // Тайл для крестообразных перекрестков
    public GameObject deadEndTile;  // Тайл для тупиков
    public GameObject blackTile;  // Черный тайл для отсутствия соседей

    // Тайлы 2x2 и их приоритеты
    public List<GameObject> cityWallTiles2x2;
    public List<int> cityWallTiles2x2Priorities;

    // Тайлы 1x1 и их приоритеты
    public List<GameObject> cityWallTiles1x1;
    public List<int> cityWallTiles1x1Priorities;

    public GameObject finishPrefab;

    Level actualLevel;

    void Start(){
        GenerateLevel(1); // Начинаем с уровня 1
    }

    public void GenerateLevel(int levelNum){
        SetGenerationParams(levelNum);
        actualLevel = new Level(mainRouteStreetCount, minStreetLength, maxStreetLength, intersectionProbability);
        GenerateLevelEnvironment();

        //Navigation

        NavigationManager.Instance.BuildNavMesh();



        TankCreator.Instance.CreateTank(new Vector3(actualLevel.startPosition.x * tileSize, 0, actualLevel.startPosition.y * tileSize));
        Instantiate(
            finishPrefab,
            new Vector3(actualLevel.finishPosition.x * tileSize, 0, actualLevel.finishPosition.y * tileSize),
            Quaternion.identity
        );

        
    }

    public void SetGenerationParams(int levelNum){
        // Максимальная длина дороги: от 1 до 2
        maxStreetLength = Mathf.Min(2, 1 + levelNum / 5);
        
        // Количество улиц: от 3 до 6
        mainRouteStreetCount = Mathf.Min(6, 3 + levelNum / 2);
        
        // Вероятность перекрестка: от 0 до 0.6
        intersectionProbability = Mathf.Min(0.6f, 0.2f + levelNum * 0.1f);
    }

    public void GenerateLevelEnvironment()
    {
        HashSet<Vector2Int> tilePositions = actualLevel.getRoadPositions();

        // Генерация дороги
        foreach (var tilePosition in tilePositions)
        {
            Vector3 worldPosition = new Vector3(tilePosition.x * tileSize, 0, tilePosition.y * tileSize);
            GameObject tileToInstantiate = DetermineTileType(tilePosition, tilePositions, out Quaternion rotation);
            Instantiate(tileToInstantiate, worldPosition, rotation);
        }

        // Определить границы уровня
        Vector2Int minBounds = new Vector2Int(int.MaxValue, int.MaxValue);
        Vector2Int maxBounds = new Vector2Int(int.MinValue, int.MinValue);

        foreach (var pos in tilePositions)
        {
            if (pos.x < minBounds.x) minBounds.x = pos.x;
            if (pos.y < minBounds.y) minBounds.y = pos.y;
            if (pos.x > maxBounds.x) maxBounds.x = pos.x;
            if (pos.y > maxBounds.y) maxBounds.y = pos.y;
        }

        minBounds -= new Vector2Int(6, 6);
        maxBounds += new Vector2Int(6, 6);

        var occupiedPositions = new HashSet<Vector2Int>(tilePositions);

        // Занять квадраты 2х2
        for (int x = minBounds.x; x < maxBounds.x; x++)
        {
            for (int y = minBounds.y; y < maxBounds.y; y++)
            {
                Vector2Int bottomLeft = new Vector2Int(x, y);
                if (IsAreaFree(occupiedPositions, bottomLeft, 2))
                {
                    occupiedPositions.Add(bottomLeft);
                    occupiedPositions.Add(bottomLeft + new Vector2Int(1, 0));
                    occupiedPositions.Add(bottomLeft + new Vector2Int(0, 1));
                    occupiedPositions.Add(bottomLeft + new Vector2Int(1, 1));

                    var tile = GetRandomTile(cityWallTiles2x2, cityWallTiles2x2Priorities);
                    Instantiate(tile, new Vector3(bottomLeft.x * tileSize, 0, bottomLeft.y * tileSize), Quaternion.identity);
                }
            }
        }

        // Занять оставшиеся свободные места домами 1х1
        for (int x = minBounds.x; x <= maxBounds.x; x++)
        {
            for (int y = minBounds.y; y <= maxBounds.y; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!occupiedPositions.Contains(position) && IsAreaFree(occupiedPositions, position, 1))
                {
                    occupiedPositions.Add(position);
                    var tile = GetRandomTile(cityWallTiles1x1, cityWallTiles1x1Priorities);
                    Instantiate(tile, new Vector3(position.x * tileSize, 0, position.y * tileSize), Quaternion.identity);
                }
            }
        }
    }

    private GameObject DetermineTileType(Vector2Int position, HashSet<Vector2Int> tilePositions, out Quaternion rotation)
    {
        bool up = tilePositions.Contains(position + Vector2Int.up);
        bool down = tilePositions.Contains(position + Vector2Int.down);
        bool left = tilePositions.Contains(position + Vector2Int.left);
        bool right = tilePositions.Contains(position + Vector2Int.right);

        rotation = Quaternion.identity;

        if (up && down && left && right)
        {
            return crossIntersectionTile;  // Крестообразный перекресток
        }
        else if (up && down && left)
        {
            rotation = Quaternion.Euler(0, -90, 0);
            return tIntersectionTile;  // Т-образный перекресток с открытым правым направлением
        }
        else if (up && down && right)
        {
            rotation = Quaternion.Euler(0, 90, 0);
            return tIntersectionTile;  // Т-образный перекресток с открытым левым направлением
        }
        else if (left && right && up)
        {
            return tIntersectionTile;  // Т-образный перекресток с открытым нижним направлением
        }
        else if (left && right && down)
        {
            rotation = Quaternion.Euler(0, 180, 0);
            return tIntersectionTile;  // Т-образный перекресток с открытым верхним направлением
        }
        else if (up && down)
        {
            return roadTile;  // Вертикальная дорога
        }
        else if (left && right)
        {
            rotation = Quaternion.Euler(0, 90, 0);
            return roadTile;  // Горизонтальная дорога
        }
        else if (up && left)
        {
            return turnTile;  // Поворот вверх-налево
        }
        else if (up && right)
        {
            rotation = Quaternion.Euler(0, 90, 0);
            return turnTile;  // Поворот вверх-направо
        }
        else if (down && left)
        {
            rotation = Quaternion.Euler(0, -90, 0);
            return turnTile;  // Поворот вниз-налево
        }
        else if (down && right)
        {
            rotation = Quaternion.Euler(0, 180, 0);
            return turnTile;  // Поворот вниз-направо
        }
        else if (up)
        {
            rotation = Quaternion.Euler(0, 180, 0);
            return deadEndTile;  // Тупик вверх
        }
        else if (down)
        {
            return deadEndTile;  // Тупик вниз
        }
        else if (left)
        {
            rotation = Quaternion.Euler(0, 90, 0);
            return deadEndTile;  // Тупик налево
        }
        else if (right)
        {
            rotation = Quaternion.Euler(0, -90, 0);
            return deadEndTile;  // Тупик направо
        }

        return blackTile;  // Черный тайл, если нет соседей
    }

    private bool IsAreaFree(HashSet<Vector2Int> occupiedPositions, Vector2Int bottomLeft, int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (occupiedPositions.Contains(bottomLeft + new Vector2Int(x, y)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private GameObject GetRandomTile(List<GameObject> tiles, List<int> priorities)
    {
        int totalPriority = 0;
        foreach (var priority in priorities)
        {
            totalPriority += priority;
        }

        int randomValue = Random.Range(0, totalPriority);
        int cumulativePriority = 0;

        for (int i = 0; i < tiles.Count; i++)
        {
            cumulativePriority += priorities[i];
            if (randomValue < cumulativePriority)
            {
                return tiles[i];
            }
        }

        return tiles[0]; // На случай если что-то пошло не так
    }
}
