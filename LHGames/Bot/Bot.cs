using System;
using System.Collections.Generic;
using LHGames.Helper;

namespace LHGames.Bot
{
    internal class Bot
    {
        internal IPlayer PlayerInfo { get; set; }

        private int[] _currentDirection = new int[] { 0, 0 };
        private int[] prices = new int[] {10000,15000,25000,50000,100000};
        private int[] upgrades = new int[] {0,0,0,0,0};
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
            _currentDirection[0] = 0;
            _currentDirection[1] = 0;
            int[] HouseDistance = getHome(map);
            if (ABS(HouseDistance[0], HouseDistance[1]) == 0){
                for(int i = 0; i<5; i++){
                    int price = prices[upgrades[i]];
                    if(price < PlayerInfo.TotalResources){
                        return getType(i);
                    }
                }
            }
            int[] ClosestMine = getDistance(TileContent.Resource, map);
            bool full = (PlayerInfo.CarryingCapacity == PlayerInfo.CarriedResources);
            if(!full){
                if (ABS(ClosestMine[0], ClosestMine[1]) == 0){return MoveDirection(HouseDistance[0], HouseDistance[1], map);}
                if (ABS(ClosestMine[0], ClosestMine[1]) == 1){ // a cote de la mine
                    if((map.GetTileAt(PlayerInfo.Position.X + 1, PlayerInfo.Position.Y) == TileContent.Resource)){
                        return AIHelper.CreateCollectAction(new Point(1,0));
                    }
                    if((map.GetTileAt(PlayerInfo.Position.X - 1, PlayerInfo.Position.Y) == TileContent.Resource)){
                        return AIHelper.CreateCollectAction(new Point(-1,0));
                    }
                    if((map.GetTileAt(PlayerInfo.Position.X , PlayerInfo.Position.Y + 1) == TileContent.Resource)){
                        return AIHelper.CreateCollectAction(new Point(0,1));
                    }
                    if((map.GetTileAt(PlayerInfo.Position.X , PlayerInfo.Position.Y - 1) == TileContent.Resource)){
                        return AIHelper.CreateCollectAction(new Point(0,-1));
                    }
                }
                else{
                    return MoveDirection(ClosestMine[0], ClosestMine[1], map);
                }
            }
            else{
                return MoveDirection(HouseDistance[0], HouseDistance[1], map);
            }
            return AIHelper.CreateMoveAction(new Point(0,1));
        }

        internal int ABS(int x, int y){
            return(Math.Abs(x)+Math.Abs(y));
        }

        internal string getType(int u){
            if (u==0){
                return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
            }
            if (u==1){
                return AIHelper.CreateUpgradeAction(UpgradeType.CollectingSpeed);
            }
            if (u==2){
                return AIHelper.CreateUpgradeAction(UpgradeType.Defence);
            }
            if (u==3){
                return AIHelper.CreateUpgradeAction(UpgradeType.AttackPower);
            }
            if (u==4){
                return AIHelper.CreateUpgradeAction(UpgradeType.MaximumHealth);
            }
            return "";
        }

        internal int[] getHome(Map map)
        {
            int[] returnValue = {PlayerInfo.HouseLocation.X - PlayerInfo.Position.X, PlayerInfo.HouseLocation.Y - PlayerInfo.Position.Y};
            return returnValue;
        }

        internal int[] getDistance(TileContent tile, Map map)
        {
            int lowest = 20;
            int Xdistance = 0;
            int Ydistance = 0;
            for (int dx = -9; dx <= 9; dx++)
            {
                for (int dy = -9; dy <= 9; dy++)
                {
                    if (map.GetTileAt(PlayerInfo.Position.X + dx, PlayerInfo.Position.Y + dy) == tile)
                    {
                        int total = Math.Abs(dx) + Math.Abs(dy);
                        if (total <= lowest)
                        {
                            Xdistance = dx;
                            Ydistance = dy;
                            lowest = total;
                        }
                        Console.WriteLine("total: " + total);
                    }
                }
            }
            int[] returnValue = {Xdistance,Ydistance};
            return returnValue;
        }

        internal string MoveDirection(int Xdistance, int Ydistance,Map map){
            if (Math.Abs(Xdistance) >= Math.Abs(Ydistance))
            {
                if (map.GetTileAt(PlayerInfo.Position.X + (Xdistance / Xdistance), PlayerInfo.Position.Y) == TileContent.Resource)
                {
                    if (Math.Abs(Ydistance) != 0)
                    {
                        return AIHelper.CreateMoveAction(new Point(0, (Ydistance / Math.Abs(Ydistance))));
                    }
                    else
                    {
                        return AIHelper.CreateMoveAction(new Point(0, 1));
                    }
                }
                if (map.GetTileAt(PlayerInfo.Position.X + (Xdistance / Math.Abs(Xdistance)), PlayerInfo.Position.Y) == TileContent.Wall)
                {
                    return AIHelper.CreateMeleeAttackAction(new Point((Xdistance / Math.Abs(Xdistance)), 0));
                }
                else
                {
                    return AIHelper.CreateMoveAction(new Point((Xdistance / Math.Abs(Xdistance)), 0));
                }
            }
            else
            {
                if (map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + (Ydistance / Math.Abs(Ydistance))) == TileContent.Resource)
                {
                    if (Math.Abs(Xdistance) != 0)
                    {
                        return AIHelper.CreateMoveAction(new Point((Xdistance / Math.Abs(Xdistance)), 0));
                    }
                    else
                    {
                        return AIHelper.CreateMoveAction(new Point(1, 0));
                    }
                }
                if (map.GetTileAt(PlayerInfo.Position.X, PlayerInfo.Position.Y + (Ydistance / Math.Abs(Ydistance))) == TileContent.Wall)
                {
                    return AIHelper.CreateMeleeAttackAction(new Point(0, (Ydistance / Math.Abs(Ydistance))));
                }
                else
                {
                    return AIHelper.CreateMoveAction(new Point(0, (Ydistance / Math.Abs(Ydistance))));
                }
            }
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