using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public int width = 6;
    public int height = 8;
    public GameObject blockPrefab;
    
    [Header("Görseller")]
    public Sprite[] blockSprites; 
    public Sprite[] iconA_Sprites; 
    public Sprite[] iconB_Sprites; 
    public Sprite[] iconC_Sprites; 

    [Header("Kurallar")]
    public int conditionA = 4; 
    public int conditionB = 7; 
    public int conditionC = 9; 

    public Block[,] allBlocks;

    [Header("UI Ayarları")]
    public TMP_Text scoreText; 
    public TMP_Text moveText;  
    
    private int score = 0;
    public int moves = 20; 

    [Header("Game Over Ayarları")]
    public GameObject gameOverPanel; 
    public TMP_Text finalScoreText;  

    void Start()
    {
        allBlocks = new Block[width, height];
        GenerateBoard();
        UpdateBoardIcons();
        scoreText.text = "Score: 0";
        moveText.text = "Moves: " + moves;
    }


    void GenerateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int randomColorIndex = Random.Range(0, blockSprites.Length);
                Sprite randomSprite = blockSprites[randomColorIndex];

                GameObject newBlockObj = Instantiate(blockPrefab, new Vector2(x, y), Quaternion.identity);
                newBlockObj.transform.parent = this.transform;
                newBlockObj.name = "Block [" + x + "," + y + "]";

                Block blockScript = newBlockObj.GetComponent<Block>();
                
               
                blockScript.Init(x, y, randomColorIndex, randomSprite, this);

                allBlocks[x, y] = blockScript;
            }
        }
    }

    public void BlockClicked(int x, int y, int colorIndex)
    {
        if (gameOverPanel.activeInHierarchy || moves <= 0) return;

        System.Collections.Generic.List<Block> matches = new System.Collections.Generic.List<Block>();
        FindMatches(x, y, colorIndex, matches);

        if (matches.Count >= 2)
        {
            moves--; 
            moveText.text = "Moves: " + moves; 

            int gainedScore = matches.Count * 100;
            score += gainedScore;
            scoreText.text = "Score: " + score;

            foreach (Block b in matches)
            {
                DestroyBlock(b.x, b.y);
            }

            StartCoroutine(FillBoardCo());
            
            if (moves <= 0)
            {
                StartCoroutine(GameOverProcess());
            }
        }
    }

    void FindMatches(int x, int y, int targetColor, System.Collections.Generic.List<Block> matches)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return;

        Block currentBlock = allBlocks[x, y];

        if (currentBlock == null || matches.Contains(currentBlock) || currentBlock.colorIndex != targetColor) return;

        matches.Add(currentBlock);

        FindMatches(x + 1, y, targetColor, matches);
        FindMatches(x - 1, y, targetColor, matches);
        FindMatches(x, y + 1, targetColor, matches);
        FindMatches(x, y - 1, targetColor, matches);
    }

    void DestroyBlock(int x, int y)
    {
        Block b = allBlocks[x, y];
        if (b != null)
        {
            allBlocks[x, y] = null; 
            Destroy(b.gameObject);  
        }
    }

    public System.Collections.IEnumerator FillBoardCo()
    {
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allBlocks[x, y] == null)
                {
                    for (int k = y + 1; k < height; k++)
                    {
                        if (allBlocks[x, k] != null)
                        {
                            allBlocks[x, k].Move(x, y);
                            allBlocks[x, y] = allBlocks[x, k];
                            allBlocks[x, k] = null; 
                            break;
                        }
                    }
                }
                
                if (allBlocks[x, y] == null)
                {
                    int randomColorIndex = Random.Range(0, blockSprites.Length);
                    Sprite randomSprite = blockSprites[randomColorIndex];

                    GameObject newBlockObj = Instantiate(blockPrefab, new Vector2(x, height + 2), Quaternion.identity);
                    newBlockObj.transform.parent = this.transform;

                    Block newBlock = newBlockObj.GetComponent<Block>();
                    newBlock.Init(x, y, randomColorIndex, randomSprite, this);
                    
                    newBlockObj.transform.position = new Vector2(x, y);
                    
                    allBlocks[x, y] = newBlock;
                }
            }
            yield return new WaitForSeconds(0.05f); 
        }

        yield return new WaitForSeconds(0.2f); 
        
        UpdateBoardIcons();

        if (IsDeadlocked())
        {
            yield return new WaitForSeconds(0.5f);
            ShuffleBoard();
        }
    }

    void UpdateBoardIcons()
    {
        bool[,] visited = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allBlocks[x, y] == null || visited[x, y]) continue;

                System.Collections.Generic.List<Block> group = new System.Collections.Generic.List<Block>();
                FindMatches(x, y, allBlocks[x, y].colorIndex, group);

                foreach (Block b in group)
                {
                    visited[b.x, b.y] = true;
                }

                int count = group.Count;
                int colorIdx = allBlocks[x, y].colorIndex;
                Sprite newSprite = blockSprites[colorIdx]; 

                if (count > conditionC) newSprite = iconC_Sprites[colorIdx];
                else if (count > conditionB) newSprite = iconB_Sprites[colorIdx];
                else if (count > conditionA) newSprite = iconA_Sprites[colorIdx];

                foreach (Block b in group)
                {
                    b.GetComponent<SpriteRenderer>().sprite = newSprite;
                }
            }
        }
    }

    bool IsDeadlocked()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allBlocks[x, y] != null)
                {
                    int currentColor = allBlocks[x, y].colorIndex;

                    if (x < width - 1 && allBlocks[x + 1, y] != null && allBlocks[x + 1, y].colorIndex == currentColor)
                        return false; 

                    if (y < height - 1 && allBlocks[x, y + 1] != null && allBlocks[x, y + 1].colorIndex == currentColor)
                        return false; 
                }
            }
        }
        return true;
    }

    public void ShuffleBoard()
    {
        System.Collections.Generic.List<Block> allBlockObjects = new System.Collections.Generic.List<Block>();
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allBlocks[x, y] != null)
                {
                    allBlockObjects.Add(allBlocks[x, y]);
                }
            }
        }

        System.Collections.Generic.List<int> colors = new System.Collections.Generic.List<int>();
        foreach (Block b in allBlockObjects)
        {
            colors.Add(b.colorIndex);
        }

        for (int i = 0; i < colors.Count; i++)
        {
            int temp = colors[i];
            int randomIndex = Random.Range(i, colors.Count);
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }

        for (int i = 0; i < allBlockObjects.Count; i++)
        {
            int newColorIdx = colors[i];
            allBlockObjects[i].colorIndex = newColorIdx;
            allBlockObjects[i].GetComponent<SpriteRenderer>().sprite = blockSprites[newColorIdx];
        }

        if (IsDeadlocked())
        {
            int forceColor = allBlocks[0, 0].colorIndex;
            
            allBlocks[1, 0].colorIndex = forceColor;
            allBlocks[1, 0].GetComponent<SpriteRenderer>().sprite = blockSprites[forceColor];
        }

        UpdateBoardIcons();
    }

    System.Collections.IEnumerator GameOverProcess()
    {
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(true);
        finalScoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}