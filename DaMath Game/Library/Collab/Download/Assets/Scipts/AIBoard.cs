using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AIBoard : MonoBehaviour
{
    public static AIBoard Instance { set; get; }

    //For instantating pieces
    public static Pieces[,] pieces = new Pieces[8, 8]; 
    private GameObject whitePiecePrefab;
    private GameObject blackPiecePrefab;
    //array lang ng 2 team
    public GameObject[] whitePieces;
    public GameObject[] blackPieces;

    public GameObject highlightsContainer;

    public CanvasGroup alertCanvas;
    private float lastAlert;
    private bool alertActive;

    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 offsetBlack = new Vector3(-4.5f, 0, -3.5f); //sakto na yang pwesto wag palitan
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f); //pls wag nyo na palitan pwesto

    private Vector3 highlightPieceOffset = new Vector3(0.5f, 0, -0.5f);

    public  bool isWhite;
    private bool isWhiteTurn;
    public  bool hasDestroyed;
    private bool hasJumped;
    private bool hasMultipleJumped;

    private Pieces selectedPiece;
    private Pieces presentPiece;
    private Pieces previousPiece;
    private List<Pieces> forcedPieces;

    private Vector2 touchOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    public GameObject computeCanvas;
    public Text questionUI;

    private int ctrTurn = 0;

    public Animator animPlayerTurn;
    public Animator animComputerTurn;

    public GameObject turnCanvas;
    public GameObject playerTurnUI;
    public GameObject computerTurnUI;
    public GameObject victoryUI;
    public Text winnerText;

    //private LANClient client;


    private void Awake()
    {
        ShuffleArray();
    }
    private void Start()
    {
        foreach(Transform t in highlightsContainer.transform)
        {
            t.position = Vector3.down * 100;
        }


        Instance = this;
        //client = FindObjectOfType<LANClient>();
        //isWhite = client.isHost;

        isWhiteTurn = true;
        forcedPieces = new List<Pieces>();
        GenerateBoard();

        //turnCanvas.SetActive(true);
        //StartCoroutine(PlayAndDisappear("Player"));
        //playerTurnUI.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in highlightsContainer.transform)
        {
            t.Rotate(Vector3.up * 90 * Time.deltaTime);
        }

        //Debug.Log("bagong ikot ulet");


        UpdateAlert();

        //if its player 1 turn
        if (isWhite)// ? isWhiteTurn : !isWhiteTurn)
        {
            int x = (int)touchOver.x;
            int y = (int)touchOver.y;

            if (selectedPiece != null)
                UpdatePieceDrag(selectedPiece);

            if (Input.GetMouseButtonDown(0))
                SelectPiece(x, y);

            if (Input.GetMouseButtonUp(0))
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
        }
        else
        {
            Debug.Log("pumasok ako darleng");

            if (forcedPieces.Count > 0)
                EasyAI.Jump(pieces, forcedPieces);
            else
                EasyAI.Greedy(pieces, forcedPieces);


            TryMove(EasyAI.x1, EasyAI.y1, EasyAI.x2, EasyAI.y2);

           // isWhite = true;

            Debug.Log("done na ako darleng");
            
        }

        UpdateTouch();

        //Debug.Log(touchOver);
    }


    private void SelectPiece(int x, int y)
    {
        //Out of Bounds

        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return;


        Pieces p = pieces[x, y];
        if (p != null && p.isWhite == isWhite)
        {
            if (forcedPieces.Count == 0)
            {
                selectedPiece = p;
                startDrag = touchOver;

                //checking lang to kung anong chip at kung may operator ba
                Debug.Log(selectedPiece.tag);
                Debug.Log(selectedPiece.hadOperator);

            }
            else
            {
                // look
                if (forcedPieces.Find(fp => fp == p) == null)
                    return;

                selectedPiece = p;
                startDrag = touchOver;
            }
        }
    }
    public void ShuffleArray()
    {
        //shuffle white
        for (int i = 0; i < whitePieces.Length; i++)
        {
            int rnd = Random.Range(0, whitePieces.Length);
            GameObject temp = whitePieces[rnd];
            whitePieces[rnd] = whitePieces[i];
            whitePieces[i] = temp;
        }
        //shuffle black
        for (int i = 0; i < blackPieces.Length; i++)
        {
            int rnd = Random.Range(0, blackPieces.Length);
            GameObject temp = blackPieces[rnd];
            blackPieces[rnd] = blackPieces[i];
            blackPieces[i] = temp;
        }
    }
    private void GenerateBoard()
    {
        int ctr = 0;
        //Generate White Pieces
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 1);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate Piece
                GeneratePieces((oddRow) ? x : x + 1, y, ctr);
                ctr++;
            }
        }

        ctr = 0;
        //Generate Black Pieces
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 1);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate Piece
                GeneratePieces((oddRow) ? x : x + 1, y, ctr);
                ctr++;
            }
        }

    }
    private void GeneratePieces(int x, int y, int ctr)
    {
        whitePiecePrefab = whitePieces[ctr] as GameObject;
        blackPiecePrefab = blackPieces[ctr] as GameObject;
        bool isPieceWhite = !(y > 3);
        GameObject gameObj = Instantiate((isPieceWhite) ? whitePiecePrefab : blackPiecePrefab) as GameObject;
        gameObj.transform.SetParent(transform);
        Pieces p = gameObj.GetComponent<Pieces>();
        pieces[x, y] = p;
        MovePieces(p, x, y);
    }
    private void MovePieces(Pieces p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + offsetBlack + pieceOffset;
    }
    private void UpdateTouch()
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            touchOver.x = (int)(hit.point.x - boardOffset.x);
            touchOver.y = (int)(hit.point.z - boardOffset.z);

        }
        else
        {
            touchOver.x = -1;
            touchOver.y = -1;
        }
    }
    private void UpdatePieceDrag(Pieces p)
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            p.transform.position = hit.point + Vector3.up;

        }
    }
    public void TryMove(int x1, int y1, int x2, int y2)
    {
        forcedPieces = ScanForPossibleMove();

        //Multiplayer
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

        //cancelling selected piece move/ out of bounds
        if (x2 < 0 || x2 >= 8 || y2 < 0 || y2 >= 8)
        {
            if (selectedPiece != null)
            {
                MovePieces(selectedPiece, x1, y1);
            }
           
            startDrag = Vector2.zero;
            selectedPiece = null;
            Highlight();
            return;
        }

        if (selectedPiece != null)
        {
            //if it did not move
            if ((endDrag == startDrag))
            {
                MovePieces(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                Highlight();
                return;
            }

            //check if move is valid
            if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
            {
                // if a piece is eaten
                //if this is a jump

                if (Mathf.Abs(x2 - x1) == 2)
                {
                    Pieces p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        presentPiece = selectedPiece;
                        previousPiece = p;
                        Destroy(p.gameObject);
                        hasDestroyed = true;
                        hasJumped = true;
                    }
                }

                //is were supposed to destroy anything
                if (forcedPieces.Count != 0 && !hasDestroyed)
                {
                    MovePieces(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    Highlight();
                    return;
                }

                //piece moved
                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePieces(selectedPiece, x2, y2);
                ctrTurn++;
                Debug.Log("Counter " + ctrTurn);

                EndTurn();

            }

            else
            {
                MovePieces(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                Highlight();
                Debug.Log("PIECE RETURNED");
                return;
            }

        }
        return;
    }
    private void EndTurn()
    {

        int x = (int)endDrag.x;
        int y = (int)endDrag.y;


        if (selectedPiece != null)
        {
            //white piece will become dama
            if (selectedPiece.isWhite && !selectedPiece.isDama && y == 7)
            {
                selectedPiece.isDama = true;    
                selectedPiece.RotateDamaPiece();
            }
            //black piece will become dama
            else if (!selectedPiece.isWhite && !selectedPiece.isDama && y == 0)
            {
                selectedPiece.isDama = true;
                selectedPiece.RotateDamaPiece();
            }
        }

      
        selectedPiece = null;
        startDrag = Vector2.zero;

        if (ScanForPossibleMove(selectedPiece, x, y).Count != 0 && hasDestroyed)
        {
            if (isWhite)
            {
                Jump();
            }
            hasMultipleJumped = true;
            return;
        }

        
        if (hasMultipleJumped && hasDestroyed)
        {
            Debug.Log("Piece Multiple Jumped");
        }


        if (hasJumped || hasMultipleJumped)
        {
            if(isWhite)
                Jump(); //single jump
        }


        isWhiteTurn = !isWhiteTurn;
        isWhite = !isWhite;
        hasDestroyed = false;
        hasMultipleJumped = false;
        hasJumped = false;

        /**if (!isWhiteTurn)
        {
            //turnCanvas.SetActive(true);
            StartCoroutine(PlayAndDisappear("Computer"));
            computerTurnUI.SetActive(true);
        }
        else
        {
            turnCanvas.SetActive(true);
            StartCoroutine(PlayAndDisappear("Player"));
            playerTurnUI.SetActive(true);
        }**/


        //if (isWhite)
        //    Alert("White Player's Turn");
        //else
        //    Alert("Black Player's Turn");

        ScanForPossibleMove();

        //check victory??
        if (ctrTurn == 40)
        {
            CheckVictory();
            ctrTurn = 0;
        }
    }
    public void Jump()
    {
        //kinukuha ko mga numbers at operators 
        string numOne = presentPiece.name.Substring(0, 2);
        Debug.Log(numOne);
        string numTwo = previousPiece.name.Substring(0, 2);
        Debug.Log(numTwo);

        int num1 = int.Parse(numOne);
        int num2 = int.Parse(numTwo);

        string op = previousPiece.myOperator.name;

        if (op == "a")
        {
            op = "+";
        }
        if (op == "s")
        {
            op = "-";
        }
        if (op == "m")
        {
            op = "x";
        }
        if (op == "d")
        {
            op = "%";
        }

        //check kung tama ung question
        Debug.Log(num1 + " " + op + " " + num2);

        ComputeAnswer.Compute(num1, num2, op, "white");

        questionUI.text = num1 + " " + op + " " + num2 + " = ?";
        computeCanvas.SetActive(true);
        return;
    }
    private void CheckVictory()
    {
        var ps = FindObjectsOfType<Pieces>();
        bool hasWhite = false, hasBlack = false;
        for (int i = 0; i < ps.Length; i++)
        {
            if (ps[i].isWhite)
                hasWhite = true;
            else
                hasBlack = true;
        }

        if (!hasWhite)
            Victory(false);

        if (!hasBlack)
            Victory(true);

        //engk ko titignan kung seno nanalo
        if(ComputeAnswer.whitePoints > ComputeAnswer.blackPoints)
        {
            Time.timeScale = 1f;
            victoryUI.SetActive(true);
            winnerText.text = "WHITE";
            Debug.Log("the winner is : WHITE");
        }
        else
        {
            Time.timeScale = 1f;
            victoryUI.SetActive(true);
            winnerText.text = "BLACK";
            Debug.Log("the winner is : BLACK");
        }
           
    }
    private void Victory(bool isWhite)
    {
        if (isWhite)
        {
            Debug.Log("White Team has won");
        }
        else
        {
            Debug.Log("Black team has won");
        }
    }
    private List<Pieces> ScanForPossibleMove(Pieces p, int x, int y)
    {
        forcedPieces = new List<Pieces>();
        //piece move
        if (pieces[x, y].isForceToMove(pieces, x, y))
            forcedPieces.Add(pieces[x, y]);
        Debug.Log("Piece Moved");
        Highlight();

        return forcedPieces;
    }
    private List<Pieces> ScanForPossibleMove()
    {
        forcedPieces = new List<Pieces>();

        //check all the pieces
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (pieces[i, j] != null && pieces[i, j].isWhite == isWhiteTurn)
                    if (pieces[i, j].isForceToMove(pieces, i, j))
                    {
                        //FORCED MOVE UNG PIECE
                        forcedPieces.Add(pieces[i, j]);
                        //Debug.Log("Jump Piece" + " " );
                    }

        Highlight();
        return forcedPieces;
    }
    private void Highlight()
    {
        foreach (Transform t in highlightsContainer.transform)
        {
            t.position = Vector3.down * 100;
        }

        if (forcedPieces.Count > 0)
        {
            highlightsContainer.SetActive(true);
            highlightsContainer.transform.GetChild(0).position = forcedPieces[0].transform.position + Vector3.up * 0.015f + highlightPieceOffset;
            Debug.Log("Jump Piece" + " " + forcedPieces[0].name);
        }
            

        if (forcedPieces.Count > 1)
        {
            highlightsContainer.SetActive(true);
            highlightsContainer.transform.GetChild(1).position = forcedPieces[1].transform.position + Vector3.up * 0.015f + highlightPieceOffset;
            Debug.Log("Jump Piece" + " " + forcedPieces[1].name);
        }
    }
    public void Alert(string text)
    {
        alertCanvas.GetComponentInChildren<TMP_Text>().text = text;
        alertCanvas.alpha = 1;
        lastAlert = Time.time;
        alertActive = true;
    }
    public void UpdateAlert()
    {
        if (alertActive)
        {
            if(Time.time - lastAlert > 1.5)
            {
                alertCanvas.alpha = 1 - ((Time.time - lastAlert) - 1.5f);
                if(Time.time - lastAlert > 2.5f)
                {
                    alertActive = false;
                }
            }
        }
    }
    IEnumerator PlayAndDisappear(string ani)
    {
        if (ani == "Player")
        {
            yield return new WaitForSeconds(2f);

            computerTurnUI.SetActive(false);
            animPlayerTurn.SetTrigger("Closing");

            yield return new WaitForSeconds(2f);

            playerTurnUI.SetActive(false); // deactivate object
        }
        if (ani == "Computer")
        {
            yield return new WaitForSeconds(2f);

            playerTurnUI.SetActive(false);
            animComputerTurn.SetTrigger("Closing");

            yield return new WaitForSeconds(2f);

            computerTurnUI.SetActive(false); // deactivate object

            StartCoroutine(PlayAndDisappear("Turn"));
        }
        if (ani == "Turn")
        {
            yield return new WaitForSeconds(2f);
            turnCanvas.SetActive(false); // deactivate object
        }
    }
}

