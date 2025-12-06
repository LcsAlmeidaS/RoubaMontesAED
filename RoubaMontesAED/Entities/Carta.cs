using RoubaMontesAED.Enums;


namespace RoubaMontesAED.Entities;

public class Carta
{
    public int Valor { get; set; }
    public Naipe Naipe { get; set; }    

    public Carta(int valor, Naipe naipe)
    {
        this.Valor = valor;
        this.Naipe = naipe;
    }

    public override string ToString()
    {
        return $"Carta {Valor} de {Naipe}.";
    }

}