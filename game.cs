using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {	
		static string[ ] letters = { "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
                
		static int rows, columns; // size of the game board
		static bool [ , ] availableSpace; // false if the tile is removed
        static int platformRowA = -1; // location of platform A [row], start off the board 
        static int platformColA = -1; // location of platform A [col]
        static int platformRowB = -1; // location of platform B [row]
        static int platformColB = -1; // location of platform B [col]
        static int pawnRowA = -1; // location of pawn A [row], start off the board 
        static int pawnColA = -1; // location of pawn A [col]
        static int pawnRowB = -1; // location of pawn B [row]
        static int pawnColB = -1; // location of pawn B {col}
        static string nameA, nameB; // names of players A and B
        static int turnCounter = 0; // counts what turn the game is on. Even numbers indicate Player A's turn, odd indicates Player B's turn
        
        const string h  = "\u2500"; // horizontal line
		const string v  = "\u2502"; // vertical line
		const string tl = "\u250c"; // top left corner
		const string tr = "\u2510"; // top right corner
		const string bl = "\u2514"; // bottom left corner
		const string br = "\u2518"; // bottom right corner
		const string vr = "\u251c"; // vertical join from right
		const string vl = "\u2524"; // vertical join from left
		const string hb = "\u252c"; // horizontal join from below
		const string ha = "\u2534"; // horizontal join from above
		const string hv = "\u253c"; // horizontal vertical cross
		const string sp = " ";      // space
		const string pa = "A";      // pawn A
		const string pb = "B";      // pawn B
		const string bb = "\u25a0"; // block
		const string fb = "\u2588"; // left half block
		const string lh = "\u258c"; // left half block
		const string rh = "\u2590"; // right half block
        
        // Main method. Loop runs infinitely.
        static void Main( )
        {
			InitializeGameBoard();
			while(true)
			{
				DrawGameBoard();
				PerformPlayerMove();
			}
		}
		
		// Set up the game board 
		static void InitializeGameBoard()
        {
        
			WriteLine("Isolation! Don't get stranded!");
			WriteLine();
			
			// Collect names of Player A & B - Default: "Player A" & "Player B"
			Write("Enter the name of player A: ");
			nameA = ReadLine();
			if(nameA.Length == 0) nameA = "Player A";
			
			Write("Enter the name of player B: ");
			nameB = ReadLine();
			if(nameB.Length == 0) nameB = "Player B";
			
			// Collect desired game board rows and columns from the user - Default: rows = 6, columns = 8
            Write("Enter the number rows for the board: ");
            string response1 = ReadLine();
            if(response1.Length == 0) rows = 6;
            else rows = int.Parse(response1);
            while (rows < 4 || rows > 26)
            {
				Write("Please enter a number of rows between 4 and 26: ");
				rows = int.Parse(ReadLine());
            }
            
            Write("Enter the number of columns for the board: ");
            string response2 = ReadLine();
            if(response2.Length == 0) columns = 8;
            else columns = int.Parse(response2);
            while (columns < 4 || columns > 26)
            {
				Write("Please enter a number of columns between 4 and 26: ");
				columns = int.Parse(ReadLine());
            }
            
            availableSpace = new bool[rows, columns];
            
            // Draws the game board based on the inputted values
            WriteLine();
            DrawGameBoard();
            
            
			Write("{0}, enter the row and column of your platform: ", nameA);
			string responseA = ReadLine();
			if(responseA.Length == 0) 
			{
				platformRowA = pawnRowA = 2; // Default starting position for Pawn A
				platformColA = pawnColA = 0;
			}
			else if (responseA.Length != 2)
			{
				WriteLine("The platform location must be two coordinate letters.");
			}
			else
			{
				platformRowA = pawnRowA = Array.IndexOf(letters, responseA.Substring(0, 1)); // Starting position [row] of Pawn A, only the first character of the string
				platformColA = pawnColA = Array.IndexOf(letters, responseA.Substring(1, 1)); // Starting position [col] of Pawn A, only the second character of the string
			}
          
			Write("{0}, enter the row and column of your platform: ", nameB);
			string responseB = ReadLine();
			if(responseB.Length == 0) 
			{
				platformRowB = pawnRowB = 3; // Default starting position for Pawn B
				platformColB = pawnColB = 7;
			}
			else if (responseB.Length != 2)
			{
				WriteLine("The platform location must be two coordinate letters.");
			}
			else
			{
				platformRowB = pawnRowB = Array.IndexOf(letters, responseB.Substring(0, 1)); // Starting position [row] of Pawn B, only the first character of the string
				platformColB = pawnColB = Array.IndexOf(letters, responseB.Substring(1, 1)); // Starting position [col] of Pawn B, only the second character of the string
			}
	 
        }
		
		static void DrawGameBoard()
        {
			Console.Clear(); // The console gets cleared every time a new move is made to display an updated game board
			Write("Isolation! Don't get stranded!		Turn: {0}", turnCounter);
			WriteLine();
			WriteLine();
			
			// The following lines are the physical set up of the game board
			Write("  ");
			for(int c = 0; c < availableSpace.GetLength(1); c++)
			{
				Write("   {0}", letters[c]);
			}
			WriteLine();
			
			Write("   ");
			for(int c = 0; c < availableSpace.GetLength(1); c++)
			{
				if(c == 0) Write(tl);
				Write("{0}{0}{0}", h);
				if(c == availableSpace.GetLength(1) - 1) Write("{0}", tr);
				else Write("{0}", hb);
			}
			WriteLine();
			
			// These loops mark each tile using the MarkTile() method
			for(int r = 0; r < availableSpace.GetLength(0); r++)
			{
				Write(" {0} ", letters[r]);
				
				for(int c = 0; c < availableSpace.GetLength(1); c++)
				{
					if(c == 0) Write(v);
					MarkTile(r, c);
					Write(v);
				}
				WriteLine();
				
				if(r != availableSpace.GetLength(0) -1)
				{
					Write("   ");
					for(int c = 0; c < availableSpace.GetLength(1); c++)
					{
						if(c == 0) Write(vr);
						Write("{0}{0}{0}", h);
						if(c == availableSpace.GetLength(1) - 1) Write("{0}", vl);
						else Write("{0}", hv);
					}
					WriteLine();
				}
				else
				{
					Write("   ");
					for(int c = 0; c < availableSpace.GetLength(1); c++)
					{
						if(c == 0) Write(bl);
						Write("{0}{0}{0}", h);
						if(c == availableSpace.GetLength(1) - 1) Write("{0}", br);	
						else Write("{0}", ha);
					}
					WriteLine();
				}
			}
			
			WriteLine();
        }
		
		// Method to perform the player's move. 
		static void PerformPlayerMove()
		{
			bool isValid = false; // A move is only valid under very specific conditions, i.e. the move must be 4 characters long, be on the board, be on an available space, etc. This loop goes through all the different logical operators and determines which moves are valid
			while(isValid == false) 
			{
				string playerName;
				if(turnCounter % 2 == 0)
				{
					playerName = nameA;
				}
				else
				{
					playerName = nameB;
				}

				Write("{0}, enter your 4 letter move [abcd]: ", playerName);
				string responseMove = ReadLine();
				
				if(responseMove.Length != 4)
				{
					WriteLine("Please enter four valid coordinate letters.");
				}
				else
				{
					int nextRow = Array.IndexOf(letters, responseMove.Substring(0,1));
					int nextCol = Array.IndexOf(letters, responseMove.Substring(1,1));
					int removeRow = Array.IndexOf(letters, responseMove.Substring(2,1));
					int removeCol = Array.IndexOf (letters, responseMove.Substring(3,1));
					
					if (nextRow < 0 || nextRow >= rows // All possible illegal pawn moves
					 || nextCol < 0 || nextCol >= columns
					 || nextRow == pawnRowA && nextCol == pawnColA
					 || nextRow == pawnRowB && nextCol == pawnColB
					 || availableSpace[nextRow, nextCol]
					 ||   turnCounter % 2 == 0 && (int) Math.Abs(pawnRowA - nextRow) > 1
					 ||   turnCounter % 2 == 0 && (int) Math.Abs(pawnColA - nextCol) > 1
					 ||   turnCounter % 2 != 0 && (int) Math.Abs(pawnRowB - nextRow) > 1
					 ||   turnCounter % 2 != 0 && (int) Math.Abs(pawnColB - nextCol) > 1)
					{
						WriteLine("Your pawn can't move there.");
					}
					else if(removeRow < 0 || removeRow >= rows // All possible illegal spaces to remove a tile
						|| removeCol < 0 || removeCol >= columns
						|| turnCounter % 2 != 0 && removeRow == pawnRowA && removeCol == pawnColA
						|| turnCounter % 2 == 0 && removeRow == pawnRowB && removeCol == pawnColB
						|| removeRow == nextRow && removeCol == nextCol
						|| removeRow == platformRowA && removeCol == platformColA
						|| removeRow == platformRowB && removeCol == platformColB
						|| availableSpace[removeRow, removeCol])
					{
						WriteLine("You can't remove that tile.");
					}
					else // If both the pawn move and the remove tile space are valid, then the move is valid
					{
						isValid = true;
						if(turnCounter % 2 == 0) 
						{
							pawnRowA = nextRow; 
							pawnColA = nextCol;
						}
						else 
						{
							pawnRowB = nextRow;
							pawnColB = nextCol;
						}
		
						availableSpace[removeRow, removeCol] = true;
						turnCounter++; 
					}
				}
			}
		}	
		
		// Method to mark each tile with either a removed space, an empty space, an occupied space (A/B) or a starting platform (A/B)
        static void MarkTile(int row, int col)
        {   
			if(row == pawnRowA && col == pawnColA)	
			{
				Write(sp + pa + sp);
			}
			else if(row == pawnRowB && col == pawnColB)
			{
				Write(sp + pb + sp);
			}
			else if(row == platformRowA && col == platformColA)
			{
				Write(sp + bb + sp);
			}
			else if(row == platformRowB && col == platformColB)
			{
				Write(sp + bb + sp);
			}
			else if(availableSpace[row, col] == false)
			{
				Write(rh + fb + lh);
			}
			else
			{
				Write(sp + sp + sp);
			}
        }
    }
}
