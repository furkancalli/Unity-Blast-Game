using UnityEngine;

public class Block : MonoBehaviour
{
    public int x;
    public int y;
    public int colorIndex; 

    private SpriteRenderer spriteRenderer;
    private Board board;

    void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    public void Init(int _x, int _y, int _colorIndex, Sprite _sprite, Board _board)
    {
        x = _x;
        y = _y;
        colorIndex = _colorIndex;
        board = _board;

        spriteRenderer.sprite = _sprite;
    }

    void OnMouseDown()
    {
        
        if (board != null)
        {
            board.BlockClicked(x, y, colorIndex);
        }
    }

    public void Move(int newX, int newY)
    {
        x = newX;
        y = newY;
        
        transform.position = new Vector2(x, y);
        
    }
}