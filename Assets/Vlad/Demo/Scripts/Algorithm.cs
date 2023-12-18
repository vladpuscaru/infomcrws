using System.Collections.Generic;
using UnityEngine;

public interface Algorithm {
    double AvgAllTime {
        get { return GetAverageAllTime(); }
    }

    List<MapNode> FindPath(MapGrid grid, Vector3 startPos, Vector3 targetPos);
    double GetAverageAllTime();
}