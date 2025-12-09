using RoubaMontesAED.Entities;
using RoubaMontesAED.Services;
using System;

class Program
{
    private const string SEP = "==================================";

    static void Main(string[] args)
    {
        Console.WriteLine("ROUBA MONTES — INICIALIZAÇÃO");

        while (true)
        {
            if (!MenuInicial())
                break;

            ExecutarNovaPartida();
        }

        Console.WriteLine("Jogo encerrado.");
    }

    static bool MenuInicial()
    {
        Console.WriteLine(SEP);
        Console.WriteLine("Deseja iniciar nova partida?");
        Console.WriteLine("1) Sim");
        Console.WriteLine("2) Não (sair)");
        Console.Write("Opção: ");

        string? opcao = Console.ReadLine();

        return opcao == "1";
    }

    static void ExecutarNovaPartida()
    {
        Console.WriteLine(SEP);
        Console.WriteLine("CONFIGURAÇÃO DE NOVA PARTIDA");

        int qtdJogadores = LerInt("Quantidade de jogadores (mínimo 2): ", 2);
        int qtdBaralhos = LerInt("Quantidade de baralhos (1 = 52 cartas): ", 1);

        Jogador[] jogadores = LerJogadores(qtdJogadores);

        int qtdCartas = qtdBaralhos * 52;
        JogoService jogo = new JogoService(qtdJogadores, qtdCartas);

        jogo.DefinirJogadores(jogadores);
        jogo.CriarBaralho();
        jogo.EmbaralharCartas();
        jogo.ExecutarPartida();

        ConsultarHistorico(jogo);

        SalvarLog(jogo);
    }

    static int LerInt(string mensagem, int minimo)
    {
        int valor;

        Console.Write(mensagem);
        while (!int.TryParse(Console.ReadLine(), out valor) || valor < minimo)
        {
            Console.Write($"Valor inválido. {mensagem}");
        }

        return valor;
    }

    static Jogador[] LerJogadores(int quantidade)
    {
        Jogador[] jogadores = new Jogador[quantidade];

        for (int i = 0; i < quantidade; i++)
        {
            Console.Write($"Nome do jogador {i + 1}: ");
            string? nome = Console.ReadLine();

            jogadores[i] = new Jogador(nome!);
        }

        return jogadores;
    }

    static void ConsultarHistorico(JogoService jogo)
    {
        Console.WriteLine();
        Console.WriteLine("Deseja consultar o histórico de um jogador?");
        Console.WriteLine("1) Sim");
        Console.WriteLine("2) Não");
        Console.Write("Opção: ");

        string? opcao = Console.ReadLine();

        if (opcao != "1")
            return;

        Console.Write("Digite o nome do jogador: ");
        string? nome = Console.ReadLine();

        var historico = jogo.PesquisarHistoricoJogador(nome!);

        Console.WriteLine($"Histórico das últimas posições de {nome}:");

        int count = 0;
        foreach (int pos in historico)
        {
            Console.WriteLine($"{pos}º lugar");
            count++;
        }

        if (count == 0)
            Console.WriteLine("(sem histórico registrado)");
    }

    static void SalvarLog(JogoService jogo)
    {
        Console.Write("Digite um nome para salvar o log (ex: partida1.txt). Deixe vazio para nome automático: ");
        string? nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
            jogo.SalvarLogAutomatico();

        else
            jogo.SalvarLog(nome);


        Console.WriteLine("Partida finalizada. Log salvo com sucesso!");
    }
}
