using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OperationNumber
{
    Addition = 0,
    Subtraction = 1,
    Multiplication = 2,
    Division = 3,
  
}

public class Operations : MonoBehaviour
{
    //Color of Piece
    public int colorPiece;

    //Position of Piece
    public int currentX;
    public int currenty;

    //position of current piece and where it should be in the next move
    private Vector3 desiredPosition;

    //when eaten, the piece will shrink
    private Vector3 desiredScale;
}
