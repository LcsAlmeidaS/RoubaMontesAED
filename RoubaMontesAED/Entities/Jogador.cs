using System;
using System.Collections.Generic;

namespace RoubaMontesAED.Entities
{
    public class Jogador
    {
        public string Nome { get; private set; }
        public int Posicao { get; private set; }
        public int QuantidadeCartasUltimaPartida { get; private set; }

        private Queue<int> _historicoPosicoes;
        private MonteJogador _monteJogador;

        public Jogador(string nome)
        {
            Nome = nome;
            Posicao = 0;
            QuantidadeCartasUltimaPartida = 0;

            _historicoPosicoes = new Queue<int>();
            _monteJogador = new MonteJogador();
        }

        public void ReceberCartasRoubadas(IEnumerable<Carta> cartas)
        {
            if (cartas == null)
                return;

            _monteJogador.ColocarNoTopoMonteRoubado(cartas);
        }

        public void ReceberCarta(Carta carta)
        {
            if (carta == null)
                return;

            _monteJogador.ColocarNoTopo(carta);
        }

        public Carta TopoDoMonteJogador()
        {
            return _monteJogador.Topo();
        }

        public int TamanhoDoMonteJogador()
        {
            return _monteJogador.Tamanho();
        }

        public List<Carta> RetirarMonte()
        {
            return _monteJogador.RetirarTodas();
        }

        public void AtualizarPosicao(int posicao)
        {
            Posicao = posicao;
            QuantidadeCartasUltimaPartida = _monteJogador.Tamanho();

            if (_historicoPosicoes.Count == 5)
                _historicoPosicoes.Dequeue();

            _historicoPosicoes.Enqueue(posicao);
        }

        public IEnumerable<int> GetHistorico()
        {
            return _historicoPosicoes.Reverse();
        }

        public override string ToString()
        {
            return Nome + " - Posição: " + Posicao + ", Cartas: " + QuantidadeCartasUltimaPartida;
        }
    }
}
