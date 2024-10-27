using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ChessSquare_Maker : MonoBehaviour
{

	[SerializeField] private ChessSquareInfo chessSquareInfo;
    public ChessSquareInfo ChessSquareInfo { get => chessSquareInfo; set => chessSquareInfo = value; }

    private ChessBoard_Maker parentChessBoard;

    private GameObject chessSquareModel;
    private TextMeshPro chessSquareTMP;

    private MeshRenderer gridMeshRenderer;
    private MeshRenderer modelMeshRenderer;
    private Color initialGridColor;
	private Color initialModelColor;

    private InputHandler inputHandler;

    private int occupiedCount = 0;
    public int OccupiedCount 
    { 
        get => occupiedCount; 
        set
		{
            if (occupiedCount == 0 && value == 1)
                ChangeSquareGridColor(Color.yellow * 0.5f);
            if (occupiedCount == 1 && value == 0)
                ChangeSquareGridColor(initialGridColor);

            occupiedCount = value;
        }
    }

    public static readonly Color entranceColor = Color.red;
    public static readonly Color exitColor = Color.green;


	#region Unity Methods

	private void Awake()
	{
        parentChessBoard = transform.GetComponentInParent<ChessBoard_Maker>();

        chessSquareModel = transform.Find("Model").gameObject;
        chessSquareTMP = GetComponentInChildren<TextMeshPro>();

        gridMeshRenderer = transform.Find("TransparentGrid").GetComponentInChildren<MeshRenderer>();
        modelMeshRenderer = chessSquareModel.GetComponentInChildren<MeshRenderer>();
        initialGridColor = gridMeshRenderer.material.color;
        initialModelColor = modelMeshRenderer.material.color;

        //inputHandler = transform.GetComponentInParent<InputHandler>();
        inputHandler = transform.parent.parent.GetComponent<InputHandler>();
    }
	private void Start()
	{
        UpdateSquareTextTo(chessSquareInfo.SquareType);
    }


	private void Reset()
    {
        AllignChessSquareUnderBoard();
    }


    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        ToggleSquareGridEmission(true);

        if (inputHandler.MouseInputHandler.IsButtonDown(MouseButton.Left) &&
            inputHandler.KeyInputHandler.IsKeyDown(KeyCode.LeftControl))
            ToggleSquareActiveStatus();
    }
    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        ToggleSquareGridEmission(false);
    }
	private void OnMouseDown()
	{
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (inputHandler.KeyInputHandler.IsKeyDown(KeyCode.LeftControl))
		{
            ToggleSquareActiveStatus();
            return;
        }

        if (inputHandler.KeyInputHandler.IsKeyDown(KeyCode.S))
		{
            ToggleBoardEntranceForThis();
            return;
        }
        if (inputHandler.KeyInputHandler.IsKeyDown(KeyCode.E))
        {
            ToggleBoardExitForThis();
            return;
        }

        if (chessSquareInfo.IsActive)
		{
            CycleChessSquareType();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // TODO: 추후에는 Layer로 지정하든지 고민 좀 해보기
        if (other.name == "ChessSquareTrigger")
        {
            var makerChessPiece = other.transform.GetComponentInParent<ChessPiece_Maker>();
            var chessPieceType = makerChessPiece.ChessPieceType;

            var occupyingGrids = ChessPieceGridCalculator.CalculateOccupiableChessGrids(chessSquareInfo.Grid, chessPieceType, parentChessBoard.ChessBoardInfo.BoardLength);
            var chessSquares = parentChessBoard.GetChessSquaresOn(occupyingGrids);

            foreach(var chessSquare in chessSquares)
                chessSquare.OccupiedCount++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "ChessSquareTrigger")
        {
            var makerChessPiece = other.transform.GetComponentInParent<ChessPiece_Maker>();
            var chessPieceType = makerChessPiece.ChessPieceType;

            var occupyingGrids = ChessPieceGridCalculator.CalculateOccupiableChessGrids(chessSquareInfo.Grid, chessPieceType, parentChessBoard.ChessBoardInfo.BoardLength);
            var chessSquares = parentChessBoard.GetChessSquaresOn(occupyingGrids);

            foreach (var chessSquare in chessSquares)
                chessSquare.OccupiedCount--;
        }
    }

    //public void Remove


    #endregion


    public void UpdateChessSquare(ChessBoardInfo chessBoardInfo, ChessSquareInfo newChessSquareInfo)
	{
        chessSquareInfo = newChessSquareInfo;

        if (! chessSquareInfo.IsActive)
		{
            chessSquareModel.SetActive(false);
            chessSquareTMP.gameObject.SetActive(false);
            return;
        }

        UpdateSquareTextTo(chessSquareInfo.SquareType);

        if (chessSquareInfo.Grid == chessBoardInfo.EntranceGrid.Grid)
            ChangeSquareModelColor(entranceColor);
        else if (chessSquareInfo.Grid == chessBoardInfo.ExitGrid.Grid)
            ChangeSquareModelColor(exitColor);
    }


	public void AllignChessSquareUnderBoard()
	{
        if (transform.parent.TryGetComponent(out ChessBoard_Maker parentChessBoard))
        {
            chessSquareInfo.Grid = new Vector2Int(transform.GetSiblingIndex() % parentChessBoard.ChessBoardInfo.BoardLength,
                                                                            transform.GetSiblingIndex() / parentChessBoard.ChessBoardInfo.BoardLength);
            this.gameObject.name = GetNameByGrid();
            this.transform.position = GetGridPositionBy(parentChessBoard.transform.position);
        }

        string GetNameByGrid() {   return ("ChessSquare_" + chessSquareInfo.Grid.x + "-" + chessSquareInfo.Grid.y);   }
    }
    private Vector3 GetGridPositionBy(Vector3 leftBottomPos)
    {
        var xPosition = leftBottomPos.x + (10 * chessSquareInfo.Grid.x);
        var zPosition = leftBottomPos.z + (10 * chessSquareInfo.Grid.y);

        return new Vector3(xPosition, leftBottomPos.y, zPosition);
    }


    private void ToggleSquareActiveStatus()
    {
        chessSquareInfo.IsActive = !chessSquareInfo.IsActive;

        chessSquareModel.SetActive(chessSquareInfo.IsActive);
        chessSquareTMP.gameObject.SetActive(ChessSquareInfo.IsActive);

        if(chessSquareInfo.IsActive == false)
		{
            if (parentChessBoard.IsChessBoardExit(chessSquareInfo))
			{
                ChangeSquareModelColor(initialModelColor);
                parentChessBoard.ResetChessBoardEntrance();
            }
            else
            if(parentChessBoard.IsChessBoardExit(chessSquareInfo))
			{
                ChangeSquareModelColor(initialModelColor);
                parentChessBoard.ResetChessBoardExit();
            }
        }
    }

    private void CycleChessSquareType()
	{
        if (chessSquareInfo.SquareType < SquareType.Bonus)
            chessSquareInfo.SquareType++;
        else
            chessSquareInfo.SquareType = SquareType.Common;

        UpdateSquareTextTo(chessSquareInfo.SquareType);
    }

    private void ToggleBoardEntranceForThis()
	{
        if (parentChessBoard.IsChessBoardEntrance(chessSquareInfo))
        {
            parentChessBoard.ResetChessBoardEntrance();
            ChangeSquareModelColor(initialModelColor);
        }
        else 
        if (parentChessBoard.IsChessBoardExit(chessSquareInfo))
        {
            if (TrySetBoardEntranceAsThis())
                parentChessBoard.ResetChessBoardExit();
        }
        else
        {
            TrySetBoardEntranceAsThis();
        }


        bool TrySetBoardEntranceAsThis()
        {
            parentChessBoard.TryGetChessSquareOn(parentChessBoard.ChessBoardInfo.EntranceGrid.Grid, out var previousExitSquare);
            if (parentChessBoard.TrySetChessBoardEntranceAs(chessSquareInfo))
            {
                previousExitSquare?.ChangeSquareModelColor(initialModelColor);
                ChangeSquareModelColor(entranceColor);

                return true;
            }
            return false;
        }
    }
    private void ToggleBoardExitForThis()
    {
        if (parentChessBoard.IsChessBoardExit(chessSquareInfo))
        {
            parentChessBoard.ResetChessBoardExit();
            ChangeSquareModelColor(initialModelColor);
        }
        else
        if (parentChessBoard.IsChessBoardEntrance(chessSquareInfo))
        {
            if (TrySetBoardExitAsThis())
                parentChessBoard.ResetChessBoardEntrance();
        }
        else
        {
            TrySetBoardExitAsThis();
        }


        bool TrySetBoardExitAsThis()
		{
            parentChessBoard.TryGetChessSquareOn(parentChessBoard.ChessBoardInfo.ExitGrid.Grid, out var previousExitSquare);
            if (parentChessBoard.TrySetChessBoardExitAs(chessSquareInfo))
            {
                previousExitSquare?.ChangeSquareModelColor(initialModelColor);
                ChangeSquareModelColor(exitColor);

                return true;
            }
            return false;
        }
    }


    public void ChangeSquareModelColor(Color newColor)
    {
        LitMaterialHandler.ChangeColor(modelMeshRenderer, newColor);
    }
    public void ChangeSquareGridColor(Color newColor)
    {
        LitMaterialHandler.ChangeColor(gridMeshRenderer, newColor);
    }


    private void ToggleSquareGridEmission(bool isOn)
	{
        LitMaterialHandler.Emit(gridMeshRenderer, Color.blue, isOn);
    }

    private void UpdateSquareTextTo(SquareType squareType)
	{
        chessSquareTMP.text = squareType.ToString();
    }


}
