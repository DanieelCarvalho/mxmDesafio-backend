using System.ComponentModel.DataAnnotations;

namespace RevendaCarros_Dc.Domain.Model;

public class Carro : Entity 
{

  
    public string Modelo { get; set; }

    public int Ano { get; set; }
    public double Preco { get; set; }
    public int Portas { get; set; }

    public DateTime? CriadoEm { get; set; } = DateTime.Now;
    
    public DateTime? DeletadoEm { get; set; }

    public DateTime AtualizadoEm { get; set; } = DateTime.Now;
    public bool? EstaAtivo { get; set; } = null;
    
}
