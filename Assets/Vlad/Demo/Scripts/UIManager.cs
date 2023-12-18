using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI txtNoAgents;
    public Button btnAddAgents;
    public Button btnRemoveAgents;

    public TextMeshProUGUI txtNoAvgPathTime;
    public TextMeshProUGUI txtNoCells;

    InfomCRWS.GameManager game;

    void Start()
    {
        game = GetComponent<InfomCRWS.GameManager>();
        btnAddAgents.onClick.AddListener( delegate { onAddAgents(); } );
        btnRemoveAgents.onClick.AddListener( delegate { OnRemoveAgents(); } );
    }

    void Update() {
        txtNoAgents.text = game.Stats.noAgents.ToString();

        txtNoAvgPathTime.text = Math.Truncate(game.Stats.avgPathTime).ToString() + "ms";
        txtNoCells.text = game.Stats.noCells.ToString();
    }

    public void OnRemoveAgents() {
        game.modifyNoAgents(Convert.ToInt32(game.m_agents.Count - 100));
    }

    public void onAddAgents() {
        game.modifyNoAgents(Convert.ToInt32(game.m_agents.Count + 100));
    }
}
