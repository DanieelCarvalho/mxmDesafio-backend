namespace RevendaCarros_Dc.Domain.Dto;

public class CarViewDto
{
    public int Id { get; set; }
    public string Modelo { get; set; }

    public int Ano { get; set; }
    public double Preco { get; set; }
    public int Portas { get; set; }
    public DateTime? CriadoEm { get; set; } = DateTime.Now;

    public DateTime AtualizadoEm { get; set; } = DateTime.Now;

}
