using RoubaMontesAED.Entities;
using System.Text;

namespace RoubaMontesAED.Services
{
    public static class RankingService
    {
        public static string GerarRankingFinal(Jogador[] jogadores)
        {
            OrdenadorService.OrdenarRanking(jogadores, 0, jogadores.Length - 1);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Ranking final da partida:");

            int posicao = 1;
            int cartasTopo = jogadores[0].TamanhoDoMonteJogador();

            for (int i = 0; i < jogadores.Length; i++)
            {
                int cartas = jogadores[i].TamanhoDoMonteJogador();

                if (cartas == cartasTopo)
                {
                    jogadores[i].AtualizarPosicao(1);
                    sb.AppendLine($"1º - {jogadores[i].Nome} ({cartas} cartas)");
                }
                else
                {
                    posicao++;
                    jogadores[i].AtualizarPosicao(i + 1);
                    sb.AppendLine($"{i + 1}º - {jogadores[i].Nome} ({jogadores[i].TamanhoDoMonteJogador()} cartas)");
                }
            }

            return sb.ToString();
        }
    }
}
