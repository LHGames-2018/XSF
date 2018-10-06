using System;
using System.Collections.Generic;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }
        private int[] _currentDirection = new int[] {0,0};

        internal Bot() { }

        /// <summary>
        /// Gets called before ExecuteTurn. This is where you get your bot's state.
        /// </summary>
        /// <param name="playerInfo">Your bot's current state.</param>
        internal void BeforeTurn(IPlayer playerInfo)
        {
            PlayerInfo = playerInfo;
        }

        /// <summary>
        /// Implement your bot here.
        /// </summary>
        /// <param name="map">The gamemap.</param>
        /// <param name="visiblePlayers">Players that are visible to your bot.</param>
        /// <returns>The action you wish to execute.</returns>
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            int lowest = 20;
            int Xdistance = 0;
            int Ydistance = 0;
            for (int dx = -9; dx <= 9; dx++)
            {
                for (int dy = -9; dy <= 9; dy++)
                    {
                        if (map.GetTileAt(PlayerInfo.Position.X + dx, PlayerInfo.Position.Y  + dy) == TileContent.Resource){
                            int total = Math.Abs(dx)+ Math.Abs(dy);
                            if (total <= lowest){
                                Xdistance = dx;
                                Ydistance = dy;
                                lowest = total;
                            }
                            Console.WriteLine("total: "+total);
                        }
                    }
            }

            if(lowest != 0){
                if (Math.Abs(Xdistance)>=Math.Abs(Ydistance)){
                    _currentDirection[0] = Xdistance/(Math.Abs(Xdistance));
                    _currentDirection[1] = 0;
                    Console.WriteLine("x");
                }
                else
                {
                    _currentDirection[1] = Ydistance/(Math.Abs(Ydistance));
                    _currentDirection[0] = 0;
                    Console.WriteLine("y");
                }
            }

            return AIHelper.CreateMoveAction(new Point(_currentDirection[0], _currentDirection[1]));
            
            // TODO: Implement your AI here.
            /*
            if (map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y  + _currentDirection) == TileContent.Wall)
            {
                _currentDirection *= -1;
            }
            */

            var data = StorageHelper.Read<TestClass>("Test");
            //Console.WriteLine(data?.Test);
            return AIHelper.CreateMoveAction(new Point(0, 0));
        }

        /// <summary>
        /// Gets called after ExecuteTurn.
        /// </summary>
        internal void AfterTurn()
        {
        }
    }
}

class TestClass
{
    public string Test { get; set; }
}