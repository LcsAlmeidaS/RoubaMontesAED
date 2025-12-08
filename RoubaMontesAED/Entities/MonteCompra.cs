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
            if (quantidadeMaximaCartas <= 0)
                throw new ArgumentException("Quantidade de cartas deve ser maior que zero.");

            _cartas.Clear();

            Naipe[] naipes = { Naipe.Ouros, Naipe.Espadas, Naipe.Copas, Naipe.Paus };

            for (int i = 0; i < quantidadeMaximaCartas; i++)
            {
                int valor = (i % 13) + 1;
                Naipe naipe = naipes[i % naipes.Length];

                _cartas.Add(new Carta(valor, naipe));
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