using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace SV_Client.Classes.ProgramLogic.PlayField
{
    /// <summary>
    /// enum for the attackresponse, one of the 4 possibilities
    /// </summary>
    public enum AttackResponse
    {
        Fail,//if the attack wasn't a success
        Hit,//if the attack hit something
        Destroyed,//if the attack destroyed a ship
        Won//if you won the game

    }

    public abstract class AbstractPlayField
    {
        protected int SizeX;

        protected int SizeY;

        protected bool[,] FieldArray;

        private int _airCraftSize;
        private IList<Ship> _airCraft;

        private int _battleShipSize;
        private IList<Ship> _battleShip;

        private int _cruiserSize;
        private IList<Ship> _cruiser;

        private int _patrolBoatSize;
        private IList<Ship> _patrolBoat;

        protected AbstractPlayField()
        {
        }

        /// <summary>
        /// to check if a attack hit a ship
        /// </summary>
        /// <param name="attack">the attack made by the enemy</param>
        /// <returns>a AttackResponse(can be hit, fail, destroyed or win)</returns>
        protected virtual AttackResponse IsHit(Attack attack)
        {
            var x = attack.Point.X;
            var y = attack.Point.Y;

            if (x > SizeX || y>SizeY)
            {
                return AttackResponse.Fail;
            }

            var allHitAir = false;
            foreach (var airCraft in _airCraft)
            {
                if (airCraft.PointsOfShip.Contains(attack.Point))
                {
                    var point = airCraft.PointsOfShip.IndexOf(attack.Point);
                    airCraft.PointsOfShip[point].IsHit = true;
                    var allHit = true;
                    foreach (var cPoint in airCraft.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                    {
                        allHit = false;
                    }
                    if (allHit == false)
                    {
                        return AttackResponse.Hit;
                    }
                    else
                    {
                        allHitAir = true;
                    }
                }
            }
            var allHitBattle = false;
            foreach (var battleShip in _battleShip)
            {
                if (battleShip.PointsOfShip.Contains(attack.Point))
                {
                    var point = battleShip.PointsOfShip.IndexOf(attack.Point);
                    battleShip.PointsOfShip[point].IsHit = true;
                    var allHit = true;
                    foreach (var cPoint in battleShip.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                    {
                        allHit = false;
                    }
                    if (allHit == false)
                    {
                        return AttackResponse.Hit;
                    }
                    else
                    {
                        allHitBattle = true;
                    }
                }
            }
            var allHitCruiser=false;
            foreach (var cruiser in _cruiser)
            {
                if (cruiser.PointsOfShip.Contains(attack.Point))
                {
                    var point = cruiser.PointsOfShip.IndexOf(attack.Point);
                    cruiser.PointsOfShip[point].IsHit = true;
                    var allHit = true;
                    foreach (var cPoint in cruiser.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                    {
                        allHit = false;
                    }
                    if (allHit == false)
                    {
                        return AttackResponse.Hit;
                    }
                    else
                    {
                        allHitCruiser = true;
                    }
                }
            }
            var allHitPatrol = false;
            foreach (var patrolBoat in _patrolBoat)
            {
                if (patrolBoat.PointsOfShip.Contains(attack.Point))
                {
                    var point = patrolBoat.PointsOfShip.IndexOf(attack.Point);
                    patrolBoat.PointsOfShip[point].IsHit = true;
                    var allHit = true;
                    foreach (var cPoint in patrolBoat.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                    {
                        allHit = false;
                    }
                    if (allHit == false)
                    {
                        return AttackResponse.Hit;
                    }
                    else
                    {
                        allHitPatrol = true;
                    }
                }
            }

            if (IsEverythingDestroyed())
            {
                return AttackResponse.Won;
            }

            if (allHitAir || allHitBattle || allHitCruiser || allHitPatrol)
            {
                return AttackResponse.Destroyed;
            }
            

            return AttackResponse.Fail;
        }

        /// <summary>
        /// checks if all ships are destroyed
        /// </summary>
        /// <returns>a true if every ship is destroyed in the gamefield</returns>
        private bool IsEverythingDestroyed()
        {
            var allHitPatrol = false;
            foreach (var patrolBoat in _patrolBoat)
            {
                var allHit = true;
                foreach (var cPoint in patrolBoat.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                {
                    allHit = false;
                }
                if (allHit == false)
                {
                    return false;
                }
                else
                {
                    allHitPatrol = true;
                }

            }
            var allHitCruiser = false;
            foreach (var cruiser in _cruiser)
            {
                var allHit = true;
                foreach (var cPoint in cruiser.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                {
                    allHit = false;
                }
                if (allHit == false)
                {
                    return false;
                }
                else
                {
                    allHitCruiser = true;
                }

            }
            var allHitAir = false;
            foreach (var airCraft in _airCraft)
            {
                var allHit = true;
                foreach (var cPoint in airCraft.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                {
                    allHit = false;
                }
                if (allHit == false)
                {
                    return false;
                }
                else
                {
                    allHitAir = true;
                }

            } 
            var allHitBattle = false;
            foreach (var battleShip in _battleShip)
            {
                var allHit = true;
                foreach (var cPoint in battleShip.PointsOfShip.Where(cPoint => !cPoint.IsHit))
                {
                    allHit = false;
                }
                if (allHit == false)
                {
                    return false;
                }
                else
                {
                    allHitBattle = true;
                }

            }
            return allHitAir && allHitBattle && allHitPatrol && allHitCruiser;
        }


        /// <summary>
        /// constructor to initialize the fields and so on
        /// </summary>
        /// <param name="sizeX">the horizontal size</param>
        /// <param name="sizeY">the vertical size</param>
        protected AbstractPlayField(int sizeX,int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            FieldArray=new bool[sizeX,sizeY];

            _airCraftSize = 1;
            _battleShipSize = 2;
            _cruiserSize = 3;
            _patrolBoatSize = 4;
            

            InitShips();
        }

        private void InitShips()
        {
            _airCraft=new Ship[_airCraftSize];

            _battleShip=new Ship[_battleShipSize];

            _cruiser=new Ship[_cruiserSize];
            
            _patrolBoat=new Ship[_patrolBoatSize];

            for (var x = _airCraftSize - 1; x >= 0; x--)
            {
                _airCraft[x] = new Ship(5);
            }

            for (var x = _battleShipSize - 1; x >= 0; x--)
            {
                _battleShip[x] = new Ship(4);
            }

            for (var x = _cruiserSize - 1; x >= 0; x--)
            {
                _cruiser[x] = new Ship(3);
            }

            for (var x = _patrolBoatSize - 1; x >= 0; x--)
            {
                _patrolBoat[x] = new Ship(2);
            }
        }
    }
}
