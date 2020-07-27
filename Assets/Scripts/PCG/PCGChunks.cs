using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGChunks : MonoBehaviour
{
    public Transform Player;
    public Chunk[] ChunkPrefabs;
    public Chunk FirstChunk;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    // Start is called before the first frame update
    void Start()
    {
        spawnedChunks.Add(FirstChunk);
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.position.x> spawnedChunks[spawnedChunks.Count - 1].Exit.position.x-15)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        Chunk newChunk=Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)]);

        newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].Exit.position-newChunk.Enter.localPosition;

        spawnedChunks.Add(newChunk);

        if (spawnedChunks.Count >= 10)
        {
            Destroy(spawnedChunks[0].gameObject);
            spawnedChunks.RemoveAt(0);
        }

    }
}
