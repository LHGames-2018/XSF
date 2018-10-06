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

        internal string MoveUp()    {return AIHelper.CreateMoveAction(new Point(0,-1));}
        internal string MoveDown()  {return AIHelper.CreateMoveAction(new Point(0, 1));}
        internal string MoveRight() {return AIHelper.CreateMoveAction(new Point(1, 0));}
        internal string MoveLeft()  {return AIHelper.CreateMoveAction(new Point(-1,0));}
        internal int[] FindMineral(Map map){
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
            int[] returnvalue =  {Xdistance,Ydistance};
            return returnvalue;
        }
        internal int getABS(int[] tab){
            return (Math.Abs(tab[0])+Math.Abs(tab[1]));
        }
        internal string Mine(Map map){
            if(map.GetTileAt(PlayerInfo.Position.X + 1, PlayerInfo.Position.Y) == TileContent.Resource){
                return AIHelper.CreateCollectAction( new Point(1, 0));
            }
            if(map.GetTileAt(PlayerInfo.Position.X - 1, PlayerInfo.Position.Y) == TileContent.Resource){
                return AIHelper.CreateCollectAction( new Point(-1, 0));
            }
            if(map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + 1) == TileContent.Resource){
                return AIHelper.CreateCollectAction( new Point(0, 1));
            }
            if(map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y - 1) == TileContent.Resource){
                return AIHelper.CreateCollectAction( new Point(0, -1));
            }
            return AIHelper.CreateCollectAction( new Point(0, 0));
            
        }
        internal int[] FindHouse(Map map){
            int lowest = 20;
            int Xdistance = 0;
            int Ydistance = 0;
            for (int dx = -9; dx <= 9; dx++)
            {
                for (int dy = -9; dy <= 9; dy++)
                    {
                        if (map.GetTileAt(PlayerInfo.Position.X + dx, PlayerInfo.Position.Y  + dy) == TileContent.House){
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
            int[] returnvalue =  {Xdistance,Ydistance};
            return returnvalue;
        }
        internal string ExecuteTurn(Map map, IEnumerable<IPlayer> visiblePlayers)
        {
            string movement = "";
            if (IPlayer.CarriedCapacity-IPlayer.CarriedResources != 0){ //if you can carry more
                int[] ClosestMineral = FindMineral(map);
                int lowestMineralDistance = getABS(ClosestMineral);
                if(lowestMineralDistance != 0){ //will only be 0 if it doesn't find a mineral
                    if (Math.Abs(ClosestMineral[0])>=Math.Abs(ClosestMineral[1])){
                        int a = ClosestMineral[0]/(Math.Abs(ClosestMineral[0]));
                        if (a==1){movement = MoveRight();}
                        if (a==-1){movement = MoveLeft();}
                    }
                    else
                    {
                        int a = ClosestMineral[1]/(Math.Abs(ClosestMineral[1]));
                        if (a== 1){movement = MoveDown();}
                        if (a==-1){movement = MoveUp();  }
                    }
                }
                return movement;
            }
            else { // you are full
                Point MyHouseDirection = IPlayer.HouseLocation - IPlayer.Position;
                int[] HouseDirection = {MyHouseDirection[0], MyHouseDirection[1]};
                return 
            }
            var data = StorageHelper.Read<TestClass>("Test");
            Console.WriteLine(data?.Test);
            return movement;
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