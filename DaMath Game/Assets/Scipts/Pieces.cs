using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    public bool isPlayer1Color;
    public bool isRed;
    public bool isDama;
    public bool hadOperator;

    public bool isLeft;

    public static Pieces Instance;

    public GameObject myOperator;

    private void OnTriggerEnter(Collider col)
    {
        myOperator = col.gameObject;
        hadOperator = true;
    }
    private void OnTriggerStay(Collider col)
    {
        myOperator = col.gameObject;
        hadOperator = true;
    }
    private void OnTriggerExit(Collider col)
    {
        myOperator = null;
        hadOperator = false;
    }

    private void Start()
    {
        Instance = this;
    }
    

    public bool isForceToMove(Pieces[,] board, int x, int y)
    {

        if (isPlayer1Color || isDama)
        {
            //top left
            if(x >= 2 && y <= 5)
            {
                Pieces p = board[x - 1, y + 1];
                //if there is a piece, and same color piece

            if( p != null && p.isPlayer1Color != isPlayer1Color)
                {
                    //check if its possible to land after jump
                    if (board[x - 2, y + 2] == null)
                        return true;
                }
            }

            //top right
            if (x <= 5 && y <= 5)
            {
                Pieces p = board[x + 1, y + 1];
                //if there is a piece, and same color piece

                if (p != null && p.isPlayer1Color != isPlayer1Color)
                {
                    //check if its possible to land after jump
                    if (board[x + 2, y + 2] == null)
                        return true;
                }
            }

        }

        if(!isPlayer1Color || isDama)
        {
            //bot left
            if (x >= 2 && y >= 2)
            {
                Pieces p = board[x - 1, y - 1];
                //if there is a piece, and same color piece

                if (p != null && p.isPlayer1Color != isPlayer1Color)
                {
                    //check if its possible to land after jump
                    if (board[x - 2, y - 2] == null)
                    {
                        isLeft = true;
                        return true;
                    }

                }
            }

            //bot right
            if (x <= 5 && y >= 2)
            {
                Pieces p = board[x + 1, y - 1];
                //if there is a piece, and same color piece

                if (p != null && p.isPlayer1Color != isPlayer1Color)
                {
                    //check if its possible to land after jump
                    if (board[x + 2, y - 2] == null)
                    {
                        isLeft = false;
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public bool ValidMove(Pieces[,] board, int x1, int y1, int x2, int y2)
    {
        //If piece is moving on top of another piece
        if (board[x2, y2] != null)
            return false;

        int deltaMove = Mathf.Abs(x1 - x2);
        int deltaMoveY = y2 - y1;

        //Player 1 Pieces
        if(isPlayer1Color || isDama)
        {
            if(deltaMove == 1)
            {
                if (deltaMoveY == 1)
                    return true;
            }
            else if(deltaMove == 2)
            {
                if(deltaMoveY == 2)
                {
                    Pieces p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isPlayer1Color != isPlayer1Color)
                        return true;
                }
            }
        }

        //Player 2 Pieces
        if (!isPlayer1Color || isDama)
        {
            if (deltaMove == 1)
            {
                if (deltaMoveY == -1)
                    return true;
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Pieces p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isPlayer1Color != isPlayer1Color)
                        return true;
                }
            }
        }

        return false;

    }
    public void RotateDamaPiece()
    {
        Transform childTransform = transform.Find("Cylinder");

        childTransform.transform.rotation *= Quaternion.Euler(180, 0, 0);
    }
}
