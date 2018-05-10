using System;
using System.Collections.Generic;

namespace Chess
{

    public class board
    {
        
        public List<square> total_board;
        public int dimension;
        public int tot_squares;
        public Dictionary<string, int> Occ_dict;

        public board(List<square> sq_list, int dim)// 1 arg constructor, takes a List<square> if one is available
        {
            total_board = sq_list;
            dimension = dim;
            tot_squares = dimension * dimension;
            Occ_dict = new Dictionary<string, int>();
        }

        public board(int dim)//0 arg constructor, makes new List<square>
        {
            total_board = new List<square>();
            dimension = dim;
            tot_squares = dimension * dimension;
            Occ_dict = new Dictionary<string, int>();
        }

        public void Add_sq(square sq)//Adds a Square to the back of the list 
        {
            if (total_board.Count < tot_squares)
            {
                total_board.Add(sq);
            }
            else { Console.WriteLine("Attempting to Add square to full board");}       
        }

        public void Add_Mpos(string position, int ind)
        {
            Occ_dict.Add(position, ind);
        }


        public void Display()//Display board
        {
            List<String> Let_list = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
            List<String> Num_list = new List<string> { "8", "7", "6", "5", "4", "3", "2", "1" };

            Console.WriteLine();
            for( int count1 = 0; count1 < dimension; count1++ )
            {
                Console.WriteLine();
                Console.WriteLine(" |");
                Console.Write(Num_list[count1] + "|");
                for( int count2 = 0; count2 < dimension; count2++)
                {
                    Console.Write(total_board[count1*dimension + count2].Print());
                } 
            }
            Console.WriteLine("\n     _____________________________________");
            Console.WriteLine("     A    B    C    D    E    F    G    H");
        }
    }
    
    
    public class square
    {
        public int index;
        public String Position;
        public String State;

        public square(string iPosition, String iState = "__")
        {
            this.Position = iPosition.ToUpper();
            this.State = iState.ToUpper();
        }

        public bool IsWhite()//if Square is occupied by a white piece, return true, if black or nothing, return false
        {
            if(State[0] == 'W') { return true; }

            else { return false; }
        }

        public bool IsBlack()//if Square is occupied by a black piece, return false, if white or nothing, return false
        {
            if(State[0] == 'B') { return true; }

            else { return false; }
        }

        public bool IsEmpty()
        {
            if(State[0] == '_') { return true; }

            else { return false; }
        }

        public string GetPos()
        {
            return Position;
        }

        public string GetState()
        {
            return State;
        }

        public void SetPos(string setPos)
        {
            this.Position = setPos;
        }

        public void SetState(string setState)
        {
            this.State = setState;
        }

