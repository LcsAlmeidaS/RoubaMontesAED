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
            for (int i = 0; i < _cartasDescarte.Count; i++)
            {
                Carta c = _cartasDescarte[i];

                if (c.Valor == valor)
                    return c;
            }

            return null;
        }

        public List<Carta> GetTodas()
        {
            return _cartasDescarte;
        }

        public int Quantidade()
        {
            return _cartasDescarte.Count;
        }
    }
}
