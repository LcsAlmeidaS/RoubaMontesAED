using RoubaMontesAED.Entities;
using RoubaMontesAED.Services;

namespace RoubaMontesAED
{
    internal class Program
    {
        private const int QtdCartasBaralho = 52;
        static Dictionary<string, Jogador> todosJogadores = new Dictionary<string, Jogador>();
        private const string Separador = "==================================";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("ROUBA MONTES — INICIALIZAÇÃO");
                Console.WriteLine(Separador);

                int opcao = PerguntarOpcaoInicial();
                if (opcao == 2)
                {
                    Console.WriteLine("Jogo encerrado.");
                    return;
                }

                Console.WriteLine(Separador);
                Console.WriteLine("CONFIGURAÇÃO DE NOVA PARTIDA");

                int qtdJogadores = LerInteiro("Quantidade de jogadores (mínimo 2): ", 2);
                int qtdBaralhos = LerInteiro("Quantidade de baralhos (1 = 52 cartas, 2 = 104 cartas...): ", 1);

                int qtdCartas = qtdBaralhos * QtdCartasBaralho;

                JogoService jogo = new JogoService(qtdJogadores);
                Jogador[] jogadores = LerJogadores(qtdJogadores);

                jogo.DefinirJogadores(jogadores);
                jogo.CriarBaralho(qtdCartas);
                jogo.Embaralhar();
                jogo.SortearJogadorInicial();

                jogo.IniciarPartida();

                Console.WriteLine();
                Console.WriteLine("Deseja consultar o histórico de um jogador?");
                Console.WriteLine("1) Sim");
                Console.WriteLine("2) Não");

                int hist = LerInteiro("Opção: ", 1, 2);
                if (hist == 1)
                {
                    Console.Write("Digite o nome do jogador: ");
                    string nome = Console.ReadLine();
                    jogo.PesquisarHistoricoJogador(nome);
                }

                SalvarLog(jogo);

                Console.WriteLine(Separador);
            }
        }

        static int PerguntarOpcaoInicial()
        {
            Console.WriteLine("Deseja iniciar nova partida?");
            Console.WriteLine("1) Sim");
            Console.WriteLine("2) Não (sair)");
            return LerInteiro("Opção: ", 1, 2);
        }

        static int LerInteiro(string mensagem, int minimo = int.MinValue, int maximo = int.MaxValue)
        {
            while (true)
            {
                Console.Write(mensagem);
                string entrada = Console.ReadLine();

                int valor;
                bool ok = int.TryParse(entrada, out valor);

                if (!ok)
                {
                    Console.WriteLine("Valor inválido. Tente novamente.");
                    continue;
                }

                if (valor < minimo || valor > maximo)
                {
                    Console.WriteLine("Valor fora do intervalo permitido.");
                    continue;
                }

                return valor;
            }
        }

        static Jogador[] LerJogadores(int quantidade)
        {
            Jogador[] jogadores = new Jogador[quantidade];

            for (int i = 0; i < quantidade; i++)
            {
                Console.Write("Nome do jogador " + (i + 1) + ": ");
                string nome = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Nome inválido, tente novamente.");
                    i--;
                    continue;
                }

                Jogador jogador;

                if (todosJogadores.ContainsKey(nome.ToLower()))
                {
                    jogador = todosJogadores[nome.ToLower()];
                }
                else
                {
                    jogador = new Jogador(nome);
                    todosJogadores.Add(nome.ToLower(), jogador);
                }

                jogadores[i] = jogador;
            }

            return jogadores;
        }

        static void SalvarLog(JogoService jogo)
        {
            Console.Write("Digite um nome para salvar o log (ex: partida1.txt). Deixe vazio para nome automático: ");
            string nome = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nome))
                jogo.SalvarLogAutomatico();
            else
                jogo.SalvarLogAutomatico(nome);

            Console.WriteLine("Partida finalizada. Log salvo com sucesso!");
        }
    }
}
