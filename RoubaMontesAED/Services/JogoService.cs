using RoubaMontesAED.Entities;
using RoubaMontesAED.IO;
using System.Text;

namespace RoubaMontesAED.Services
{
    public class JogoService
    {
        private int _quantidadeJogadores;
        private MonteCompra MonteCompra;
        private AreaDescarte AreaDescarte;
        private Jogador[] Jogadores;
        private Random random;
        private LogPartida _logger;
        private int indiceJogadorInicial;

        public JogoService(int quantidadeJogadores)
        {
            _quantidadeJogadores = quantidadeJogadores;
            MonteCompra = new MonteCompra();
            AreaDescarte = new AreaDescarte();
            Jogadores = new Jogador[quantidadeJogadores];
            random = new Random();
            _logger = new LogPartida();
        }

        public void DefinirJogadores(Jogador[] jogadores)
        {
            Jogadores = jogadores;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < jogadores.Length; i++)
            {
                sb.Append(jogadores[i].Nome);
                if (i < jogadores.Length - 1)
                    sb.Append(", ");
            }

            _logger.Registrar("Jogadores definidos: " + sb.ToString());
        }

        public void CriarBaralho(int quantidade)
        {
            MonteCompra.CriarBaralho(quantidade);
            _logger.Registrar("Baralho criado com " + quantidade + " cartas.");
        }

        public void Embaralhar()
        {
            MonteCompra.Embaralhar();
            _logger.Registrar("Baralho embaralhado.");
        }

        public void SortearJogadorInicial()
        {
            indiceJogadorInicial = random.Next(0, Jogadores.Length);
            _logger.Registrar("Jogador sorteado para iniciar: " + Jogadores[indiceJogadorInicial].Nome);
        }

        public void IniciarPartida()
        {
            Jogador atual = Jogadores[indiceJogadorInicial];

            while (!MonteCompra.EstaVazio())
            {
                ExecutarTurno(atual);
                if (!MonteCompra.EstaVazio())
                {
                    atual = PassarVez(atual);
                }
            }

            string ranking = RankingService.GerarRankingFinal(Jogadores);
            _logger.Registrar(ranking);
        }

        private Jogador PassarVez(Jogador jogadorAtual)
        {
            int indice = 0;

            for (int i = 0; i < Jogadores.Length; i++)
            {
                if (Jogadores[i] == jogadorAtual)
                {
                    indice = i;
                    break;
                }
            }

            int proximoIndice = (indice + 1) % Jogadores.Length;
            Jogador proximo = Jogadores[proximoIndice];

            _logger.Registrar("Passando a vez: " + jogadorAtual.Nome + " → " + proximo.Nome);
            return proximo;
        }

        private void ExecutarTurno(Jogador jogadorAtual)
        {
            Carta cartaDaVez = ComprarCarta();
            if (cartaDaVez == null)
                return;

            bool continuar = true;

            while (continuar && cartaDaVez != null)
            {
                continuar = false;

                _logger.SeparadorJogada(jogadorAtual.Nome);
                AtualizarEstadoVisual();

                Jogador alvoRoubo = VerificarRoubo(jogadorAtual, cartaDaVez);
                if (alvoRoubo != null)
                {
                    ExecutarRoubo(jogadorAtual, alvoRoubo, cartaDaVez);
                    AtualizarEstadoVisual();

                    cartaDaVez = ComprarCarta();
                    continuar = cartaDaVez != null;
                    continue;
                }

                Carta cartaDescarte = AreaDescarte.ProcurarPorValor(cartaDaVez.Valor);
                if (cartaDescarte != null)
                {
                    ExecutarCapturaDescarte(jogadorAtual, cartaDaVez, cartaDescarte);
                    AtualizarEstadoVisual();

                    cartaDaVez = ComprarCarta();
                    continuar = cartaDaVez != null;
                    continue;
                }

                if (PodeEmpilhar(jogadorAtual, cartaDaVez))
                {
                    jogadorAtual.ReceberCarta(cartaDaVez);
                    _logger.Registrar(jogadorAtual.Nome + " empilhou a carta no próprio monte.");
                    AtualizarEstadoVisual();

                    cartaDaVez = ComprarCarta();
                    continuar = cartaDaVez != null;
                    continue;
                }

                ExecutarDescarte(jogadorAtual, cartaDaVez);
                AtualizarEstadoVisual();
                _logger.Registrar("Jogada encerrada.");

                return;
            }
        }

