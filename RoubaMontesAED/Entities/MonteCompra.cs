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

        public void CriarBaralho(int quantidadeCartas)
        {
            _cartas.Clear();

            int i;
            for (i = 1; i <= quantidadeCartas; i++)
            {
                int valor = i % 13;
                if (valor == 0)
                    valor = 13;

                Naipe naipe;
                if (i % 4 == 0)
                    naipe = Naipe.Copas;
                else if (i % 4 == 1)
                    naipe = Naipe.Ouros;
                else if (i % 4 == 2)
                    naipe = Naipe.Paus;
                else
                    naipe = Naipe.Espadas;

                Carta novaCarta = new Carta(valor, naipe);
                _cartas.Add(novaCarta);
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