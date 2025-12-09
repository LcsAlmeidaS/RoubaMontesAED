using RoubaMontesAED.Entities;
using RoubaMontesAED.IO;
using System;
using System.Runtime.CompilerServices;

namespace RoubaMontesAED.Services;

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

    public void DefinirJogadores(Jogador[] jogadores)
    {
        Jogadores = jogadores;

        string nomes = "";
        for (int i = 0; i < jogadores.Length; i++)
        {
            nomes += jogadores[i].Nome;

            if (i < jogadores.Length - 1)
            {
                nomes += ", ";
            }
        }

        _logger.Registrar($"Jogadores definidos: {nomes}");
    }

    public void CriarBaralho()
    {
        MonteCompra.CriarBaralho(_quantidadeCartasPermitidas);
        _logger.Registrar($"Baralho criado com {_quantidadeCartasPermitidas} cartas.");
    }

    public void EmbaralharCartas()
    {
        MonteCompra.Embaralhar();
        _logger.Registrar("Baralho embaralhado.");
    }

    public Jogador JogadorIniciaPartida()
    {
        int indiceJogador = random.Next(0, _quantidadeJogadores);
        Jogador jogadorEscolhido = Jogadores[indiceJogador];

        _logger.Registrar($"Jogador sorteado para iniciar: {jogadorEscolhido.Nome}");

        return jogadorEscolhido;
    }

    public void ExecutarPartida()
    {
        Jogador jogadorAtual = JogadorIniciaPartida();

        while (!MonteCompra.EstaVazio())
        {
            ExecutarTurno(jogadorAtual);
            jogadorAtual = PassarVez(jogadorAtual);
        }

        RankingFinal();

        SalvarLogAutomatico();
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

    private void ExecutarTurno(Jogador jogadorAtual)
    {
        Carta cartaDaVez = JogadaPadrao();
        if (cartaDaVez == null)
            return;

        _logger.RegistrarToposDosMontes(Jogadores);
        _logger.RegistrarCartasDescarte(AreaDescarte);

        bool continuarJogada = true;

        while (continuarJogada)
        {
            continuarJogada = false;

            Jogador jogadorRoubado = VerificarRoubo(jogadorAtual, cartaDaVez);
            if (jogadorRoubado != null)
            {
                ExecutarRoubo(jogadorAtual, jogadorRoubado, cartaDaVez);

                _logger.RegistrarToposDosMontes(Jogadores);
                _logger.RegistrarCartasDescarte(AreaDescarte);

                cartaDaVez = JogadaPadrao();
                if (cartaDaVez == null)
                    return;

                continuarJogada = true;
                continue;
            }

            Carta cartaDescarte = AreaDescarte.ProcurarPorValor(cartaDaVez.Valor);
            if (cartaDescarte != null)
            {
                ExecutarCapturaDescarte(jogadorAtual, cartaDaVez, cartaDescarte);

                _logger.RegistrarToposDosMontes(Jogadores);
                _logger.RegistrarCartasDescarte(AreaDescarte);

                cartaDaVez = JogadaPadrao();
                if (cartaDaVez == null)
                    return;

                continuarJogada = true;
                continue;
            }

            if (PodeEmpilhar(jogadorAtual, cartaDaVez))
            {
                ExecutarEmpilhamento(jogadorAtual, cartaDaVez);

                _logger.RegistrarToposDosMontes(Jogadores);
                _logger.RegistrarCartasDescarte(AreaDescarte);

                cartaDaVez = JogadaPadrao();
                if (cartaDaVez == null)
                    return;

                continuarJogada = true;
                continue;
            }

            ExecutarDescarte(jogadorAtual, cartaDaVez);

            _logger.RegistrarToposDosMontes(Jogadores);
            _logger.RegistrarCartasDescarte(AreaDescarte);

            return;
        }
    }

    public Carta JogadaPadrao()
    {
        Carta carta = MonteCompra.Comprar();

        if (carta != null)
            _logger.Registrar($"Jogador comprou a carta: {carta}");

        return carta;
    }

    private Jogador VerificarRoubo(Jogador jogadorAtual, Carta cartaDaVez)
    {
        List<Jogador> candidatos = new List<Jogador>();
        int maiorMonte = -1;

        foreach (Jogador j in Jogadores)
        {
            if (j == null || j == jogadorAtual)
                continue;

            Carta topo = j.TopoDoMonteJogador();
            if (topo == null || topo.Valor != cartaDaVez.Valor)
                continue;

            int tamanho = j.TamanhoDoMonteJogador();

            if (tamanho > maiorMonte)
            {
                maiorMonte = tamanho;
                candidatos.Clear();
                candidatos.Add(j);
            }
            else if (tamanho == maiorMonte)
            {
                candidatos.Add(j);
            }
        }

        if (candidatos.Count == 0)
            return null;

        int escolha = random.Next(0, candidatos.Count);
        return candidatos[escolha];
    }

    private void ExecutarRoubo(Jogador quemRouba, Jogador quemPerde, Carta cartaDaVez)
    {
        List<Carta> cartasRoubadas = quemPerde.RetirarMonte();

        quemRouba.ReceberCartasRoubadas(cartasRoubadas);
        quemRouba.ReceberCarta(cartaDaVez);

        RegistrarRoubo(quemRouba, quemPerde);
    }

    private void ExecutarCapturaDescarte(Jogador jogador, Carta cartaDaVez, Carta descarte)
    {
        AreaDescarte.Remover(descarte);

        jogador.ReceberCarta(descarte);
        jogador.ReceberCarta(cartaDaVez);

        RegistrarCapturaDescarte(jogador, descarte);
    }

    private bool PodeEmpilhar(Jogador jogador, Carta cartaDaVez)
    {
        Carta topo = jogador.TopoDoMonteJogador();

        if (topo == null)
            return false;

        return topo.Valor == cartaDaVez.Valor;
    }

    private void ExecutarEmpilhamento(Jogador jogador, Carta carta)
    {
        jogador.ReceberCarta(carta);
        _logger.Registrar($"{jogador.Nome} empilhou a carta no próprio monte.");
    }

    private void ExecutarDescarte(Jogador jogador, Carta carta)
    {
        AreaDescarte.Adicionar(carta);
        RegistrarDescarte(jogador, carta);
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

    private string RankingFinal()
    {
        int esq = 0, dir = Jogadores.Length;
        string ranking = null;

        OrdenadorService.OrdenarRanking(Jogadores, esq, dir);

        for (int i = 0; i < Jogadores.Length; i++)
        {
            ranking = $"Ranking da partida: \n{Jogadores[i].ToString()}";
            Jogadores[i].AtualizarPosicao(i + 1);
        }


        if (ranking != null)
            _logger.Registrar($"Ranking final da partida: {ranking}");

        return ranking!;
    }

    public IEnumerable<int> PesquisarHistoricoJogador(string nomeJogador)
    {
        Jogador pesquisado = null;
        IEnumerable<int> posicoes = null;
        foreach (Jogador j in Jogadores)
        {
            if (j.Nome.Equals(nomeJogador))
            {
                pesquisado = j;
                posicoes = pesquisado.GetHistorico();
            }
        }

        if (posicoes != null)
            foreach (int posicao in posicoes)
            {
                _logger.Registrar($"Historico do jogador {nomeJogador}: \n {posicao + "°, "}");
            }

        return posicoes!;
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

    public void SalvarLogAutomatico()
    {
        string pastaLogs = "Logs";

        if (!Directory.Exists(pastaLogs))
            Directory.CreateDirectory(pastaLogs);

        string nomeArquivo = "log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

        string caminhoFinal = pastaLogs + "/" + nomeArquivo;

        _logger.SalvarParaArquivo(caminhoFinal);
    }
}