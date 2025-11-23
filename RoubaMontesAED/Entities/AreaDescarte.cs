using System.Collections.Generic;
using System.Linq;

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
            for (i = 0; i < _cartasDescarte.Count; i++)
            {
                if (_cartasDescarte[i].Valor == valor)
                    return _cartasDescarte[i];
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