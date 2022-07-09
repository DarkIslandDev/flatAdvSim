using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pathfinding;

[Serializable]
public class Components
{
    public GameObject[] EnvPrefabs;
}

[Serializable]
public class General
{
    [Header("Префабы")] public GameObject grass;
    public GameObject water;
    [Header("Материалы")] public Material terrainMaterial;
    public Material edgeMaterial;
    public Material gridMaterial;
}

[Serializable]
public class Settings
{
    public float waterLevel = .4f;
    public float scale = .1f;
    public float treeNoiseScale = .05f;
    public float treeDensity = .5f;
    public float riverNoiseScale = .06f;
    public int rivers = 5;
    public int size = 100;
}

public class MapGenerator : MonoBehaviour
{
    public General general = new General();
    public Components components = new Components();
    public Settings settings = new Settings();

    Cell[,] grid;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        float[,] noiseMap = new float[settings.size, settings.size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * settings.scale + xOffset, y * settings.scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[settings.size, settings.size];
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                float xv = x / (float) settings.size * 2 - 1;
                float yv = y / (float) settings.size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[settings.size, settings.size];
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < settings.waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }

        // GenerateRivers(grid);
        DrawTerrainMesh(grid);
        DrawEdgeMesh(grid);
        DrawTexture(grid);
        GenerateWater();
        GenerateVegetation(grid);
        GenerateEnviroment(grid);

        // DrawBuildMesh(grid);

