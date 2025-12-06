using RoubaMontesAED.Entities;

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

    public JogoService(int quantidadeJogadores, int quantidadeCartasPermitidas)
    {
        this._quantidadeJogadores = quantidadeJogadores;
        this._quantidadeCartasPermitidas = quantidadeCartasPermitidas;
        this.MonteCompra = new MonteCompra();
        this.AreaDescarte = new AreaDescarte();
        this.Jogadores = new Jogador[quantidadeJogadores];
        this.random = new Random();
    }

    public void CriarBaralho()
    {
        // log que foi criado o baralho
        MonteCompra.CriarBaralho(_quantidadeCartasPermitidas);
    }

    public void EmbaralharCartas()
    {
        // log que foi embaralhado
        MonteCompra.Embaralhar();
    }

    public Jogador JogadorIniciaPartida()
    {
        // log que indica qual jogador foi sorteado para começar a partida (Depois vai em sentido horário).
        int indiceJogador = random.Next(0, _quantidadeJogadores);

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

        return proximo;
    }

    public Carta JogadaPadrao()
    {
        // Denominada Jogada Padrão quando jogador atual retira a carta de cima do monte e exibe para todos os jogadores (CARTA DA VEZ).
        throw new NotImplementedException();
    }


    public Jogador MaiorMonte()
    {
        throw new NotImplementedException();
        // método para verificar qual jogador tem o maior monte para roubar.
    }



    public void RankingPartida()
    {
        //Exibir Ranking da partida ao finalizar com dados e ordenados pela quantidade do Monte(maior primeiro).
        // implementar método que utiliza a RankingService para obter o ranking da partida e exibir
    }



    public IEnumerable<int> PesquisarHistoricoJogador(string nomeJogador)
    {
        throw new NotImplementedException();
        // retornar histórico do jogador pesquisado(Fila de posições passadas)
    }

}