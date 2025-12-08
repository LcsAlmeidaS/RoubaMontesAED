namespace RoubaMontesAED.IO;

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

    public void SalvarParaArquivo(string caminho)
    {
        File.WriteAllLines(caminho, _linhas);
    }

    public List<string> GetLinhas()
    {
        return new List<string>(_linhas);
    }

}