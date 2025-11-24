using System;
using System.Collections.Generic;

namespace RoubaMontesAED.Entities
{
    public class AreaDescarte
    {
        private List<Carta> _cartasDescarte;

        public AreaDescarte()
        {
            _cartasDescarte = new List<Carta>();
        }

        public void Adicionar(Carta carta)
        {
            if (carta == null)
                return;

            _cartasDescarte.Add(carta);
        }

        public bool Remover(Carta carta)
        {
            if (carta == null)
                return false;

            return _cartasDescarte.Remove(carta);
        }

        public Carta ProcurarPorValor(int valor)
        {
            int i;
            int quantidade = _cartasDescarte.Count;

            for (i = 0; i < quantidade; i++)
            {
                Carta atual = _cartasDescarte[i];

                if (atual.Valor == valor)
                    return atual;
            }

            return null;
        }

        public List<Carta> GetTodas()
        {
            return new List<Carta>(_cartasDescarte);
        }

        public int Quantidade()
        {
            return _cartasDescarte.Count;
        }
    }
}
