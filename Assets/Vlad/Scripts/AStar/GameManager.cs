using System;
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
    public TMP_Dropdown dropdownAlgSelect;

    public MyGrid grid;



    // Start is called before the first frame update
    void Start()
    {
        dropdownAlgSelect.onValueChanged.AddListener(delegate {
            OnAlgSelectionChange(dropdownAlgSelect);
        });
    }

    public void OnAlgSelectionChange(TMP_Dropdown dropdown) {
        switch(dropdown.value) {
            case 0:
                grid.activeAlg = AlgorithmType.BFS;
            break;
            case 1:
                grid.activeAlg = AlgorithmType.DFS;
            break;
            case 2:
                grid.activeAlg = AlgorithmType.ASTAR;
            break;
        }
    }
}
