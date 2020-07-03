using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileType type;
    public TileType GetTileType => type;

    public int PosX { get; set; }
    public int PosY { get; set; }

    public bool InUse { get; set; }

    private SpriteRenderer ballColorRender;

    private Board board;

    private void Awake()
    {
        ballColorRender = GetComponent<SpriteRenderer>();

        Explode(0);
    }

    public void Init(Board board, int posX, int posY, TileType type, float time)
    {
        this.board = board;

        this.PosX = posX;
        this.PosY = posY;
        this.type = type;

        InUse = true;

        ballColorRender.sprite = type.sprite;
        ballColorRender.color = type.color;

        DOTween.Sequence()
               .Append(transform.DOScale(1f, time))
               .Join(ballColorRender.DOFade(1f, time));
    }

    public void Explode(float time)
    {
        InUse = false;
        DOTween.Sequence()
               .Append(transform.DOScale(0f, time))
               .Join(ballColorRender.DOFade(0f, time));
    }

    private void OnMouseUp()
    {
        if (GameManager.Paused || GameManager.GameIsOver)
            return;

        board.PlaySound();
        board.FindAllMatch(this);
    }
}