        public string Print()
        {
            string print;
            /*if (State == "WP")
            {
                print = string.Format("{0,5}", '♙');

            }

            else
            */
                print = string.Format("{0,5}", State);
            
            return print;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();


            string user_move = "";
            string start_str = "";
            string end_str = "";
            bool white_turn = true;
            bool move_bool = false;
            string Help = "White starts first, to make a move, enter the square you would like to move your piece from,\nfollowed by the square you would like to move your piece to. For Example, E2E3, moves your E pawn up one square.To show this message again, enter HELP \nTo leave the program, enter EXIT \nand remember, always have fun!!";

            board Classic_Test = Classic_board();
            Console.WriteLine("Welcome to C# Chess!!");
            Console.WriteLine(Help);

            while (user_move != "EXIT") {

                Classic_Test.Display();

                bool Black_Win = true;
                bool White_Win = true;

                foreach (square elem in Classic_Test.total_board)//check for promotions or white/black wins
                {
                    if (elem.GetState() == "WK")
                    {
                        Black_Win = false;//and this
                    }

                    if (elem.GetState() == "BK")
                    {
                        White_Win = false;//do something with this
                    }

                    if (elem.State == "WP" && elem.Position[1] == '8')
                    {
                        Promote(elem);
                        Classic_Test.Display();
                    }

                    if (elem.GetState() == "BP" && elem.Position[1] == '1')
                    {
                        Promote(elem);
                        Classic_Test.Display();
                    }
                }

                if (Black_Win == true)
                {
                    Console.WriteLine("\n Congratulations Black, You captured the White King and are victorius!!!");
                    Console.ReadLine();
                    break;
                }

                if (White_Win == true)
                {
                    Console.WriteLine("\n Congratulations White, You captured the Black King and are victorius!!!");
                    Console.ReadLine();
                    break;
                } 

                if(white_turn == true)
                {
                    Console.Write("Make your move, White: ");
                }

                else { Console.Write("Make your move, Black: "); }

                user_move = Console.ReadLine().ToUpper();

                if (user_move == "EXIT")
                {
                    break;
                }

                if (user_move == "HELP")
                {
                    Console.WriteLine(Help);
                    continue;
                }

                if(user_move.Length != 4)
                {
                    Console.WriteLine("Input should only be 4 charachters long, try again!");
                    continue;
                }

                start_str = user_move.Substring(0, 2);
                end_str = user_move.Substring(2, 2);
                int start_ind, end_ind;
                
                try { 
                    start_ind = Classic_Test.Occ_dict[start_str];
                    end_ind = Classic_Test.Occ_dict[end_str];
                }

                catch (KeyNotFoundException ex)
                {
                    Console.WriteLine("Incorrect input format, please try again!");
                    continue;
                }

                square start_sq = Classic_Test.total_board[start_ind];//sq has pos(A4) and state(__/WP/BN)
                square end_sq = Classic_Test.total_board[end_ind];

                if ((start_sq.State[0] == 'W' && white_turn == true) || (start_sq.State[0] == 'B' && white_turn == false))
                {
                    if (start_sq.State == "WP")
                    {
                        move_bool = Move_WPawn(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if (start_sq.State == "BP")
                    {
                        move_bool = Move_BPawn(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if(start_sq.State == "WN")
                    {
                        move_bool = Move_WKnight(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if (start_sq.State == "BN")
                    {
                        move_bool = Move_BKnight(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if (start_sq.State == "WR")
                    {
                        move_bool = Move_WRook(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else if (start_sq.State == "BR")
                    {
                        move_bool = Move_BRook(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else if (start_sq.State == "WK")
                    {
                        move_bool = Move_WKing(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if (start_sq.State == "BK")
                    {
                        move_bool = Move_BKing(start_sq, end_sq, start_ind, end_ind);
                    }

                    else if (start_sq.State == "WB")
                    {
                        move_bool = Move_WBishop(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else if (start_sq.State == "BB")
                    {
                        move_bool = Move_BBishop(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else if (start_sq.State == "WQ")
                    {
                        move_bool = Move_WQueen(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else if (start_sq.State == "BQ")
                    {
                        move_bool = Move_BQueen(start_sq, end_sq, start_ind, end_ind, Classic_Test);
                    }

                    else { move_bool = false; }
                }

                else { move_bool = false; }
                    
                if (move_bool == false)
                {
                    Console.WriteLine("Invalid Move, Please try again");
                    continue;
                } 
                
                else//if move was correct and made
                {
                    if (white_turn == true)//and it was whites move
                    {
                        white_turn = false;//switch to blacks move
                    }

                    else { white_turn = true; }//else it was just blacks move, switch to whites move
                }
            }//end of while
        } //end of main
        //turns 


        static bool Move_WPawn(square start_sq, square end_sq, int start_ind, int end_ind)//works except at taking edges
        {
            bool success = true;
          
            if ((start_ind - 8 == end_ind) && end_sq.IsEmpty()) {  }

            else if (((start_ind - 9 == end_ind) || (start_ind - 7 == end_ind)) && end_sq.IsBlack()) {  }//taking pieces

            else { return false; }

            start_sq.State = "__";
            end_sq.State = "WP";
            return success;

        }


        static bool Move_BPawn(square start_sq, square end_sq, int start_ind, int end_ind)
        {
            bool success = true;
          
            if ((start_ind + 8 == end_ind) && end_sq.IsEmpty()) { }

            else if (((start_ind + 9 == end_ind) || (start_ind + 7 == end_ind)) && end_sq.IsWhite()) { }//taking upward right of black piece true

            else { return false; }

            start_sq.State = "__";
            end_sq.State = "BP";
            return success;
        }


        static bool Move_WKnight(square start_sq, square end_sq, int start_ind, int end_ind)
        {

            bool success = true;

            if (((Math.Abs(start_ind - end_ind) == 6) || (Math.Abs(start_ind - end_ind) == 10) || (Math.Abs(start_ind - end_ind) == 15) || (Math.Abs(start_ind - end_ind) == 17)) && (end_sq.IsEmpty() || end_sq.IsBlack())) { }

            else { return false; }

            start_sq.State = "__";
            end_sq.State = "WN";
            return success;
            
        }


        static bool Move_BKnight(square start_sq, square end_sq, int start_ind, int end_ind)
        {

            bool success = true;

            if (((Math.Abs(start_ind - end_ind) == 6) || (Math.Abs(start_ind - end_ind) == 10) || (Math.Abs(start_ind - end_ind) == 15) || (Math.Abs(start_ind - end_ind) == 17)) && (end_sq.IsEmpty() || end_sq.IsWhite())) { }

            else { return false; }

            start_sq.State = "__";
            end_sq.State = "BN";
            return success;

        }


        static bool Move_WRook(square start_sq, square end_sq, int start_ind, int end_ind, board classic)//move White Rook
        {
            bool success = true;

            if(end_sq.IsBlack() || end_sq.IsEmpty())
            {
                if((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind < 0 ))//if moving up
                {
                    for (int i = start_ind - 8; i != end_ind; i -= 8)
                    {
                        if(classic.total_board[i].IsEmpty() == false) { return false; }     
                    }
                }

                if((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind > 0))//if moving down
                {
                    for(int i = start_ind + 8; i != end_ind; i += 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }//if not empty
                        
                    }
                }

                if ((start_sq.Position[1] == end_sq.Position[1]) && (start_ind > end_ind)) // if moving left
                {
                    for(int i = start_ind - 1; i != end_ind; i -= 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }


                if ((start_sq.Position[1] == end_sq.Position[1]) && (end_ind > start_ind)) // if moving right
                {
                    for (int i = start_ind + 1; i != end_ind; i += 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                } 

            }

            else { return false; }

            end_sq.State = "WR";
            start_sq.State = "__";

            return success;
        }


        static bool Move_BRook(square start_sq, square end_sq, int start_ind, int end_ind, board classic)//move Black Rook
        {
            bool success = true;

            if (end_sq.IsWhite() || end_sq.IsEmpty())
            {
                if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind < 0))//if moving up
                {
                    for (int i = start_ind - 8; i != end_ind; i -= 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind > 0))//if moving down
                {
                    for (int i = start_ind + 8; i != end_ind; i += 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                if ((start_sq.Position[1] == end_sq.Position[1]) && (start_ind > end_ind)) // if moving left
                {
                    for (int i = start_ind - 1; i != end_ind; i -= 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }


                if ((start_sq.Position[1] == end_sq.Position[1]) && (end_ind > start_ind)) // if moving right
                {
                    for (int i = start_ind + 1; i != end_ind; i += 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }
            }

            else { return false; }


            end_sq.State = "BR";
            start_sq.State = "__";

            return success;
        }


        static bool Move_WKing(square start_sq, square end_sq, int start_ind, int end_ind)
        {
            bool success = true;

            if (((Math.Abs(end_ind - start_ind) == 1) || (Math.Abs(end_ind - start_ind) == 7) || (Math.Abs(end_ind - start_ind) == 8) || (Math.Abs(end_ind - start_ind) == 9)) && (end_sq.IsEmpty() || end_sq.IsBlack())) { }
            
            else { return false; }

            start_sq.State = "__";
            end_sq.State = "WK";
            return success;
        }


        static bool Move_BKing(square start_sq, square end_sq, int start_ind, int end_ind)
        {
            bool success = true;

            if (((Math.Abs(end_ind - start_ind) == 1) || (Math.Abs(end_ind - start_ind) == 7) || (Math.Abs(end_ind - start_ind) == 8) || (Math.Abs(end_ind - start_ind) == 9)) && (end_sq.IsEmpty() || end_sq.IsWhite())) { }

            else { return false; }

            start_sq.State = "__";
            end_sq.State = "BK";
            return success;
        }


        static bool Move_WBishop(square start_sq, square end_sq, int start_ind, int end_ind, board classic)
        {
            bool success = true;
            List<Char> Let_list = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            int let_diff = Let_list.IndexOf(end_sq.Position[0]) - Let_list.IndexOf(start_sq.Position[0]);//calc difference in number(row)
            
            int num_diff = Convert.ToInt32(end_sq.Position[1]) - Convert.ToInt32(start_sq.Position[1]);//calc difference in letter(col)

            if (end_sq.IsEmpty() || end_sq.IsBlack())
            {

                if (let_diff == num_diff)//if difference between letter(col) and row(num) are equal, end is diag from start (pos slope)
                {//used for up right and down left movement, (pos=pos, neg=neg)

                    if (num_diff > 0)//up right
                    {
                        for (int i = start_ind - 7; i != end_ind; i -= 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff < 0)//down left
                    {
                        for (int i = start_ind + 7; i != end_ind; i += 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if(let_diff == -1 * num_diff)//if diff between letter(col) and row(num) are opposites, end is diag from start (neg slope)
                {
                    if (num_diff < 0)//down right
                    {
                        for (int i = start_ind + 9; i != end_ind; i += 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if(num_diff > 0)//up left
                    {
                        for (int i = start_ind - 9; i != end_ind; i -= 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }
                else { return false; }
            }
            
            else { return false; }

            end_sq.State = "WB";
            start_sq.State = "__";

            return success;
        }


        static bool Move_BBishop(square start_sq, square end_sq, int start_ind, int end_ind, board classic)
        {
            bool success = true;
            List<Char> Let_list = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            int let_diff = Let_list.IndexOf(end_sq.Position[0]) - Let_list.IndexOf(start_sq.Position[0]);//calc difference in number(row)

            int num_diff = Convert.ToInt32(end_sq.Position[1]) - Convert.ToInt32(start_sq.Position[1]);//calc difference in letter(col)

            if (end_sq.IsEmpty() || end_sq.IsWhite())
            {

                if (let_diff == num_diff)//if difference between letter(col) and row(num) are equal, end is diag from start (pos slope)
                {//used for up right and down left movement, (pos=pos, neg=neg)

                    if (num_diff > 0)//up right
                    {
                        for (int i = start_ind - 7; i != end_ind; i -= 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff < 0)//down left
                    {
                        for (int i = start_ind + 7; i != end_ind; i += 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if (let_diff == -1 * num_diff)//if diff between letter(col) and row(num) are opposites, end is diag from start (neg slope)
                {
                    if (num_diff < 0)//down right
                    {
                        for (int i = start_ind + 9; i != end_ind; i += 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff > 0)//up left
                    {
                        for (int i = start_ind - 9; i != end_ind; i -= 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else { return false; }
            }

            else { return false; }

            end_sq.State = "BB";
            start_sq.State = "__";

            return success;
        }


        static bool Move_WQueen(square start_sq, square end_sq, int start_ind, int end_ind, board classic)  
        {
            bool success = true;
            List<Char> Let_list = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            int let_diff = Let_list.IndexOf(end_sq.Position[0]) - Let_list.IndexOf(start_sq.Position[0]);//calc difference in number(row)

            int num_diff = Convert.ToInt32(end_sq.Position[1]) - Convert.ToInt32(start_sq.Position[1]);//calc difference in letter(col)

            if (end_sq.IsEmpty() || end_sq.IsBlack())
            {
                if (let_diff == num_diff)//if difference between letter(col) and row(num) are equal, end is diag from start (pos slope)
                {//used for up right and down left movement, (pos=pos, neg=neg)

                    if (num_diff > 0)//up right
                    {
                        for (int i = start_ind - 7; i != end_ind; i -= 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff < 0)//down left
                    {
                        for (int i = start_ind + 7; i != end_ind; i += 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if (let_diff == -1 * num_diff)//if diff between letter(col) and row(num) are opposites, end is diag from start (neg slope)
                {
                    if (num_diff < 0)//down right
                    {
                        for (int i = start_ind + 9; i != end_ind; i += 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff > 0)//up left
                    {
                        for (int i = start_ind - 9; i != end_ind; i -= 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind < 0))//if moving up
                {
                    for (int i = start_ind - 8; i != end_ind; i -= 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                else if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind > 0))//if moving down
                {
                    for (int i = start_ind + 8; i != end_ind; i += 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }//if not empty

                    }
                }

                else if ((start_sq.Position[1] == end_sq.Position[1]) && (start_ind > end_ind)) // if moving left
                {
                    for (int i = start_ind - 1; i != end_ind; i -= 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }


                else if ((start_sq.Position[1] == end_sq.Position[1]) && (end_ind > start_ind)) // if moving right
                {
                    for (int i = start_ind + 1; i != end_ind; i += 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                else { return false; }
            }

            else { return false; }

            end_sq.State = "WQ";
            start_sq.State = "__";

            return success;
        }


        static bool Move_BQueen(square start_sq, square end_sq, int start_ind, int end_ind, board classic)
        {
            bool success = true;
            List<Char> Let_list = new List<Char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

            int let_diff = Let_list.IndexOf(end_sq.Position[0]) - Let_list.IndexOf(start_sq.Position[0]);//calc difference in number(row)

            int num_diff = Convert.ToInt32(end_sq.Position[1]) - Convert.ToInt32(start_sq.Position[1]);//calc difference in letter(col)

            if (end_sq.IsEmpty() || end_sq.IsWhite())
            {
                if (let_diff == num_diff)//if difference between letter(col) and row(num) are equal, end is diag from start (pos slope)
                {//used for up right and down left movement, (pos=pos, neg=neg)

                    if (num_diff > 0)//up right
                    {
                        for (int i = start_ind - 7; i != end_ind; i -= 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff < 0)//down left
                    {
                        for (int i = start_ind + 7; i != end_ind; i += 7)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if (let_diff == -1 * num_diff)//if diff between letter(col) and row(num) are opposites, end is diag from start (neg slope)
                {
                    if (num_diff < 0)//down right
                    {
                        for (int i = start_ind + 9; i != end_ind; i += 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }


                    if (num_diff > 0)//up left
                    {
                        for (int i = start_ind - 9; i != end_ind; i -= 9)
                        {
                            if (classic.total_board[i].IsEmpty() == false) { return false; }
                        }
                    }
                }

                else if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind < 0))//if moving up
                {
                    for (int i = start_ind - 8; i != end_ind; i -= 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                else if ((start_sq.Position[0] == end_sq.Position[0]) && (end_ind - start_ind > 0))//if moving down
                {
                    for (int i = start_ind + 8; i != end_ind; i += 8)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }//if not empty

                    }
                }

                else if ((start_sq.Position[1] == end_sq.Position[1]) && (start_ind > end_ind)) // if moving left
                {
                    for (int i = start_ind - 1; i != end_ind; i -= 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }


                else if ((start_sq.Position[1] == end_sq.Position[1]) && (end_ind > start_ind)) // if moving right
                {
                    for (int i = start_ind + 1; i != end_ind; i += 1)
                    {
                        if (classic.total_board[i].IsEmpty() == false) { return false; }
                    }
                }

                else { return false; }
            }

            else { return false; }

            end_sq.State = "BQ";
            start_sq.State = "__";

            return success;
        }


        static square Promote(square prom_sq)
        {
            bool corr_inp = false;
            string prom_choice;
            string prom_piece;

            if (prom_sq.GetState() == "WP")
            {                
                Console.WriteLine("White pawn promoted!!, please select which piece to promote to(R,B,N,Q): ");
                prom_choice = Console.ReadLine();
                prom_piece = "WP";
                while (corr_inp == false)
                {

                    if (prom_choice == "R")
                    {
                        prom_piece = "WR";
                        corr_inp = true;
                    }

                    else if (prom_choice == "B")
                    {
                        prom_piece = "WB";
                        corr_inp = true;
                    }

                    else if (prom_choice == "N")
                    {
                        prom_piece = "WN";
                        corr_inp = true;
                    }

                    else if (prom_choice == "Q")
                    {
                        prom_piece = "WQ";
                        corr_inp = true;
                    }

                    else
                    {
                        prom_piece = "WP";
                        Console.WriteLine("Incorrect input, try again!");
                    }

                }
                prom_sq.SetState(prom_piece);
                return prom_sq;
            }

            else if (prom_sq.GetState() == "BP")
            {
                
                Console.WriteLine("Black pawn promoted!!, please select which piece to promote to(R,B,N,Q): ");
                prom_choice = Console.ReadLine();
                prom_piece = "BP";

                while (corr_inp == false)
                {

                    if (prom_choice == "R")
                    {
                        prom_piece = "BR";
                        corr_inp = true;
                    }

                    else if (prom_choice == "B")
                    {
                        prom_piece = "BB";
                        corr_inp = true;
                    }

                    else if (prom_choice == "N")
                    {
                        prom_piece = "BN";
                        corr_inp = true;
                    }

                    else if (prom_choice == "Q")
                    {
                        prom_piece = "BQ";
                        corr_inp = true;
                    }

                    else { Console.WriteLine("Incorrect input, try again!"); }

                }
                prom_sq.SetState(prom_piece);
                return prom_sq;
            }
            return prom_sq;
        }


        static board Classic_board()
        {

            board Classic = new board(8);

            square A1 = new square("A1", "WR");
            square B1 = new square("B1", "WN");
            square C1 = new square("C1", "WB");
            square D1 = new square("D1", "WQ");
            square E1 = new square("E1", "WK");
            square F1 = new square("F1", "WB");
            square G1 = new square("G1", "WN");
            square H1 = new square("H1", "WR");
            square A2 = new square("A2", "WP");
            square B2 = new square("B2", "WP");
            square C2 = new square("C2", "WP");
            square D2 = new square("D2", "WP");
            square E2 = new square("E2", "WP");
            square F2 = new square("F2", "WP");
            square G2 = new square("G2", "WP");
            square H2 = new square("H2", "WP");
            square A3 = new square("A3", "__");
            square B3 = new square("B3", "__");
            square C3 = new square("C3", "__");
            square D3 = new square("D3", "__");
            square E3 = new square("E3", "__");
            square F3 = new square("F3", "__");
            square G3 = new square("G3", "__");
            square H3 = new square("H3", "__");
            square A4 = new square("A4", "__");
            square B4 = new square("B4", "__");
            square C4 = new square("C4", "__");
            square D4 = new square("D4", "__");
            square E4 = new square("E4", "__");
            square F4 = new square("F4", "__");
            square G4 = new square("G4", "__");
            square H4 = new square("H4", "__");
            square A5 = new square("A5", "__");
            square B5 = new square("B5", "__");
            square C5 = new square("C5", "__");
            square D5 = new square("D5", "__");
            square E5 = new square("E5", "__");
            square F5 = new square("F5", "__");
            square G5 = new square("G5", "__");
            square H5 = new square("H5", "__");
            square A6 = new square("A6", "__");
            square B6 = new square("B6", "__");
            square C6 = new square("C6", "__");
            square D6 = new square("D6", "__");
            square E6 = new square("E6", "__");
            square F6 = new square("F6", "__");
            square G6 = new square("G6", "__");
            square H6 = new square("H6", "__");
            square A7 = new square("A7", "BP");
            square B7 = new square("B7", "BP");
            square C7 = new square("C7", "BP");
            square D7 = new square("D7", "BP");
            square E7 = new square("E7", "BP");
            square F7 = new square("F7", "BP");
            square G7 = new square("G7", "BP");
            square H7 = new square("H7", "BP");
            square A8 = new square("A8", "BR");
            square B8 = new square("B8", "BN");
            square C8 = new square("C8", "BB");
            square D8 = new square("D8", "BQ");
            square E8 = new square("E8", "BK");
            square F8 = new square("F8", "BB");
            square G8 = new square("G8", "BN");
            square H8 = new square("H8", "BR");

            Classic.Add_sq(A8);
            Classic.Add_sq(B8);
            Classic.Add_sq(C8);
            Classic.Add_sq(D8);
            Classic.Add_sq(E8);
            Classic.Add_sq(F8);
            Classic.Add_sq(G8);
            Classic.Add_sq(H8);
            Classic.Add_sq(A7);
            Classic.Add_sq(B7);
            Classic.Add_sq(C7);
            Classic.Add_sq(D7);
            Classic.Add_sq(E7);
            Classic.Add_sq(F7);
            Classic.Add_sq(G7);
            Classic.Add_sq(H7);
            Classic.Add_sq(A6);
            Classic.Add_sq(B6);
            Classic.Add_sq(C6);
            Classic.Add_sq(D6);
            Classic.Add_sq(E6);
            Classic.Add_sq(F6);
            Classic.Add_sq(G6);
            Classic.Add_sq(H6);
            Classic.Add_sq(A5);
            Classic.Add_sq(B5);
            Classic.Add_sq(C5);
            Classic.Add_sq(D5);
            Classic.Add_sq(E5);
            Classic.Add_sq(F5);
            Classic.Add_sq(G5);
            Classic.Add_sq(H5);
            Classic.Add_sq(A4);
            Classic.Add_sq(B4);
            Classic.Add_sq(C4);
            Classic.Add_sq(D4);
            Classic.Add_sq(E4);
            Classic.Add_sq(F4);
            Classic.Add_sq(G4);
            Classic.Add_sq(H4);
            Classic.Add_sq(A3);
            Classic.Add_sq(B3);
            Classic.Add_sq(C3);
            Classic.Add_sq(D3);
            Classic.Add_sq(E3);
            Classic.Add_sq(F3);
            Classic.Add_sq(G3);
            Classic.Add_sq(H3);
            Classic.Add_sq(A2);
            Classic.Add_sq(B2);
            Classic.Add_sq(C2);
            Classic.Add_sq(D2);
            Classic.Add_sq(E2);
            Classic.Add_sq(F2);
            Classic.Add_sq(G2);
            Classic.Add_sq(H2);
            Classic.Add_sq(A1);
            Classic.Add_sq(B1);
            Classic.Add_sq(C1);
            Classic.Add_sq(D1);
            Classic.Add_sq(E1);
            Classic.Add_sq(F1);
            Classic.Add_sq(G1);
            Classic.Add_sq(H1);

            Classic.Add_Mpos("A8", 0);
            Classic.Add_Mpos("B8", 1);
            Classic.Add_Mpos("C8", 2);
            Classic.Add_Mpos("D8", 3);
            Classic.Add_Mpos("E8", 4);
            Classic.Add_Mpos("F8", 5);
            Classic.Add_Mpos("G8", 6);
            Classic.Add_Mpos("H8", 7);
            Classic.Add_Mpos("A7", 8);
            Classic.Add_Mpos("B7", 9);
            Classic.Add_Mpos("C7", 10);
            Classic.Add_Mpos("D7", 11);
            Classic.Add_Mpos("E7", 12);
            Classic.Add_Mpos("F7", 13);
            Classic.Add_Mpos("G7", 14);
            Classic.Add_Mpos("H7", 15);
            Classic.Add_Mpos("A6", 16);
            Classic.Add_Mpos("B6", 17);
            Classic.Add_Mpos("C6", 18);
            Classic.Add_Mpos("D6", 19);
            Classic.Add_Mpos("E6", 20);
            Classic.Add_Mpos("F6", 21);
            Classic.Add_Mpos("G6", 22);
            Classic.Add_Mpos("H6", 23);
            Classic.Add_Mpos("A5", 24);
            Classic.Add_Mpos("B5", 25);
            Classic.Add_Mpos("C5", 26);
            Classic.Add_Mpos("D5", 27);
            Classic.Add_Mpos("E5", 28);
            Classic.Add_Mpos("F5", 29);
            Classic.Add_Mpos("G5", 30);
            Classic.Add_Mpos("H5", 31);
            Classic.Add_Mpos("A4", 32);
            Classic.Add_Mpos("B4", 33);
            Classic.Add_Mpos("C4", 34);
            Classic.Add_Mpos("D4", 35);
            Classic.Add_Mpos("E4", 36);
            Classic.Add_Mpos("F4", 37);
            Classic.Add_Mpos("G4", 38);
            Classic.Add_Mpos("H4", 39);
            Classic.Add_Mpos("A3", 40);
            Classic.Add_Mpos("B3", 41);
            Classic.Add_Mpos("C3", 42);
            Classic.Add_Mpos("D3", 43);
            Classic.Add_Mpos("E3", 44);
            Classic.Add_Mpos("F3", 45);
            Classic.Add_Mpos("G3", 46);
            Classic.Add_Mpos("H3", 47);
            Classic.Add_Mpos("A2", 48);
            Classic.Add_Mpos("B2", 49);
            Classic.Add_Mpos("C2", 50);
            Classic.Add_Mpos("D2", 51);
            Classic.Add_Mpos("E2", 52);
            Classic.Add_Mpos("F2", 53);
            Classic.Add_Mpos("G2", 54);
            Classic.Add_Mpos("H2", 55);
            Classic.Add_Mpos("A1", 56);
            Classic.Add_Mpos("B1", 57);
            Classic.Add_Mpos("C1", 58);
            Classic.Add_Mpos("D1", 59);
            Classic.Add_Mpos("E1", 60);
            Classic.Add_Mpos("F1", 61);
            Classic.Add_Mpos("G1", 62);
            Classic.Add_Mpos("H1", 63);

            return Classic;

        }
    }
}