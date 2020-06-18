using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Runtime.InteropServices;

public class Board : MonoBehaviour
{
    [Header("Size")]
    [SerializeField]
    private int xSize = 5;
    [SerializeField]
    private int ySize = 7;

    [Header("Tiles")]
    [SerializeField]
    private List<TileType> tileTypes = new List<TileType>();
    [SerializeField, Tooltip("Префаб тайла")]
    private GameObject matchPieceObject = null;
    [SerializeField, Tooltip("Время анимации тайла")]
    private float timeToExplode = 0.1f;

    [Header("Other")]
    [SerializeField, Tooltip("Левый нижний угол доски")]
    private Transform startPosition = null;
    [SerializeField, Tooltip("Использовать ли физику для тайлов. Может багаться")]
    private bool useRigidbody = false;

    public float downtime = 0f;

    private Vector2[] arrayDirrection = new Vector2[4] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    private Coroutine shiftDownTilesCoroutine = null;

    private Tile[][] board;

    private GameManager gameMgr;

    // Инициализация игровой области.
    public void Init(GameManager gameMgr)
    {
        this.gameMgr = gameMgr;

        DestroyAllTiles();

        Vector2 offset = matchPieceObject.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);

        downtime = 0f;
    }

    // Обработка клика по тайлу.
    public void FindAllMatch(Tile tile)
    {
        if (gameMgr.Paused || gameMgr.GameIsOver || shiftDownTilesCoroutine != null)
            return;

        if (tile.InUse)
        {
            downtime = 0f;
            RemoveMatchingTiles(tile, arrayDirrection);
            StartShiftDownTilesCoroutine();
        }
    }

    private void Update()
    {
        if (gameMgr.Paused)
            return;

        downtime += Time.deltaTime;
        if (downtime > gameMgr.GetPromptFrequency)
        {
            ShowPrompt();
        }
    }

    // Формирование игровой области.
    private void CreateBoard(float xOffset, float yOffset)
    {
        if (startPosition == null)
            Debug.LogError("[Board] Start position is empty!");

        float startX = startPosition.transform.position.x;
        float startY = startPosition.transform.position.y;

        TileType[] previousLeft = new TileType[ySize];
        TileType previousBelow = null;

        board = new Tile[xSize][];
        for (int x = 0; x < xSize; x++)
        {
            board[x] = new Tile[ySize];
            for (int y = 0; y < ySize; y++)
            {
                var tile = Instantiate(
                    matchPieceObject,
                    new Vector3(startX + (xOffset * x), startY + (yOffset * y), 2),
                    matchPieceObject.transform.rotation,
                    startPosition).AddComponent<Tile>();

                if (useRigidbody)
                {
                    tile.gameObject.AddComponent<Rigidbody2D>();
                }

                List<TileType> possibleTypes = new List<TileType>();
                possibleTypes.AddRange(tileTypes);

                if (gameMgr.GetComplexity == Complexity.Hard)
                {
                    possibleTypes.Remove(previousLeft[y]);
                    possibleTypes.Remove(previousBelow);
                }

                TileType type = possibleTypes[Random.Range(0, possibleTypes.Count)];

                tile.Init(this, y, x, type, timeToExplode);

                if (gameMgr.GetComplexity == Complexity.Hard)
                {
                    previousLeft[y] = type;
                    previousBelow = type;
                }

                board[x][y] = tile;
            }
        }
    }

    // Подсказка хода.
    private void ShowPrompt()
    {
        downtime = 0f;

        // Выбор случайных элементов.
        List<Tile> randomTiles = new List<Tile>();
        for (int i = 0; i < gameMgr.GetPromptAccuracy; i++)
        {
            randomTiles.Add(board[Random.Range(0, xSize)][Random.Range(0, ySize)]);
        }

        // Выбор лучшего варианта из случайных.
        List<Tile> bestResult = new List<Tile>();
        for (int i = 0; i < randomTiles.Count; i++)
        {
            List<Tile> matchingTiles = new List<Tile>() { randomTiles[i] };
            for (int j = 0; j < arrayDirrection.Length; j++)
            {
                matchingTiles.AddRange(FindMatch(randomTiles[i], arrayDirrection[j]));
            }
            if (matchingTiles.Count > bestResult.Count || bestResult == null)
            {
                bestResult = matchingTiles;
            }
        }

        // Анимация подсказки.
        for (int i = 0; i < bestResult.Count; i++)
        {
            if (bestResult[i].InUse)
            {
                DOTween.Sequence().Append(bestResult[i].transform.DOShakeScale(1f, 0.1f));
            }
        }
    }

    // Удаление совпадающих тайлов.
    private void RemoveMatchingTiles(Tile tile, Vector2[] dirArray)
    {
        List<Tile> tileList = new List<Tile>() { tile };

        for (int i = 0; i < dirArray.Length; i++)
        {
            tileList.AddRange(FindMatch(tile, dirArray[i]));
        }

        if (tileList.Count > 0)
        {
            CalculateResult(tileList.Count);
            ExplodeTiles(tileList);
        }
    }

    // Расчёт жизней и очков за произведённый ход.
    private void CalculateResult(int countMatches)
    {
        if (countMatches == 1)
        {
            gameMgr.UpdateStats(gameMgr.GetHearts - 1, gameMgr.GetScore);
        }
        else if (countMatches > 2)
        {
            gameMgr.UpdateStats(gameMgr.GetHearts + countMatches,
                gameMgr.GetScore + (int)(countMatches * gameMgr.GetTileReward * gameMgr.GetComboMuiltiplier));
        }
    }

    // Поиск совпадающих тайлов в одном направлении.
    private List<Tile> FindMatch(Tile tile, Vector2 dir)
    {
        List<Tile> tileList = new List<Tile>();
        RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, dir);

        while (hit.collider != null &&
            hit.collider.gameObject.GetComponent<Tile>().GetTileType.CompareTo(tile.GetTileType) == 0)
        {
            tileList.Add(hit.collider.gameObject.GetComponent<Tile>());
            hit = Physics2D.Raycast(hit.collider.gameObject.transform.position, dir);
        }
        return tileList;
    }

    // Удаление тайлов.
    private void ExplodeTiles(List<Tile> tileList)
    {
        tileList.ForEach(x => x.Explode(timeToExplode));
    }

    private void DestroyAllTiles()
    {
        Tile[] listOldTiles = FindObjectsOfType<Tile>();
        for (int i = 0; i < listOldTiles.Length; i++)
        {
            Destroy(listOldTiles[i].gameObject);
        }
    }

    private void StartShiftDownTilesCoroutine()
    {
        if (shiftDownTilesCoroutine == null)
            shiftDownTilesCoroutine = StartCoroutine(ShiftDownTiles());
    }

    // Перенос удалённых тайлов наверх.
    private IEnumerator ShiftDownTiles()
    {
        float offset = matchPieceObject.GetComponent<SpriteRenderer>().bounds.size.y;

        for (int x = 0; x < xSize; x++)
        {
            int shifts = 0;
            for (int y = 0; y < ySize; y++)
            {
                if (!board[x][y].InUse)
                {
                    shifts++;
                    continue;
                }

                if (shifts == 0)
                    continue;

                board[x][y].transform.DOMoveY(board[x][y].transform.position.y - (offset * shifts), timeToExplode).SetEase(Ease.InExpo);

                var holder = board[x][y - shifts];

                board[x][y - shifts] = board[x][y];
                board[x][y - shifts].PosY = y - shifts;

                board[x][y] = holder;
                board[x][y].transform.position = board[x][y - shifts].transform.position;
            }
        }

        yield return new WaitForSeconds(timeToExplode);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (board[x][y].InUse)
                    continue;

                board[x][y].Init(this, x, y, tileTypes[Random.Range(0, tileTypes.Count)], timeToExplode);
            }
        }

        yield return new WaitForSeconds(timeToExplode);

        shiftDownTilesCoroutine = null;
    }
}
