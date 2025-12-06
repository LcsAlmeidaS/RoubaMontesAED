using RoubaMontesAED.Enums;
using System;
using System.Collections.Generic;

namespace RoubaMontesAED.Entities
{
    public class MonteCompra
    {
        private List<Carta> _cartas;
        private Random _random;

        public MonteCompra()
        {
            _cartas = new List<Carta>();
            _random = new Random();
        }

        public void CriarBaralho(int quantidadeMaximaCartas)
        {
            if (_cartas.Count > 0)
            {
                _cartas.Clear();
            }

            Naipe[] naipes = (Naipe[])Enum.GetValues(typeof(Naipe));

            int indice = 0;

            while (indice < quantidadeMaximaCartas)
            {
                // Porque valor tá com essa conta (Entendi que são os valores possíveis do baralho começando por 1)
                int valor = (indice % 13) + 1;
                Naipe naipe = naipes[indice % naipes.Length];

                Carta carta = new Carta(valor, naipe);
                _cartas.Add(carta);

                indice++;
            }
        }

        public void Embaralhar()
        {
            int n = _cartas.Count;

            while (n > 1)
            {
                n--;
                //Porque n + 1? Pra trocar a semente do random? 
                int k = _random.Next(n + 1);

                Carta temp = _cartas[k];
                _cartas[k] = _cartas[n];
                _cartas[n] = temp;
            }
        }

        //Responsável por gerar a carta da vez
        public Carta Comprar()
        {
            // Se o monte de compras for 0 acabou o jogo (Conferir verificação se é necessária) --> Coloquei o método pois tava duplicando
                                                                                                   //Código
            if (EstaVazio())
                return null;

            int ultimoIndice = _cartas.Count - 1;
            Carta carta = _cartas[ultimoIndice];

            _cartas.RemoveAt(ultimoIndice);
            return carta;
        }

        public bool EstaVazio()
        {
            return _cartas.Count == 0;
        }

        public int Quantidade()
        {
            return _cartas.Count;
        }
    }
}