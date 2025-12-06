// See https://aka.ms/new-console-template for more information

using RoubaMontesAED.Services;

const string Separador = "==================================";

Console.WriteLine("Código Main");
ContinuarPartida();




void Config(JogoService jogo)
{
    // Método que irá configurar o jogo antes de iniciar. (verificar lógica posteriormente)
    // chamar sorteio de jogador que irá iniciar a partida, criar baralho, embaralhar, etc...
}


// Fazer aqui a parte de perguntar se o jogador deseja continuar ou não a partida.
int ContinuarPartida()
{
    Console.WriteLine(Separador);
    Console.WriteLine("Deseja iniciar uma nova partida?");
    Console.WriteLine("1) Sim, iniciar uma nova partida.");
    Console.WriteLine("2) Não, sair do jogo.");
    return int.Parse(Console.ReadLine());
}
