using System;
using System.Collections.Generic; // Allows to use Lists<T>

using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

    }

    public int Columns = 8;
    public int Rows = 8;

    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);

    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    // reference to the Board object
    private Transform _boardHolder;
    // all of the possible locations
    private List<Vector3> _gridPosition = new List<Vector3>();

    void InitialiseList()
    {
        _gridPosition.Clear();

        for (int x = 1; x < Columns - 1; x++) {
            for (int y = 1; y < Rows - 1; y++) {
                _gridPosition.Add(new Vector3(x, y, 0f));
            }
        }

    }

    void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;

        for (int x = -1; x <= Columns; x++) {
            for (int y = -1; y <= Rows; y++) {

                GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];

                if (x == -1 || y == -1 || x == Columns || y == Columns) {
                    toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(_boardHolder);

            }
        }
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int nObjects = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < nObjects; i++) {

            Vector3 randomPosition = GetRandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);

        }

    }

    Vector3 GetRandomPosition()
    {
        int randomIndex = Random.Range(0, _gridPosition.Count);
        Vector3 randomPosition = _gridPosition[randomIndex];
        _gridPosition.RemoveAt(randomIndex);
        return randomPosition;
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();

        LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);
        LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
        Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
    }

}
