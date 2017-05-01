﻿using SharpSteroids.Base.Model.Objects;
using System;
using System.Collections.Generic;

namespace SharpSteroids.Base.Model
{
    public static class GameSharedItems
    {
        private static Ship _ship;
        private static IList<Asteroid> _asteroids;
        private static IList<Shoot> _shoots;
        public static int windowWidth, windowHeight;
        public static Random random = new Random();
        public static float asteroidScale = 0.2f;

        public static Ship Ship
        {
            get
            {
                if (_ship == null)
                {
                    _ship = new Ship(new SharpSteroids.Model.Coordinates(300, 300));
                }
                return _ship;
            }
        }

        public static IList<Asteroid> Asteroids
        {
            get
            {
                if (_asteroids == null)
                {
                    _asteroids = new List<Asteroid>();
                }
                return _asteroids;
            }
        }

        public static IList<Shoot> Shoots
        {
            get
            {
                if (_shoots == null)
                {
                    _shoots = new List<Shoot>();
                }
                return _shoots;
            }
        }
    }
}