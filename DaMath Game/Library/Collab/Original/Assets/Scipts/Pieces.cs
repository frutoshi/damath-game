using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceNumber
{
    Piece_0 = 0,
    Piece_1 = 1,
    Piece_2 = 2,
    Piece_3 = 3,
    Piece_4 = 4, 
    Piece_5 = 5,
    Piece_6 = 6,
    Piece_7 = 7,
    Piece_8 = 8,
    Piece_9 = 9,
    Piece_10 = 10,
    Piece_11 = 11,
}

public class Pieces : MonoBehaviour
{
    public bool isWhite;
    public bool isDama;

    public bool isForceToMove(Pieces[,] board, int x, int y)
    {

        if (isWhite || isDama)
        {
            //top left
            if(x >= 2 && y <= 5)
            {
                Pieces p = board[x - 1, y + 1];
                //if there is a piece, and same color piece

            if( p != null && p.isWhite != isWhite)
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

                if (p != null && p.isWhite != isWhite)
                {
                    //check if its possible to land after jump
                    if (board[x + 2, y + 2] == null)
                        return true;
                }
            }

        }

        if(!isWhite || isDama)
        {
            //bot left
            if (x >= 2 && y >= 2)
            {
                Pieces p = board[x - 1, y - 1];
                //if there is a piece, and same color piece

                if (p != null && p.isWhite != isWhite)
                {
                    //check if its possible to land after jump
                    if (board[x - 2, y - 2] == null)
                        return true;
                }
            }

            //bot right
            if (x <= 5 && y >= 2)
            {
                Pieces p = board[x + 1, y - 1];
                //if there is a piece, and same color piece

                if (p != null && p.isWhite != isWhite)
                {
                    //check if its possible to land after jump
                    if (board[x + 2, y - 2] == null)
                        return true;
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

        //White Pieces
        if(isWhite || isDama)
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
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        //Black Piece
        if (!isWhite || isDama)
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
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        return false;

    }

        /*
    //Color of Piece
    public int colorPiece;
    public PieceNumber pieceNum;

    //Position of Piece
    public int currentX;
    public int currenty;

    //position of current piece and where it should be in the next move
    private Vector3 desiredPosition;

    //when eaten, the piece will shrink
    private Vector3 desiredScale;
    */
}
