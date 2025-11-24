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
            _cartas.Clear();

            Naipe[] naipes = (Naipe[])Enum.GetValues(typeof(Naipe));

            int indice = 0;

            while (indice < quantidadeMaximaCartas)
            {
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
                int k = _random.Next(n + 1);

                Carta temp = _cartas[k];
                _cartas[k] = _cartas[n];
                _cartas[n] = temp;
            }
        }

        public Carta Comprar()
        {
            if (_cartas.Count == 0)
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