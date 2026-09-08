using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyController : MonoBehaviour
{
    [Header("Referências da Interface")]
    [SerializeField] private TextMeshProUGUI txtCodigoSala;
    [SerializeField] private TextMeshProUGUI txtContadorJogadores;
    [SerializeField] private Transform containerListaJogadores;
    [SerializeField] private GameObject prefabNomeJogador;
    [SerializeField] private Button btnComecar;
    [SerializeField] private Button btnSair;

    [Header("Configurações das Regras")]
    [SerializeField] private int minJogadores = 4;
    [SerializeField] private int maxJogadores = 6;

    // Altere para false no Inspector se quiser testar a visão de um jogador comum
    public bool souOMestre = true;

    private List<GameObject> itensListaUI = new List<GameObject>();
    private List<string> listaNomesJogadores = new List<string>();

    private void Start()
    {
        GerarCodigoSalaAleatorio();

        // Configura as ações dos botões ao clicar
        btnComecar.onClick.AddListener(OnClickComecar);
        btnSair.onClick.AddListener(OnClickSair);

        // --- APENAS PARA TESTE LOCAL (Simulando entrada de jogadores) ---
        AdicionarJogador("Jogador 1 (Você)");
        AdicionarJogador("Jogador 2");
        AdicionarJogador("Jogador 3");
    }

    private void GerarCodigoSalaAleatorio()
    {
        string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string parte1 = "", parte2 = "";

        for (int i = 0; i < 3; i++) parte1 += caracteres[Random.Range(0, caracteres.Length)];
        for (int i = 0; i < 3; i++) parte2 += caracteres[Random.Range(0, caracteres.Length)];

        txtCodigoSala.text = $"{parte1}-{parte2}";
    }

    public void AdicionarJogador(string nome)
    {
        if (listaNomesJogadores.Count >= maxJogadores) return;

        listaNomesJogadores.Add(nome);
        AtualizarInterfaceLobby();
    }

    public void RemoverJogador(string nome)
    {
        if (listaNomesJogadores.Contains(nome))
        {
            listaNomesJogadores.Remove(nome);
            AtualizarInterfaceLobby();
        }
    }

    private void AtualizarInterfaceLobby()
    {
        // 1. Limpa a lista antiga da tela
        foreach (GameObject item in itensListaUI)
        {
            Destroy(item);
        }
        itensListaUI.Clear();

        // 2. Cria um novo item na tela para cada jogador atual
        foreach (string nome in listaNomesJogadores)
        {
            GameObject novoItem = Instantiate(prefabNomeJogador, containerListaJogadores);
            novoItem.GetComponent<TextMeshProUGUI>().text = nome;
            itensListaUI.Add(novoItem);
        }

        // 3. Atualiza o contador (X/6)
        int totalAtual = listaNomesJogadores.Count;
        txtContadorJogadores.text = $"{totalAtual}/{maxJogadores}";

        // 4. Regra do Botão Começar: Precisa ser Mestre E ter entre 4 e 6 jogadores
        bool podeComecar = souOMestre && (totalAtual >= minJogadores && totalAtual <= maxJogadores);

        btnComecar.interactable = podeComecar;
        btnComecar.gameObject.SetActive(souOMestre); // Esconde botão se não for mestre
    }

    private void OnClickComecar()
    {
        Debug.Log("Partida iniciada com sucesso!");
    }

    private void OnClickSair()
    {
        Debug.Log("Saindo do lobby...");
    }
}