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

            for (int i = 0; i < jogadores.Length; i++)
            {
                jogadores[i].AtualizarPosicao(i + 1);
                sb.AppendLine($"{i + 1}º - {jogadores[i].Nome} ({jogadores[i].TamanhoDoMonteJogador()} cartas)");
            }

            return sb.ToString();
        }
    }
}
