using RoubaMontesAED.Entities;
using RoubaMontesAED.IO;

namespace RoubaMontesAED.Services;

// Classe que vai conter toda lógica do jogo. (Modularizar o código)
public class JogoService
{
    private int _quantidadeJogadores;
    private int _quantidadeCartasPermitidas;
    private MonteCompra MonteCompra;
    private AreaDescarte AreaDescarte;
    private Jogador[] Jogadores;
    private Random random;
    private LogPartida _logger;

    public JogoService(int quantidadeJogadores, int quantidadeCartasPermitidas)
    {
        this._quantidadeJogadores = quantidadeJogadores;
        this._quantidadeCartasPermitidas = quantidadeCartasPermitidas;
        this.MonteCompra = new MonteCompra();
        this.AreaDescarte = new AreaDescarte();
        this.Jogadores = new Jogador[quantidadeJogadores];
        this.random = new Random();
        _logger = new LogPartida();
    }

    public void CriarBaralho()
    {
        // log que foi criado o baralho
        MonteCompra.CriarBaralho(_quantidadeCartasPermitidas);
        _logger.Registrar($"Baralho criado com {_quantidadeCartasPermitidas} cartas.");
    }

    public void EmbaralharCartas()
    {
        // log que foi embaralhado
        MonteCompra.Embaralhar();
        _logger.Registrar("Baralho embaralhado.");
    }

    public Jogador JogadorIniciaPartida()
    {
        // log que indica qual jogador foi sorteado para começar a partida (Depois vai em sentido horário).
        int indiceJogador = random.Next(0, _quantidadeJogadores);
        Jogador jogadorEscolhido = Jogadores[indiceJogador];

        _logger.Registrar($"Jogador sorteado para iniciar: {jogadorEscolhido.Nome}");

        return Jogadores[indiceJogador];
    }


    public Jogador PassarVez(Jogador jogadorAtual)
    {
        int indiceJogadorAtual = Array.IndexOf(Jogadores, jogadorAtual);
        Jogador proximo;

        if (indiceJogadorAtual == Jogadores.Length - 1)
        {
            proximo = Jogadores[0];
        }

        proximo = Jogadores[(indiceJogadorAtual + 1) % Jogadores.Length];
        _logger.Registrar($"Passando a vez: {jogadorAtual.Nome} → {proximo.Nome}");

        return proximo;
    }

    public Carta JogadaPadrao()
    {
        Carta carta = MonteCompra.Comprar();

        if (carta != null)
            _logger.Registrar($"Jogador comprou a carta: {carta}");

        return carta;
    }


    public Jogador MaiorMonte()
    {
        Jogador maior = null;
        int maiorQtd = -1;

        foreach (Jogador jogador in Jogadores)
        {
            int qtd = jogador.TamanhoDoMonteJogador();
            if (qtd > maiorQtd)
            {
                maiorQtd = qtd;
                maior = jogador;
            }
        }

        if (maior != null)
            _logger.Registrar($"Jogador com maior monte no momento: {maior.Nome} ({maiorQtd} cartas)");

        return maior;
    }

    public void RegistrarRoubo(Jogador quemRouba, Jogador quemPerde)
    {
        _logger.Registrar($"{quemRouba.Nome} ROUBOU o monte de {quemPerde.Nome}");
    }

    public void RegistrarCapturaDescarte(Jogador jogador, Carta carta)
    {
        _logger.Registrar($"{jogador.Nome} capturou carta do descarte: {carta}");
    }

    public void RegistrarDescarte(Jogador jogador, Carta carta)
    {
        _logger.Registrar($"{jogador.Nome} descartou: {carta}");
    }

    public void RankingPartida()
    {
        _logger.Registrar("===== RANKING FINAL DA PARTIDA =====");

    }

    public IEnumerable<int> PesquisarHistoricoJogador(string nomeJogador)
    {

    }
}