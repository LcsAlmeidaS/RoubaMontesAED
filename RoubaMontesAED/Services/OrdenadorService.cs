using RoubaMontesAED.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoubaMontesAED.Services
{
    public static class OrdenadorService
    {
        public static void OrdenarRanking(Jogador[] jogadores, int inicio, int fim)
        {
            int i = inicio, j = fim;
            Jogador pivo = jogadores[(inicio + fim) / 2];


            while (i <= j)
            {
                while (jogadores[i].TamanhoDoMonteJogador() > pivo.TamanhoDoMonteJogador())
                {
                    i++;
                }
                while (jogadores[i].TamanhoDoMonteJogador() < pivo.TamanhoDoMonteJogador())
                {
                    j--;
                }

                if (i <= j)
                {
                    Trocar(jogadores, i, j);
                }

                if(inicio < j)
                {
                    OrdenarRanking(jogadores, inicio, j);
                }
                if(i < fim)
                {
                    OrdenarRanking(jogadores, i, fim);
                }
            }
        }
        private static void Trocar(Jogador [] jogadores, int i, int j)
        {
            Jogador jogador = jogadores[i];
            jogadores[i] = jogadores[j];
            jogadores[j] = jogador;
        }
    }
}
