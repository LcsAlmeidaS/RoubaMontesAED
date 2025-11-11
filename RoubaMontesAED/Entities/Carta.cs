namespace RoubaMontesAED.Entities;

public class Carta
{
    private int _numero;
    private string _naipe;

    public Carta(int Numero, string Naipe)
    {
        _numero = Numero;
        _naipe = Naipe;
    }
    
    public override string ToString()
    {
        string nome;
        switch (_numero)
        {
            case 1:
                nome = "ÁS";
                break;
            case 2:
            case 11:
                nome = "Valete";
                break;
            case 12:
                nome = "Dama";
                break;
            case 13:
                nome = "Rei";
                break;
            default:
                nome = _numero.ToString();
                break;
        }

        return $"{nome} de {_naipe}";
    }
}