using RevendaCarros_Dc.Domain.Model;

namespace RevendaCarros_Dc.Domain.Dto;

public class CarResponse
{
    public string Modelo { get; set; }

    public int Ano { get; set; }
    public double Preco { get; set; }
    public int Portas { get; set; }
   

    public CarResponse(string modelo, int ano, double preco, int portas)
    {
        Modelo = modelo;
        Ano = ano;
        Preco = preco;
        Portas = portas;
    }

    public static CarResponse ConverteParaResponse(Carro carro)
    {
        return new CarResponse
        (
            carro.Modelo,
            carro.Ano,
            carro.Preco,
            carro.Portas
            
        );
    }
}
