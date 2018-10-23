using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace twozerofoureight
{
    class TwoZeroFourEightModel : Model
    {
        protected int boardSize; // default is 4
        protected int[,] board;
        protected Random rand;
        protected int[] range;



        public TwoZeroFourEightModel() : this(4)
        {
            // default board size is 4 
        }

        public TwoZeroFourEightModel(int size)
        {
            boardSize = size;
            board = new int[boardSize, boardSize];
            range = Enumerable.Range(0, boardSize).ToArray();
            foreach (int i in range)
            {
                foreach (int j in range)
                {
                    board[i, j] = 0;
                }
            }
            rand = new Random();
            // initialize board
            HandleChanges();
        }

        public int[,] GetBoard()
        {
            return board;
        }

        private void AddRandomSlot()
        {
            while (true)
            {
                int x = rand.Next(boardSize);
                int y = rand.Next(boardSize);
                if (board[x, y] == 0)
                {
                    board[x, y] = 2;
                    return;
                }
            }
        }

        // Perform shift and merge to the left of the given array.
        protected bool ShiftAndMerge(int[] buffer)
        {
            bool changed = false; // whether the array has changed
            int pos = 0; // next available slot index
            int lastMergedSlot = -1; // last slot that resulted from merging
            foreach (int k in range)
            {
                if (buffer[k] != 0) // nonempty slot
                {
                    // check if we can merge with the previous slot
                    if (pos > 0 && pos - 1 > lastMergedSlot && buffer[pos - 1] == buffer[k])
                    {
                        // merge
                        buffer[pos - 1] *= 2;
                        buffer[k] = 0;
                        lastMergedSlot = pos - 1;
                        changed = true;
                    }
                    else
                    {
                        // shift to the next available slot
                        buffer[pos] = buffer[k];
                        if (pos != k)
                        {
                            buffer[k] = 0;
                            changed = true;
                        }
                        // move the next available slot
                        pos++;
                    }
                }
            }
            return changed;
        }

        protected void HandleChanges(bool changed = true)
        {
            // if the board has changed, add a new number
            // and notify all views
            if (changed)
            {
                AddRandomSlot();
                NotifyAll();
            }
        }

        public void PerformDown()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from bottom to top
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[j, i];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[j, i] = buffer[boardSize - j - 1];
                }
            }
            HandleChanges(changed);
        }

        public void PerformUp()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from top to bottom
                foreach (int j in range)
                {
                    buffer[j] = board[j, i];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[j, i] = buffer[j];
                }
            }
            HandleChanges(changed);
        }

        public void PerformRight()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from right to left
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[i, j];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[i, j] = buffer[boardSize - j - 1];
                }
            }
            HandleChanges(changed);
        }

        public void PerformLeft()
        {
            bool changed = false; // whether the board has changed
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                // extract the current column from left to right
                foreach (int j in range)
                {
                    buffer[j] = board[i, j];
                }
                // process the extracted array
                // also track changes
                changed = ShiftAndMerge(buffer) || changed;
                // copy back
                foreach (int j in range)
                {
                    board[i, j] = buffer[j];
                }
            }
            HandleChanges(changed);
        }


        public int GetScore()
        {
            int score = 0;
            foreach(int s in GetBoard())
            {
                score += s;
            }
            return score;
        }

        public bool GameWin()
        {
            bool isgamewin = false;
            foreach(int i in GetBoard())
            {
                if (i == 2048)
                {
                    isgamewin = true;
                }

            }
            return isgamewin;
        }

        public bool GameFull()
        {
            bool isGameOver = false;
            int count = 0;
            int tmp_count = 0;
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                foreach (int j in range)
                {
                    buffer[j] = board[i, j];
                }
                if (!ShiftAndMerge(buffer))
                {
                    tmp_count++;
                }
                if (tmp_count == 4)
                {
                    count++;
                }
            }
            tmp_count = 0;

            //*****
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[i, j];
                }
                if (!ShiftAndMerge(buffer))
                {
                    tmp_count++;
                }
                if (tmp_count == 4)
                {
                    count++;
                }

            }
            tmp_count = 0;
            //up
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                foreach (int j in range)
                {
                    buffer[j] = board[i, j];
                }
                if (!ShiftAndMerge(buffer))
                {
                    tmp_count++;
                }
                if (tmp_count == 4)
                {
                    count++;
                }
            }
            //down
            tmp_count = 0;
            foreach (int i in range)
            {
                int[] buffer = new int[boardSize];
                foreach (int j in range)
                {
                    buffer[boardSize - j - 1] = board[i, j];
                }
                if (!ShiftAndMerge(buffer))
                {
                    tmp_count++;
                }
                if (tmp_count == 4)
                {
                    count++;
                }
            }
            if (count == 4)
            {
                isGameOver = true;

            }
            return isGameOver;
        }
        /*
        public bool isgameover() 
           {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++){
                    if(i==3 ||j == 3)
                    {
                        if(i==3 && j != 3)
                        {
                            if (board[i, j] == board[i, j + 1])
                            {
                                return false;
                            }
                         
                        }
                        if (j == 3 && i != 3)
                        {
                            if(board[i,j]==board[i+1,j])
                            {
                                return false;                  
                            }
                        }
                    }   else if (board[i, j] == board[i, j + 1] || board[i, j] == board[i + 1, j])
                    {
                        return false;
                    }
                    
                }
            }
            MessageBox.Show("GAMEOVER GG");
            return true;
           }
           */
              


       /* public bool Gameoverlose()
        {
            bool isover = false;
            foreach(int i in range)
            {
                int[] buf = new int[boardSize];
                foreach(int j in range)
                {
                    buf[i] = board[i,j];
                }

            }
        }*/

    }
}
