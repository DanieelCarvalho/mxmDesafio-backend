using System.ComponentModel.DataAnnotations;

namespace RevendaCarros_Dc.Domain.Dto;

public class CriarAnuncioDto
{
    [Required(ErrorMessage = "O Modelo é obrigatório.")]
    [MaxLength(10)]

    public string Modelo { get; set; }


    [Required(ErrorMessage = "O ano é obrigatório.")]
    [Range(1950, 2025, ErrorMessage = "Por favor, insira um ano válido.")]
    public int Ano { get; set; }

   [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 2000000, ErrorMessage = "O preço deve estar entre 0.01 e 2,000,000.")]
    public double Preco { get; set; }
   

    [Required(ErrorMessage = "O número de portas é obrigatório.")]
    [Range(2, 4, ErrorMessage = "O número de portas deve estar entre 2 e 4.")]
    public int Portas { get; set; }

    public bool? EstaAtivo { get; set; } = true;
   


}
