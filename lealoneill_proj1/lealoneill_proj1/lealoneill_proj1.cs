// Project 1: Connect 4
// CS 471 
// Hugh O'Neill
// Nicholas Leal


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class lealoneill_proj1
    {

        static void Main(string[] args)
        {
            // create a new 6x7 board
            char[,] board = new char[6, 7];
            // an array to hold the possible coloumn the ai can chose
            int[] possibleMoves = new int[7];

            // fill up the initial board with *
            fillBoard(board);

            // check if the board is full
            if (checkFullBoard(board) == true)
            {
                Console.WriteLine("The board is full!");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // get the first player move
            int playerMove = getPlayerMove();
            while (playerMove != 8)
            {
                // add the players move to the board
                addMove(playerMove, board, 'X', true);

                //check win
                int result = gameResult(board);

                if (result == 0)
                {
                    Console.Write("DRAW!");
                    Console.ReadLine();
                    return;
                }

                else if (result == 1)
                {
                    Console.Write("AI WINS!");
                    Console.ReadLine();
                    return;
                }

                else if (result == 2)
                {
                    Console.WriteLine("YOU WIN!");
                    fillBoard(board);
                    Console.ReadLine();
                    return;
                }

                bool isWin = false;

                // ai loops through every column and evaulutes the value of each one
                // check to see if there is a win available for the AI then take it
                for (int i = 0; i < 7; i++)
                {
                    // finds the position for the play in each column (piece falls down the column until it hits another move)
                    char[,] childBoard = (char[,])board.Clone();
                    addMove(i, childBoard, 'O', false);

                    int win = gameResult(childBoard);
                    if (win == 1 || win == 2)
                    {
                        addMove(i, board, 'O', false);
                        isWin = true;
                        break;

                    }

                    // run the current board through alpha beta pruning determining the value of the move looking at a depth of (5)
                    int test = abPrune(childBoard, 5, -1000000, 1000000, true);

                    // add the determined value of the move to an array holding each columns value
                    possibleMoves[i] = test;
                }

                // check if there is a win available for the player, then block it
                for (int i = 0; i < 7; i++)
                {
                    // finds the position for the play in each column (piece falls down the column until it hits another move)

                    char[,] childBoard = (char[,])board.Clone();
                    addMove(i, childBoard, 'X', false);


                    int win = gameResult(childBoard);
                    if (win == 1 || win == 2)
                    {
                        addMove(i, board, 'O', false);
                        isWin = true;
                        break;

                    }

                }

                if (isWin == false) { 
                // find the best move from the array of possible moves (the move with the highest herusitic value) 
                    int aiMove = getBestAIMove(possibleMoves);

                    // add that move to the real board 

                    addMove(aiMove, board, 'O', false);
                }

                //fill the board
                fillBoard(board);
                

                // check win
				result = gameResult (board);


				if (result == 0) 
				{
					Console.Write ("DRAW!");
                    Console.ReadLine();
                    return;
				}

				else if(result == 1)
				{
					Console.Write ("AI WINS!");
                    Console.ReadLine();
                    return;
				}

				else if (result == 2) 
				{
					Console.Write ("YOU WIN!");
                    Console.ReadLine();
                    return;
				}

                // get a new player move and loop for now 
                playerMove = getPlayerMove();

            }

            // placehold to keep console open
            string end = Console.ReadLine();
        }

        // get the best ai move from the array of values for each column
        static int getBestAIMove(int[] moves)
        {
            //set the new best move and column to the front for now
            int best = moves[0]; // the highest heruistic value
            int bestCol = 0; // the is the column the ai will chose to make its move in

            // loop through each cell of the array and if a move has a higher value, make it the new best move and that column the best column
            for(int i = 0; i < 7; i++)
            {
                if (best < moves[i])
                {
                    best = moves[i];
                    bestCol = i;
                }
                    
            }

            // track if there are moves with same max heuristic value
            int tiedBest = 0;

            // see if there are identical values in the moves 
            for (int i = 0; i < 7; i++)
            {
                if (best == moves[i])
                    tiedBest++;
            }

            // if there are multiple moves with the same value, choose a random one
            if (tiedBest > 1)
            {
                Random rnd = new Random();
                int temp = rnd.Next(0, 7);

                while (moves[temp] != best) { 
                    temp = rnd.Next(0, 7);
                }
                bestCol = temp; 
            }

            // return the column that the ai will makes its move in
            return bestCol;
        }

        // get a move from the player, read in a move from 1 - 7, then sub 1 to make it between 0 - 6 for the array 
        static int getPlayerMove()
        {
            int playerMove = 8;
            Console.WriteLine("Player please choose a column (1 - 7) to play, or 8 to quit:");
            try
            {
                playerMove = Convert.ToInt16(Console.ReadLine());
            }
            catch { Environment.Exit(1); };
            if (playerMove == 8)
                Environment.Exit(1);
            if (playerMove < 1 || playerMove > 7)
                Console.WriteLine("Not a valid move.");
            return playerMove - 1;
        }

        // add a move to the board
        static char[,] addMove(int playerMove, char[,] board, char play, bool human)
        {
            // loop down a column until you hit a move then add the new move above it
           int i = 0;
           if (board[0, playerMove] != '*')
           {
                if (human == true)
                {
                    Console.WriteLine("That column is full!");
                }
                return board;
           }

           while(i < 6 && board[i, playerMove] == '*' )
           {
                i++;
           }

            if (i > 0)
            {
                i--;
            }

           board[i, playerMove] = play;
           return board;
        }

        // fill the board with * for blank spaces, X for player moves, O for AI moves, then display
        static void fillBoard(char[,] board)
        {
            int rows = 6, columns = 7, i, j;

            for (i = 0; i < rows; i++)
            {
                Console.Write("|");
                for (j = 0; j < columns; j++)
                {
                    if (board[i, j] != 'X' && board[i, j] != 'O')
                        board[i, j] = '*';

                    Console.Write(board[i, j]);

                }

                Console.Write("| \n");
            }

        }

        // check if a the board is full ( just check top value in each column to see if it has a player or AI play in it)
        static bool checkFullBoard(char[,] board)
        {
            int checkFull = 0;
            for(int i = 0; i < 7; i++)
            {
                if (board[0, i] == 'X' || board[0, i] == 'O')
                    checkFull++;
            }

            if (checkFull == 6)
                return true;
            else
                return false;
        }
	


        // alpha beta pruning: takes a current board, a depth to check, alpha, beta, and wether or not the current move is from the maximizing player (should be the human)
        // returns the overall heuristic value of the passed move
        static int abPrune(char[,] board, int depth, int a, int b, bool maxPlayer)
        {
            int hvalue;

            // check win
            int win = gameResult(board);
            if (win == 1 || win == 2) {
                return 100;
            }

            //check depth
            if (depth == 0) { return evaluateBoard(board); } // if depth 0 or win, return h value **CHECK FOR A WIN HERE**
           
            // if maximizing player is true
            if (maxPlayer)
            {
                // start a new negative herustic value ( need to check before now if the current board is a win)
                hvalue = -1000000;

                // loop through each play that can be made in the next step of the game
                for (int i = 0; i < 7; i++)
                {

                    char[,] childBoard = (char[,])board.Clone();
                    addMove(i, childBoard, 'X', false);

                    // check win
                    win = gameResult(childBoard);
                    if (win == 1 || win == 2)
                    {
                        return 100;
                    }

                    // get the heurstic value of the current board
                    hvalue = evaluateBoard(childBoard);
                    // compare that against moves at a lower depth recursively
                    hvalue = Math.Max(hvalue, abPrune(childBoard, depth - 1, a, b, false));
                    // get the alpha value
                    a = Math.Max(a, hvalue);
                    // check against beta and determin wether to break out of the current branch or continue
                    if (b <= a)
                        break;
                }

                return hvalue;
            }
            // if maximizing player is false 
            else
            {
                // start a new positive heuristic value
                hvalue = 1000000;

                // loop through all the possible moves that can be made
                for (int i = 0; i < 7; i++)
                {

                    char[,] childBoard = (char[,])board.Clone();
                    addMove(i, childBoard, 'O', false);

                    // check win
                    win = gameResult(childBoard);
                    if (win == 1 || win == 2)
                    {
                        return 100;
                    }

                    // get the hueristic value of the new board
                    hvalue = evaluateBoard(childBoard);
                    // get the min value of the heuristic or a lower depth recursively
                    hvalue = Math.Min(hvalue, abPrune(childBoard, depth - 1, a, b, true));
                    // set beta to the min of b and the heuristic value
                    b = Math.Min(b, hvalue);
                    // determine wether to break the break or keep checking 
                    if (b <= a)
                        break;
                }

                return hvalue;
            }


        }

		//Game Result
		static int gameResult(char[,] board){
			int aiScore = 0, humanScore = 0;
			for(int i=5;i>=0;--i){
				for(int j=0;j<=6;++j){
					if(board[i,j]== '*') continue;

					//Checking cells to the right
					if(j<=3){
						for(int k=0;k<4;++k){
							if(board[i,j+k]== 'O') aiScore++;
							else if(board[i,j+k]== 'X') humanScore++;
							else break; 
						}
						if(aiScore==4)return 1; else if (humanScore==4)return 2;
						aiScore = 0; humanScore = 0;
					} 

					//Checking cells up
					if(i>=3){
						for(int k=0;k<4;++k){
							if(board[i-k,j]== 'O') aiScore++;
							else if(board[i-k,j]== 'X') humanScore++;
							else break;
						}
						if(aiScore==4)return 1; else if (humanScore==4)return 2;
						aiScore = 0; humanScore = 0;
					} 

					//Checking diagonal up-right
					if(j<=3 && i>= 3){
						for(int k=0;k<4;++k){
							if(board[i-k,j+k]=='O') aiScore++;
							else if(board[i-k,j+k]== 'X') humanScore++;
							else break;
						}
						if(aiScore==4)return 1; else if (humanScore==4)return 2;
						aiScore = 0; humanScore = 0;
					}

					//Checking diagonal up-left
					if(j>=3 && i>=3){
						for(int k=0;k<4;++k){
							if(board[i-k,j-k]=='O') aiScore++;
							else if(board[i-k,j-k]=='X') humanScore++;
							else break;
						} 
						if(aiScore==4)return 1; else if (humanScore==4)return 2;
						aiScore = 0; humanScore = 0;
					}  
				}
			}

			for(int j=0;j<7;++j){
				//Game has not ended yet
				if(board[0,j]== '*')return -1;
			}
			//Game draw!
			return 0;
		}

        
		//heuristic to calculate the score 
        static int calculateScore(int aiScore, int moreMoves)

        {

            int moveScore = 4 - moreMoves;

            if (aiScore == 0) return 0;

            else if (aiScore == 1) return 1 * moveScore;

            else if (aiScore == 2) return 10 * moveScore;

            else if (aiScore == 3) return 100 * moveScore;

            else return 1000;

        }


        //Evaluate how favorable board is for ai. Go through each column of board and everytime calculate an overall score for ai
		//the higher the score the better the board is for ai

        public static int evaluateBoard(char[,] board)
        {


            int aiScore = 1;

            int score = 0;

            int blanks = 0;

            int k = 0, moreMoves = 0;

			//loop for 6 by 7 board
            for (int i = 5; i >= 0; --i)
            {

                for (int j = 0; j <= 6; ++j)
                {


                    if (board[i, j] == '*' || board[i, j] == 'X') continue;


                    if (j <= 3)
                    {
						
                        for (k = 1; k < 4; ++k)
                        {
							//every time ai has consecutive pieces from left to right its score goes up
                            if (board[i, j + k] == 'O') aiScore++;

							//every time human blocks the ai the score goes to 0 and blanks go to 0
                            else if (board[i, j + k] == 'X') { aiScore = 0; blanks = 0; break; }

                            else blanks++;

                        }


                        moreMoves = 0;
						//if there are more spots open from left to right
                        if (blanks > 0)

                            for (int c = 1; c < 4; ++c)
                            {

                                int column = j + c;

                                for (int m = i; m <= 5; m++)
                                {
									//more moves will increment every time there is an * in that given column
                                    if (board[m, column] == '*') moreMoves++;

                                    else break;

                                }

                            }

						//higher score means higher favorableness for ai
                        if (moreMoves != 0) score += calculateScore(aiScore, moreMoves);

                        aiScore = 1;

                        blanks = 0;

                    }
					//continue check of each column using same technique above
                    if (i >= 3)
                    {

                        for (k = 1; k < 4; ++k)
                        {

                            if (board[i - k, j] == 'O') aiScore++;

                            else if (board[i - k, j] == 'X') { aiScore = 0; break; }

                        }

                        moreMoves = 0;


                        if (aiScore > 0)
                        {

                            int column = j;

                            for (int m = i - k + 1; m <= i - 1; m++)
                            {

                                if (board[m, column] == 'O') moreMoves++;

                                else break;

                            }

                        }

                        if (moreMoves != 0) score += calculateScore(aiScore, moreMoves);

                        aiScore = 1;

                        blanks = 0;

                    }

                    if (j >= 3)
                    {

                        for (k = 1; k < 4; ++k)
                        {

                            if (board[i, j - k] == 'O') aiScore++;

                            else if (board[i, j - k] == 'X') { aiScore = 0; blanks = 0; break; }

                            else blanks++;

                        }

                        moreMoves = 0;

                        if (blanks > 0)

                            for (int c = 1; c < 4; ++c)
                            {

                                int column = j - c;

                                for (int m = i; m <= 5; m++)
                                {

                                    if (board[m, column] == '*') moreMoves++;

                                    else break;

                                }

                            }


                        if (moreMoves != 0) score += calculateScore(aiScore, moreMoves);

                        aiScore = 1;

                        blanks = 0;

                    }

                    if (j <= 3 && i >= 3)
                    {

                        for (k = 1; k < 4; ++k)
                        {

                            if (board[i - k, j + k] == 'O') aiScore++;

                            else if (board[i - k, j + k] == 'X') { aiScore = 0; blanks = 0; break; }

                            else blanks++;

                        }

                        moreMoves = 0;

                        if (blanks > 0)
                        {

                            for (int c = 1; c < 4; ++c)
                            {

                                int column = j + c, row = i - c;

                                for (int m = row; m <= 5; ++m)
                                {

                                    if (board[m, column] == '*')

                                        moreMoves++;

                                    else if (board[m, column] == 'O')

                                        ;

                                    else break;

                                }

                            }

                            if (moreMoves != 0) score += calculateScore(aiScore, moreMoves);

                            aiScore = 1;

                            blanks = 0;

                        }

                    }

                    if (i >= 3 && j >= 3)
                    {

                        for (k = 1; k < 4; ++k)
                        {

                            if (board[i - k, j - k] == 'O') aiScore++;

                            else if (board[i - k, j - k] == 'X') { aiScore = 0; blanks = 0; break; }

                            else blanks++;

                        }

                        moreMoves = 0;

                        if (blanks > 0)
                        {

                            for (int c = 1; c < 4; ++c)
                            {

                                int column = j - c, row = i - c;

                                for (int m = row; m <= 5; ++m)
                                {

                                    if (board[m, column] == '*')

                                        moreMoves++;

                                    else if (board[m, column] == 'O')

                                        ;

                                    else
                                    {

                                        break;

                                    }

                                }

                            }

                            if (moreMoves != 0) score += calculateScore(aiScore, moreMoves);

                            aiScore = 1;

                            blanks = 0;

                        }

                    }

                }

            }
		
            return score;

        }

        




    }
}
