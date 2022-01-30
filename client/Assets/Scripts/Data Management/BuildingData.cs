using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Заготовка для класса, который создаёт объекты Building и хранит их для генерации
*/

namespace OSMDataRenderer
{
    public static class BuildingData
    {
        public static List<Building> BuildingsList;

        public static void addBuilding(List<Line> _lines, uint _levels)
        {
            BuildingsList.Add(new Building(_lines, _levels));
        }
    }
}