        this.transform.position = new Vector3(-100, 0, -50);
    }

    // void GenerateRivers(Cell[,] grid)
    // {
    //     float[,] noiseMap = new float[size, size];
    //     (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
    //     for (int y = 0; y < size; y++)
    //     {
    //         for (int x = 0; x < size; x++)
    //         {
    //             float noiseValue = Mathf.PerlinNoise(x * riverNoiseScale + xOffset, y * riverNoiseScale + yOffset);
    //             noiseMap[x, y] = noiseValue;
    //         }
    //     }
    //
    //     GridGraph gg = AstarData.active.graphs[0] as GridGraph;
    //     gg.center = new Vector3(size / 2f - .5f, 0, size / 2f - .5f);
    //     gg.SetDimensions(size, size, 1);
    //     AstarData.active.Scan(gg);
    //     AstarData.active.AddWorkItem(new AstarWorkItem(ctx =>
    //     {
    //         for (int y = 0; y < size; y++)
    //         {
    //             for (int x = 0; x < size; x++)
    //             {
    //                 GraphNode node = gg.GetNode(x, y);
    //                 node.Walkable = noiseMap[x, y] > .4f;
    //             }
    //         }
    //     }));
    //     AstarData.active.FlushGraphUpdates();
    //
    //     int k = 0;
    //     for (int i = 0; i < rivers; i++)
    //     {
    //         GraphNode start = gg.nodes[Random.Range(16, size - 16)];
    //         GraphNode end = gg.nodes[Random.Range(size * (size - 1) + 16, size * size - 16)];
    //         ABPath path = ABPath.Construct((Vector3) start.position, (Vector3) end.position, (Path result) =>
    //         {
    //             for (int j = 0; j < result.path.Count; j++)
    //             {
    //                 GraphNode node = result.path[j];
    //                 int x = Mathf.RoundToInt(((Vector3) node.position).x);
    //                 int y = Mathf.RoundToInt(((Vector3) node.position).z);
    //                 grid[x, y].isWater = true;
    //             }
    //
    //             k++;
    //         });
    //         AstarPath.StartPath(path);
    //         AstarPath.BlockUntilCalculated(path);
    //     }
    // }

    void DrawTerrainMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float) settings.size, y / (float) settings.size);
                    Vector2 uvB = new Vector2((x + 1) / (float) settings.size, y / (float) settings.size);
                    Vector2 uvC = new Vector2(x / (float) settings.size, (y + 1) / (float) settings.size);
                    Vector2 uvD = new Vector2((x + 1) / (float) settings.size, (y + 1) / (float) settings.size);
                    Vector3[] v = new Vector3[] {a, b, c, b, d, c};
                    Vector2[] uv = new Vector2[] {uvA, uvB, uvC, uvB, uvD, uvC};
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    void DrawEdgeMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] {a, b, c, b, d, c};
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }

                    if (x < settings.size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] {a, b, c, b, d, c};
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }

                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] {a, b, c, b, d, c};
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }

                    if (y < settings.size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] {a, b, c, b, d, c};
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = general.edgeMaterial;
    }

    // void DrawBuildMesh(Cell[,] grid)
    // {
    //     Mesh mesh = new Mesh();
    //     List<Vector3> vertices = new List<Vector3>();
    //     List<int> triangles = new List<int>();
    //     for (int y = 0; y < settings.size; y++)
    //     {
    //         for (int x = 0; x < settings.size; x++)
    //         {
    //             Cell cell = grid[x, y];
    //             if (!cell.isWater)
    //             {
    //                 if (x > 0)
    //                 {
    //                     Cell left = grid[x - 1, y];
    //                     if (left.isWater)
    //                     {
    //                         Vector3 a = new Vector3(x - .5f, 0, y + .5f);
    //                         Vector3 b = new Vector3(x - .5f, 0, y - .5f);
    //                         Vector3 c = new Vector3(x - .5f, -1, y + .5f);
    //                         Vector3 d = new Vector3(x - .5f, -1, y - .5f);
    //                         Vector3[] v = new Vector3[] {a, b, c, b, d, c};
    //                         for (int k = 0; k < 6; k++)
    //                         {
    //                             vertices.Add(v[k]);
    //                             triangles.Add(triangles.Count);
    //                         }
    //                     }
    //                 }
    //
    //                 if (x < settings.size - 1)
    //                 {
    //                     Cell right = grid[x + 1, y];
    //                     if (right.isWater)
    //                     {
    //                         Vector3 a = new Vector3(x + .5f, 0, y - .5f);
    //                         Vector3 b = new Vector3(x + .5f, 0, y + .5f);
    //                         Vector3 c = new Vector3(x + .5f, -1, y - .5f);
    //                         Vector3 d = new Vector3(x + .5f, -1, y + .5f);
    //                         Vector3[] v = new Vector3[] {a, b, c, b, d, c};
    //                         for (int k = 0; k < 6; k++)
    //                         {
    //                             vertices.Add(v[k]);
    //                             triangles.Add(triangles.Count);
    //                         }
    //                     }
    //                 }
    //
    //                 if (y > 0)
    //                 {
    //                     Cell down = grid[x, y - 1];
    //                     if (down.isWater)
    //                     {
    //                         Vector3 a = new Vector3(x - .5f, 0, y - .5f);
    //                         Vector3 b = new Vector3(x + .5f, 0, y - .5f);
    //                         Vector3 c = new Vector3(x - .5f, -1, y - .5f);
    //                         Vector3 d = new Vector3(x + .5f, -1, y - .5f);
    //                         Vector3[] v = new Vector3[] {a, b, c, b, d, c};
    //                         for (int k = 0; k < 6; k++)
    //                         {
    //                             vertices.Add(v[k]);
    //                             triangles.Add(triangles.Count);
    //                         }
    //                     }
    //                 }
    //
    //                 if (y < settings.size - 1)
    //                 {
    //                     Cell up = grid[x, y + 1];
    //                     if (up.isWater)
    //                     {
    //                         Vector3 a = new Vector3(x + .5f, 0, y + .5f);
    //                         Vector3 b = new Vector3(x - .5f, 0, y + .5f);
    //                         Vector3 c = new Vector3(x + .5f, -1, y + .5f);
    //                         Vector3 d = new Vector3(x - .5f, -1, y + .5f);
    //                         Vector3[] v = new Vector3[] {a, b, c, b, d, c};
    //                         for (int k = 0; k < 6; k++)
    //                         {
    //                             vertices.Add(v[k]);
    //                             triangles.Add(triangles.Count);
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //
    //     mesh.vertices = vertices.ToArray();
    //     mesh.triangles = triangles.ToArray();
    //     mesh.RecalculateNormals();
    //
    //     GameObject gridObj = new GameObject("BuildGrid");
    //     gridObj.transform.SetParent(transform);
    //
    //     MeshFilter meshFilter = gridObj.AddComponent<MeshFilter>();
    //     meshFilter.mesh = mesh;
    //
    //     MeshRenderer meshRenderer = gridObj.AddComponent<MeshRenderer>();
    //     meshRenderer.material = general.gridMaterial;
    //
    //     gridObj.transform.localScale = new Vector3(1,0.1f, 1);
    //     gridObj.transform.position = new Vector3(0, 0.1f, 0);
    // }

    void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(settings.size, settings.size);
        Color[] colorMap = new Color[settings.size * settings.size];
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * settings.size + x] = Color.blue;
                else
                    colorMap[y * settings.size + x] = new Color(153 / 255f, 191 / 255f, 115 / 255f);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = general.terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }

    void GenerateWater()
    {
        Instantiate(general.water, transform);
    }

    void GenerateVegetation(Cell[,] grid)
    {
        GameObject VegHolder = new GameObject("vegetation");
        VegHolder.transform.SetParent(transform);

        for (int x = 0; x < settings.size; x++)
        {
            for (int y = 0; y < settings.size; y++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    GameObject vegetation = Instantiate(general.grass, transform);
                    vegetation.transform.position = new Vector3(x, 0, y);
                    vegetation.transform.localScale = Vector3.one * Random.Range(.8f, 1.5f);
                    vegetation.transform.SetParent(VegHolder.transform);

                    // int randRotation;
                    // randRotation = Random.Range(0, 3);
                    // switch (randRotation)
                    // {
                    //     case 0:
                    //         vegetation.transform.rotation = Quaternion.Euler(0, Random.Range(0, 0), 0);
                    //         break;
                    //     case 1:
                    //         vegetation.transform.rotation = Quaternion.Euler(0, Random.Range(0, 90), 0);
                    //         break;
                    //     case 2:
                    //         vegetation.transform.rotation = Quaternion.Euler(0, Random.Range(0, 180), 0);
                    //         break;
                    //     case 3:
                    //         vegetation.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    //         break;
                    //     
                    // }
                }
            }
        }
    }

    void GenerateEnviroment(Cell[,] grid)
    {
        GameObject EnvHolder = new GameObject("Env");
        EnvHolder.transform.SetParent(transform);
        float[,] noiseMap = new float[settings.size, settings.size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * settings.treeNoiseScale + xOffset,
                    y * settings.treeNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < settings.size; y++)
        {
            for (int x = 0; x < settings.size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    float v = Random.Range(0f, settings.treeDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab = components.EnvPrefabs[Random.Range(0, components.EnvPrefabs.Length)];
                        GameObject env = Instantiate(prefab, transform);
                        env.transform.position = new Vector3(x, 0, y);
                        // env.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        env.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                        env.transform.SetParent(EnvHolder.transform);
                    }
                }
            }
        }
    }
}