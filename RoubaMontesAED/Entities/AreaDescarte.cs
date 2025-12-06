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
            int quantidade = _cartasDescarte.Count;
            Carta atual = null;

            for (int i = 0; i < quantidade; i++)
            {
                atual = _cartasDescarte[i];

                if (atual.Valor == valor)
                    return atual;
            }

            return atual;
        }

        //Dnv ta criando uma nova sendo que pode só percorrer a lista? 
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