        private Carta ComprarCarta()
        {
            Carta carta = MonteCompra.Comprar();
            if (carta != null)
            {
                _logger.Registrar("Jogador comprou a carta: " + carta.ToString());
            }
            return carta;
        }

        private void AtualizarEstadoVisual()
        {
            _logger.RegistrarToposDosMontes(Jogadores);
            _logger.RegistrarCartasDescarte(AreaDescarte);
        }

        private Jogador VerificarRoubo(Jogador jogadorAtual, Carta carta)
        {
            List<Jogador> candidatos = new List<Jogador>();
            int maior = -1;

            for (int i = 0; i < Jogadores.Length; i++)
            {
                Jogador j = Jogadores[i];

                if (j == jogadorAtual)
                    continue;

                Carta topo = j.TopoDoMonteJogador();
                if (topo == null)
                    continue;

                if (topo.Valor == carta.Valor)
                {
                    int tam = j.TamanhoDoMonteJogador();

                    if (tam > maior)
                    {
                        maior = tam;
                        candidatos.Clear();
                        candidatos.Add(j);
                    }
                    else if (tam == maior)
                    {
                        candidatos.Add(j);
                    }
                }
            }

            if (candidatos.Count == 0)
                return null;

            int escolha = random.Next(0, candidatos.Count);
            return candidatos[escolha];
        }

        private void ExecutarRoubo(Jogador quemRouba, Jogador quemPerde, Carta cartaDaVez)
        {
            List<Carta> monte = quemPerde.RetirarMonte();
            quemRouba.ReceberCartasRoubadas(monte);
            quemRouba.ReceberCarta(cartaDaVez);

            _logger.Registrar(quemRouba.Nome + " ROUBOU o monte de " + quemPerde.Nome);
        }

        private void ExecutarCapturaDescarte(Jogador jogador, Carta cartaDaVez, Carta cartaDescarte)
        {
            AreaDescarte.Remover(cartaDescarte);
            jogador.ReceberCarta(cartaDescarte);
            jogador.ReceberCarta(cartaDaVez);

            _logger.Registrar(jogador.Nome + " capturou carta do descarte: " + cartaDescarte.ToString());
        }

        private bool PodeEmpilhar(Jogador jogador, Carta carta)
        {
            Carta topo = jogador.TopoDoMonteJogador();
            if (topo == null)
                return false;

            return topo.Valor == carta.Valor;
        }

        private void ExecutarDescarte(Jogador jogador, Carta carta)
        {
            AreaDescarte.Adicionar(carta);
            _logger.Registrar(jogador.Nome + " descartou: " + carta.ToString());
        }

        public void SalvarLogAutomatico()
        {
            string pasta = "Logs";
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string arquivo = "log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string caminho = Path.Combine(pasta, arquivo);
            _logger.SalvarParaArquivo(caminho);
        }

        public void SalvarLogAutomatico(string nomeArquivo)
        {
            string pasta = "Logs";
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string caminho = Path.Combine(pasta, nomeArquivo);
            _logger.SalvarParaArquivo(caminho);
        }

        public IEnumerable<int> PesquisarHistoricoJogador(string nome)
        {
            Jogador encontrado = null;

            for (int i = 0; i < Jogadores.Length; i++)
            {
                if (Jogadores[i].Nome.ToLower() == nome.ToLower())
                {
                    encontrado = Jogadores[i];
                    break;
                }
            }

            if (encontrado == null)
            {
                _logger.Registrar("Jogador '" + nome + "' não encontrado.");
                return new List<int>();
            }

            IEnumerable<int> hist = encontrado.GetHistorico();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Histórico das últimas posições de " + nome + ":");

            int cont = 0;
            foreach (int pos in hist)
            {
                sb.AppendLine(pos + "º lugar");
                cont++;
            }

            if (cont == 0)
                sb.AppendLine("(sem histórico registrado)");

            _logger.Registrar(sb.ToString());

            return hist;
        }
    }
}
