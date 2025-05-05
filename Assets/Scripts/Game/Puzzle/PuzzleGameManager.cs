using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour
{
    public static PuzzleGameManager Instance;
    [Header("Game Elements")]
    [Range(2, 6)]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Camera camera;


    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;

    private Transform draggingPiece = null;
    private Vector3 offset;

    private int piecesCorrect;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        imageTextures = Resources.LoadAll<Texture2D>("JigsawTextures").ToList();

    }
    public void StartPuzzleGame()
    {
        if (imageTextures == null || imageTextures.Count == 0) { 
            GameFiniched(true);
            return;
        }
        
        // Pick a random texture
        int randomIndex = Random.Range(0, imageTextures.Count);
        Texture2D randomTexture = imageTextures[randomIndex];
        gameHolder.position = camera.transform.position;
        gameHolder.position += new Vector3(0, 0, +1f);

        // Start the game with the selected texture
        StartGame(randomTexture);

        // Remove it from the list so it won't be used again
        imageTextures.RemoveAt(randomIndex);
    }
    public void StartGame(Texture2D jigsawTexture)//mazalt na9sa function bch tadi wa7ad m tsawar randomly
    {

        // We store a list of the transform for each jigsaw piece so we can track them later.
        pieces = new List<Transform>();

        // Calculate the size of each jigsaw piece, based on a difficulty setting.
        dimensions = GetDimensions(jigsawTexture, difficulty);

        // Create the pieces of the correct size with the correct texture.
        CreateJigsawPieces(jigsawTexture);

        // Place the pieces randomly.
        Scatter();

        // Update the border to fit the chosen puzzle.
        UpdateBorder();

        //The game start by 0 correct pieces.
        piecesCorrect = 0;
    }

    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero;
        // Difficulty is the number of pieces on the smallest texture dimension.
        // This helps ensure the pieces are as square as possible.
        if (jigsawTexture.width < jigsawTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }
        return dimensions;
    }

    // Create all the jigsaw pieces
    void CreateJigsawPieces(Texture2D jigsawTexture)
    {
        // Calculate piece sizes based on the dimensions.
        height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;

        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                // Create the piece in the right location of the right size.
                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                  (-width * dimensions.x / 2) + (width * col) + (width / 2),
                  (-height * dimensions.y / 2) + (height * row) + (height / 2),
                  -1);
                piece.localScale = new Vector3(width, height, 1f);

                // We don't have to name them, but always useful for debugging.
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                // Assign the correct part of the texture for this jigsaw piece
                // We need our width and height both to be normalised between 0 and 1 for the UV.
                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;
                // UV coord order is anti-clockwise: (0, 0), (1, 0), (0, 1), (1, 1)
                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col, height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));
                // Assign our new UVs to the mesh.
                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;
                // Update the texture on the piece
                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }

    // Place the pieces randomly in the visible area.
    private void Scatter()
    {
        // Calculate the visible orthographic size of the screen.
        float orthoHeight = camera.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = screenAspect * orthoHeight;

        // Ensure pieces are away from the edges.
        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        orthoHeight -= pieceHeight / 2f;
        orthoWidth -= pieceWidth / 2f;

        Vector3 camPos = camera.transform.position;

        // Place each piece randomly in the visible area relative to the camera.
        foreach (Transform piece in pieces)
        {
            float x = Random.Range(camPos.x - orthoWidth, camPos.x + orthoWidth);
            float y = Random.Range(camPos.y - orthoHeight, camPos.y + orthoHeight);
            piece.position = new Vector3(x, y, -9.9f); // Keep original z (or set to -1 if you prefer)
        }
    }



    // Update the border to fit the chosen puzzle.
    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        // Calculate half sizes to simplify the code.
        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;

        // We want the border to be behind the pieces.
        float borderZ = 0f;

        // Set border vertices, starting top left, going clockwise.
        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));

        // Set the thickness of the border line.
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Show the border line.
        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                // Everything is moveable, so we don't need to check it's a Piece.
                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.forward;
            }
        }

        // When we release the mouse button stop dragging.
        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisableIfCorrect();
            draggingPiece.position += Vector3.back;
            draggingPiece = null;
        }

        // Set the dragged piece position to the position of the mouse.
        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //newPosition.z = draggingPiece.position.z;
            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }

    private void SnapAndDisableIfCorrect()
    {
        // We need to know the index of the piece to determine its correct position.
        int pieceIndex = pieces.IndexOf(draggingPiece);

        // Calculate the coordinates of all possible positions
        List<Vector2> allPositions = new List<Vector2>();
        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {
                Vector2 position = new Vector2(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2)
                );
                allPositions.Add(position);
            }
        }

        // Find the closest position to the dragging piece
        Vector2 closestPosition = allPositions[0];
        float minDistance = float.MaxValue;
        int closestPositionIndex = 0;

        for (int i = 0; i < allPositions.Count; i++)
        {
            float distance = Vector2.Distance(draggingPiece.localPosition, allPositions[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPosition = allPositions[i];
                closestPositionIndex = i;
            }
        }

        // If we're close enough to the closest position
        if (minDistance < (width / 2))
        {
            // Check if another piece is already at this position
            Transform occupyingPiece = null;
            foreach (Transform piece in pieces)
            {
                if (piece != draggingPiece && Vector2.Distance(piece.localPosition, closestPosition) < 0.01f)
                {
                    occupyingPiece = piece;
                    break;
                }
            }

            // If another piece occupies the position, swap positions
            if (occupyingPiece != null)
            {
                if (occupyingPiece.GetComponent<BoxCollider2D>().enabled)
                {
                    Vector3 tempPosition = draggingPiece.localPosition;
                    draggingPiece.localPosition = new Vector3(closestPosition.x, closestPosition.y, draggingPiece.localPosition.z);
                    occupyingPiece.localPosition = tempPosition;
                }

            }
            else
            {
                // Nothing occupying, just snap
                draggingPiece.localPosition = new Vector3(closestPosition.x, closestPosition.y, draggingPiece.localPosition.z);
            }

            // Check if this piece is now in the correct position
            // The coordinates of the piece in the puzzle.
            int correctCol = pieceIndex % dimensions.x;
            int correctRow = pieceIndex / dimensions.x;

            // The target position in the non-scaled coordinates.
            Vector2 correctPosition = new Vector2(
                (-width * dimensions.x / 2) + (width * correctCol) + (width / 2),
                (-height * dimensions.y / 2) + (height * correctRow) + (height / 2)
            );

            // If the piece is in its correct position, disable its collider
            if (Vector2.Distance(draggingPiece.localPosition, correctPosition) < 0.01f)
            {
                draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
                draggingPiece.position += Vector3.forward;

                // Increase the number of correct pieces, and check for puzzle completion.
                piecesCorrect++;
                if (piecesCorrect == pieces.Count)
                {
                    GameFiniched(true); // puzzle completed
                }
            }
        }
    }

    private void GameFiniched(bool completed)
    {
        // Play the victory sound
        AudioManager.Instance.PlayCorrectAnswer();
        foreach (Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();

        gameHolder.GetComponent<LineRenderer>().enabled = false;
        GameplayManager.Instance.StartStrategicPhase();
    }

}