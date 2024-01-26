using Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__RPG.src
{
    public class Map
    {
        public Event[,] WorldMap {  get; private set; }
        private const int WORLDSIZE = 16;
        public int playerX = 0;
        public int playerY = 0;
        public Map() 
        {
            WorldMap = new Event[WORLDSIZE, WORLDSIZE];

        }
    }
}
