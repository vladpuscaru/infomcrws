using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textTotalCells;
    public MyGrid grid;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnAlgSelectionChange(Dropdown dropdown) {
        switch(dropdown.value.ToString()) {
            case "Breadth-First-Search":
                grid.activeAlg = AlgorithmType.BFS;
            break;
            case "Depth-First-Search":
                grid.activeAlg = AlgorithmType.DFS;
            break;
            case "A* Search":
                grid.activeAlg = AlgorithmType.ASTAR;
            break;
        }
    }
}
