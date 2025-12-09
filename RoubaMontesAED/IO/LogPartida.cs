using RoubaMontesAED.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RoubaMontesAED.IO
{
    public class LogPartida
    {
        private List<string> _linhas;

        public LogPartida()
        {
            _linhas = new List<string>();
        }

        public void Registrar(string linha)
        {
            if (!string.IsNullOrWhiteSpace(linha))
                _linhas.Add(linha);
        }

        public void RegistrarToposDosMontes(Jogador[] jogadores)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("TOPO DOS MONTES: ");

            for (int i = 0; i < jogadores.Length; i++)
            {
                Carta topo = jogadores[i].TopoDoMonteJogador();

                string textoTopo = topo != null ? topo.ToString() : "[vazio]";

                sb.Append($"{jogadores[i].Nome}: {textoTopo}");

                if (i < jogadores.Length - 1)
                    sb.Append(" | ");
            }

            Registrar(sb.ToString());
        }

        public void RegistrarCartasDescarte(AreaDescarte descarte)
        {
            List<Carta> cartas = descarte.GetTodas();

            if (cartas.Count == 0)
            {
                Registrar("DESCARTE: [vazio]");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("DESCARTE: ");

            for (int i = 0; i < cartas.Count; i++)
            {
                sb.Append(cartas[i].ToString());

                if (i < cartas.Count - 1)
                    sb.Append(" | ");
            }

            Registrar(sb.ToString());
        }

        public void SalvarParaArquivo(string caminho)
        {
            File.WriteAllLines(caminho, _linhas);
        }

        public List<string> GetLinhas()
        {
            return new List<string>(_linhas);
        }
    }
}