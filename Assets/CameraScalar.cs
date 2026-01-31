using UnityEngine;

public class CameraScalar : MonoBehaviour
{
   public Board board; 
    public float cameraOffset = -10f;
    public float padding = 2f;
    public float yOffset = 1f;

    void Start()
    {
       
        if (board == null) board = FindFirstObjectByType<Board>();
        
        RepositionCamera();
    }

    void RepositionCamera()
    {
       
        Vector3 tempPosition = new Vector3((board.width - 1) / 2f, (board.height - 1) / 2f, cameraOffset);
        
        
        tempPosition.y += yOffset; 
        
        transform.position = tempPosition;

        
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        
        float targetSize = ((board.width / 2f) + padding) / aspectRatio;
        
        Camera.main.orthographicSize = targetSize;
    }
}
