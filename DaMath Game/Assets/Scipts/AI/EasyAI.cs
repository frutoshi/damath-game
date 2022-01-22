using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyAI : MonoBehaviour
{
    //private static bool AiFirstTurn = false;
    private static List<Pieces> selectedPieces;
    private static Pieces selectedPiece;
    private static Pieces previousPiece;
    public static int randomPiece;
    public static Pieces randomSelectedPiece;
    public static int rightAnswer;

    public static int x2 = 0;
    public static int y2 = 0;
    public static int x1 = 0;
    public static int y1 = 0;

    public static void Greedy(Pieces[,] pieces, List<Pieces> forcedPieces)
    {
        randomPiece = 0;
        randomSelectedPiece = null;

        ScanForPossiblePiecesToMove(pieces);

        Debug.Log("eto pinakamagandang pwesto " + randomPiece + " sa pwesto ni: " + randomSelectedPiece);
        Debug.Log("pwpwesto si " + randomSelectedPiece + " sa " + x2 + " at " + y2);

        //checking kung seno mga pwedeng magmove
        foreach (var x in selectedPieces)
        {
            Debug.Log(x.ToString());
        }

    }

    public static void Jump(Pieces[,] pieces, List<Pieces> forcedPieces)
    {
        int x = 0;
        int y = 0;

        selectedPiece = forcedPieces[Random.Range(0, forcedPieces.Count)];

        previousPiece = null;

        Debug.Log("jump");

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == selectedPiece)
                {
                    x = i;
                    y = j;
                }
            }
        }

        Debug.Log("pinili kong forced piece: " + selectedPiece + x + ":" + y);


        if (pieces[x, y] != null && pieces[x, y].isPlayer1Color == false && pieces[x, y].isDama == false)
        {
            //method na titignan ung katabi kung may space
            //pag nasa dulo mga piece

            if (x == 7) //kaliwa lang pwede
            {
                if (pieces[x - 1, y - 1] != null && pieces[x - 1, y - 1].isPlayer1Color) //pag may piece ung katabi -l
                {
                    if (pieces[x - 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x - 1, y - 1];
                    }
                }
            }
            else if (x == 0) //kanan lang pwede
            {
                if (pieces[x + 1, y - 1] != null && pieces[x + 1, y - 1].isPlayer1Color) //pag may space ung katabi -r
                {
                    if (pieces[x + 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x + 1, y - 1];
                    }
                }
            }
            else //both sides
            {
                if (pieces[x - 1, y - 1] != null && pieces[x - 1, y - 1].isPlayer1Color) //pag may piece ung katabi -l
                {
                    if (pieces[x - 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x - 1, y - 1];
                    }
                }

                else if (pieces[x + 1, y - 1] != null && pieces[x + 1, y - 1].isPlayer1Color) //pag may space ung katabi -r
                {
                    if (pieces[x + 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x + 1, y - 1];
                    }
                }
            }
        }
        else
        {
            //method na titignan ung katabi kung may space
            //pag nasa dulo mga piece
            //dama this

            if (x == 7) //kaliwa lang pwede
            {
                if (pieces[x - 1, y + 1] != null && pieces[x - 1, y + 1].isPlayer1Color) //pag may piece ung katabi -l
                {
                    if (pieces[x - 2, y + 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 + 2;

                        previousPiece = pieces[x - 1, y + 1];
                    }
                }
                else if (pieces[x - 1, y - 1] != null && pieces[x - 1, y - 1].isPlayer1Color && y > 0) //pag may piece ung katabi -l
                {
                    if (pieces[x - 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x - 1, y - 1];
                    }
                }
            }
            else if (x == 0) //kanan lang pwede
            {
                if (pieces[x + 1, y + 1] != null && pieces[x + 1, y + 1].isPlayer1Color) //pag may space ung katabi -r
                {
                    if (pieces[x + 2, y + 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 + 2;

                        previousPiece = pieces[x + 1, y + 1];
                    }
                }
                else if (pieces[x + 1, y - 1] != null && pieces[x + 1, y - 1].isPlayer1Color) //pag may space ung katabi -r
                {
                    if (pieces[x + 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 - 2;

                        previousPiece = pieces[x + 1, y - 1];
                    }
                }
            }
            else //both sides
            {
                if (pieces[x - 1, y + 1] != null && pieces[x - 1, y + 1].isPlayer1Color) //pag may piece ung katabi -l
                {
                    if (pieces[x - 2, y + 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 + 2;

                        previousPiece = pieces[x - 1, y + 1];
                    }
                }

                if (pieces[x - 1, y - 1] != null && pieces[x - 1, y - 1].isPlayer1Color && y > 0)
                {
                    if (pieces[x - 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 - 2;
                        y2 = y1 - 2;
                    }
                }


                if (pieces[x + 1, y + 1] != null && pieces[x + 1, y + 1].isPlayer1Color) //pag may space ung katabi -r
                {
                    if (pieces[x + 2, y + 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 + 2;

                        previousPiece = pieces[x + 1, y + 1];
                    }
                }

                if (pieces[x + 1, y - 1] != null && pieces[x + 1, y - 1].isPlayer1Color && y > 0)
                {
                    if (pieces[x + 2, y - 2] == null)
                    {
                        x1 = x;
                        y1 = y;

                        x2 = x1 + 2;
                        y2 = y1 - 2;
                    }
                }
            }
        }

        Debug.Log("tatalon si: " + selectedPiece + x1 + " : " + y1 + ", sa " + previousPiece + x2 + " : " + y2);
        ComputeScore(selectedPiece, previousPiece);


    }

    private static void ComputeScore(Pieces presentPiece, Pieces previousPiece)
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

        if (op == "+")
        {
            rightAnswer = num1 + num2;
        }
        if (op == "-")
        {
            rightAnswer = num1 - num2;
        }
        if (op == "x")
        {
            rightAnswer = num1 * num2;
        }
        if (op == "%")
        {
            rightAnswer = num1 / num2;

        }

        ComputeAnswer.blackPoints += rightAnswer;

    }

    private static List<Pieces> ScanForPossiblePiecesToMove(Pieces[,] pieces) //aalamin ano mga pieces na pwede imove
    {
        //Debug.Log("move na ni black - 1");
        selectedPieces = new List<Pieces>();

        //check all the pieces
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] != null && pieces[x, y].isPlayer1Color == false && pieces[x, y].isDama == false)
                {
                    //method na titignan ung katabi kung may space
                    //pag nasa dulo mga piece

                    if (x == 7) //kaliwa lang pwede
                    {
                        if (pieces[x - 1, y - 1] == null) //pag may space ung katabi
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                    }
                    else if (x == 0) //kanan lang pwede
                    {
                        if (pieces[x + 1, y - 1] == null) //pag may space ung katabi
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                    }
                    else //both sides
                    {
                        bool alreadyAdded = false;

                        if (pieces[x - 1, y - 1] == null) //pag may space ung katabi
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);

                                alreadyAdded = true;
                            }
                        }
                        if (pieces[x + 1, y - 1] == null) //pag may space ung katabi
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);
                            }
                        }
                    }
                }

                if (pieces[x, y] != null && pieces[x, y].isPlayer1Color == false && pieces[x, y].isDama == true) // dama
                {
                    //method na titignan ung katabi kung may space
                    //pag nasa dulo mga piece

                    if (x == 7) //kaliwa lang pwede
                    {
                        if (pieces[x - 1, y + 1] == null) //pag may space ung katabi
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                        else if (pieces[x - 1, y - 1] == null)
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                    }
                    else if (x == 0) //kanan lang pwede
                    {
                        if (pieces[x + 1, y + 1] == null) //pag may space ung katabi
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                        else if (pieces[x + 1, y - 1] == null)
                        {
                            selectedPieces.Add(pieces[x, y]);
                        }
                    }
                    else //both sides
                    {
                        bool alreadyAdded = false;

                        if (pieces[x - 1, y + 1] == null) //pag may space ung katabi
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);

                                alreadyAdded = true;
                            }
                        }
                        /**else if (pieces[x - 1, y - 1] == null)
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);

                                alreadyAdded = true;
                            }
                        }**/


                        if (pieces[x + 1, y + 1] == null) //pag may space ung katabi
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);
                            }
                        }
                        /**else if (pieces[x + 1, y - 1] == null)
                        {
                            if (!alreadyAdded)
                            {
                                selectedPieces.Add(pieces[x, y]);
                            }
                        }**/
                    }
                }
            }

        }


        randomSelectedPiece = selectedPieces[Random.Range(0, selectedPieces.Count)];
        Debug.Log("random to tropa no choice e :" + randomSelectedPiece);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == randomSelectedPiece)
                {
                    x1 = i;
                    y1 = j;

                    if (x1 == 7) // kaliwa
                    {
                        if (pieces[i, j].isDama)
                        {
                            x2 = (x1 + (x1 - 2)) / 2;
                            y2 = (y1 + (y1 + 1)) / 2;
                        }
                        else
                        {
                            x2 = (x1 + (x1 - 2)) / 2;
                            y2 = (y1 + (y1 - 1)) / 2;
                        }

                    }
                    else if (x1 == 0) // kanan
                    {
                        if (pieces[i, j].isDama)
                        {
                            x2 = (x1 + (x1 + 2)) / 2;
                            y2 = (y1 + (y1 + 1)) / 2;
                        }
                        else
                        {
                            x2 = (x1 + (x1 + 2)) / 2;
                            y2 = (y1 + (y1 - 1)) / 2;
                        }
                    }
                    else // both
                    {
                        if ((x1 == 5 || x1 == 6)) //kaliwa lang open
                        {
                            if (pieces[i, j].isDama)
                            {
                                x2 = (x1 + (x1 - 2)) / 2;
                                y2 = (y1 + (y1 + 1)) / 2;
                            }
                            else
                            {
                                x2 = (x1 + (x1 - 2)) / 2;
                                y2 = (y1 + (y1 - 1)) / 2;
                            }
                        }
                        else if ((x1 == 1 || x1 == 2)) //kanan lang open
                        {
                            if (pieces[i, j].isDama)
                            {
                                x2 = (x1 + (x1 + 2)) / 2;
                                y2 = (y1 + (y1 + 1)) / 2;
                            }
                            else
                            {
                                x2 = (x1 + (x1 + 2)) / 2;
                                y2 = (y1 + (y1 - 1)) / 2;
                            }
                        }
                        else // pag parehas may space
                        {
                            int random = Random.Range(0, 1);

                            if (random == 0)
                            {
                                if (pieces[i, j].isDama)
                                {
                                    x2 = (x1 + (x1 - 2)) / 2;
                                    y2 = (y1 + (y1 + 1)) / 2;
                                }
                                else
                                {
                                    x2 = (x1 + (x1 - 2)) / 2;
                                    y2 = (y1 + (y1 - 1)) / 2;
                                }
                            }
                            else
                            {
                                if (pieces[i, j].isDama)
                                {
                                    x2 = (x1 + (x1 + 2)) / 2;
                                    y2 = (y1 + (y1 + 1)) / 2;
                                }
                                else
                                {
                                    x2 = (x1 + (x1 + 2)) / 2;
                                    y2 = (y1 + (y1 - 1)) / 2;
                                }
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("random pwesto ni :" + randomSelectedPiece + " " + x1 + " & " + y1 + " dito : " + x2 + " & " + y2);

        return selectedPieces;
    }
}
