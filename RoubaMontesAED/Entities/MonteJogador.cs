using RoubaMontesAED;

namespace RoubaMontesAED.Entities
{
    public class MonteJogador
    {
        private Stack<Carta> _pilha;

        public MonteJogador()
        {
            _pilha = new Stack<Carta>();
        }

        public void ColocarNoTopo(IEnumerable<Carta> cartas)
        {
            if (cartas == null)
                return;

            foreach (Carta carta in cartas)
                _pilha.Push(carta);
        }

        public void ColocarNoTopo(Carta carta)
        {
            if (carta == null)
                return;

            _pilha.Push(carta);
        }

        public Carta Topo()
        {
            if (_pilha.Count == 0)
                return null;

            return _pilha.Peek();
        }

        public List<Carta> RoubarMonte()
        {
            List<Carta> lista = new List<Carta>();

            while (_pilha.Count > 0)
            {
                Carta removida = _pilha.Pop();
                lista.Add(removida);
            }

            lista.Reverse();
            return lista;
        }

        public int Tamanho()
        {
            return _pilha.Count();
        }

        public void Limpar()
        {
            _pilha.Clear();
        }

        public List<Carta> GetCartas()
        {
            return _pilha.ToList();
        }

        public bool EstahVazia()
        {
            return _pilha.Count == 0;
        }
    }
}