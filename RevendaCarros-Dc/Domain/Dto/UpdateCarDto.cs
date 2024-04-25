using System.ComponentModel.DataAnnotations;

namespace RevendaCarros_Dc.Domain.Dto;

public class UpdateCarDto
{
    [Required]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "O ano é obrigatório.")]

    [Range(1950, 2025, ErrorMessage = "Por favor, insira um ano válido.")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public double Preco { get; set; }

    [Required(ErrorMessage = "O número de portas é obrigatório.")]

    [Range(2, 4, ErrorMessage = "O número de portas deve estar entre 2 e 4.")]
    public int Portas { get; set; }

    public int Id { get; set; }


}
