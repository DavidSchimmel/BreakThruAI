using Board;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgentLibrary
{
    public interface IAgent
    {
        public (int, int) GetNextMove(Board.Board board);

        //keep a remaining actions parameter, that counts down by 1 or 2 depending on the action, and if it is zero,
        // change player and reset the parameter BEFORE making the recursive function calls
        // so that alpha-beta and the return values can be inverted in time if needed
        // also probably dont reduce the remaining search depth if it is the same players turn
    }
}
