using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingClass;

// Заготовка для класса, который создаёт объекты Building и хранит их для генерации

namespace DataManagement
{
    public class BuildingData
    {
        public List<Building> BuildingsList;

        public void addBuilding(List<Line> _lines, uint _levels)
        {
            BuildingsList.Add(new Building(_lines, _levels));
        }
    }
}
