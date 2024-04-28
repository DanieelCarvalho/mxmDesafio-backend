using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using RevendaCarros_Dc.Domain.Context;
using RevendaCarros_Dc.Domain.Dto;
using RevendaCarros_Dc.Domain.Model;
using RevendaCarros_Dc.Infra.Repositories.Interfaces;
using System.Data;

namespace RevendaCarros_Dc.Controllers;


[ApiController]
[Route("/revendas")]
public class RevendaController : ControllerBase
{
   private readonly IRevendaRepository _revendaRepository;
    private readonly IMapper _mapper;

    public RevendaController(IRevendaRepository revendaRepository, IMapper mapper)
    {
        _revendaRepository = revendaRepository;
        _mapper = mapper;
    }


    /// <summary>
    /// Cria um novo anúncio de carro.
    /// </summary>
    /// <remarks>
    /// Cria um novo anúncio de carro.
    /// </remarks>
    /// <param name="criarAnuncio">Os detalhes do novo anúncio.</param>
    /// <returns>Um status HTTP indicando o resultado da criação.</returns>
    /// <response code="200">Retorna uma mensagem de sucesso.</response>
    /// <response code="400">Se os dados fornecidos forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [ProducesResponseType(typeof(SuccessCarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CriarAnuncio(CriarAnuncioDto criarAnuncio)
    {
        try
        {
            if(criarAnuncio == null) 
            {
                return BadRequest(ModelState);
            }

        var newAnuncio = _mapper.Map<Carro>(criarAnuncio);
        await _revendaRepository.Save(newAnuncio);
        var response = new SuccessCarResponse { 
            Sucesso = "Anúncio de carro criado com sucesso.",
            Id = newAnuncio.Id };

        return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }

    /// <summary>
    /// Obtém todos os carros
    /// </summary>
    /// <remarks>
    /// Retorna uma lista paginada de carros ativos.
    /// </remarks>
    /// <param name="page">O número da página.</param>
    /// <param name="size">O tamanho da página</param>
    /// <param name="modelo"></param>
    /// <returns></returns>
    /// <returns>Uma lista paginada de carros ativos.</returns>
    /// <response code="200">Retorna uma lista paginada de carros ativos.</response>
    /// <response code="404">Se houver requisição não encontrada.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [ProducesResponseType(typeof(IEnumerable<CarViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpGet]
    [Route("getCars")]
    public async Task<IActionResult> PegarTodos([FromQuery(Name ="page")] int page = 0,
                                                [FromQuery(Name = "size")] int size = 0,
                                                [FromQuery(Name ="Modelo")] string modelo = "",
                                                [FromQuery(Name ="sort")] string sort = "")                      
    {
        try
        {
            
            var offset = page * size;

            var cars = await _revendaRepository.FindAll();
            if(cars == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(modelo))
            {
                cars = cars.Where(c => c.Modelo.ToLower().Contains(modelo.ToLower())).ToList();
            }

            cars = cars.Where(t => t.EstaAtivo == true).ToList();

             
            
            switch (sort)
            {
                case "preco_asc":
                    cars = cars.OrderBy(c => c.Preco).ToList();
                    break;
                case "preco_desc":
                    cars = cars.OrderByDescending(c => c.Preco).ToList();
                    break;
             
                    
                default: cars = cars.OrderByDescending(c => c.CriadoEm).ToList();
                    break;

            }



            cars = cars.Skip(offset).Take(size).ToList();
            var viewCars = _mapper.Map<IEnumerable<CarViewDto>>(cars);
            
         
            return Ok(viewCars);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }

    /// <summary>
    /// Obtém um carro pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do carro.</param>
    /// <returns>O carro correspondente ao ID fornecido.</returns>
    /// <response code="200">Retorna o carro correspondente ao ID fornecido.</response>
    /// <response code="404">Se houver requisição não encontrada</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [ProducesResponseType(typeof(CarViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

    [HttpGet("{id:int}")]

    public async Task<IActionResult> PegarPorId(int id)
    {
        try
        {
            var car = await _revendaRepository.FindById(id);

            if (car == null)
            {
                return NotFound(); 
            }

            var viewCar = _mapper.Map<CarViewDto>(car);
            return Ok(viewCar);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
       
    }

    /// <summary>
    /// Exporta os dados da tabela de carros para um arquivo Excel.
    /// </summary>
    /// <returns>O arquivo Excel contendo os dados da venda de carros.</returns>
    /// <response code="200">Retorna um arquivo Excel contendo os dados da venda de carros.</response>
   
    [HttpGet]
    [Route("export")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    public IActionResult Export()
    {

        var dados = GetDados().Result;
       

        using (XLWorkbook workbook = new XLWorkbook())
        {
            workbook.AddWorksheet(dados, "Venda de Carros Dados");

            using (MemoryStream ms = new MemoryStream())
            {
                var fileName = "RevendaCarros.xls";
                workbook.SaveAs(ms);
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                  fileName);
            }
        }


    }

    /// <summary>
    /// Atualiza um carro existente.
    /// </summary>
    /// <param name="id">O ID do carro a ser atualizado.</param>
    /// <param name="updateCarDto">Os novos detalhes do carro.</param>
    /// <returns>Um status HTTP indicando o resultado da atualização.</returns>
    /// <response code="200">Retorna um objeto com as modificações.</response>
    /// <response code="400">Se os dados fornecidos forem inválidos.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [ProducesResponseType(typeof(UpdateCarDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPut]
    [Route("{id}")]

    public async Task<IActionResult> UpdateCar(int id, UpdateCarDto updateCarDto)
    {
        if (id <= 0)
        {
            return BadRequest("ID inválido");
        }

        var existingCar = await _revendaRepository.FindById(id);

        if (existingCar == null)
        {
            return NotFound("Tarefa não encontrada");
        }

        existingCar.Ano = updateCarDto.Ano;
        existingCar.Modelo = updateCarDto.Modelo;
        existingCar.Preco = updateCarDto.Preco;
        existingCar.Portas = updateCarDto.Portas;

        existingCar.AtualizadoEm = DateTime.Now;


        await _revendaRepository.Update(existingCar);
        var responseText = new
        {
            Message = "Successful",
            CreatedAt = existingCar.AtualizadoEm
        };

        return Ok(responseText);
    }
    /// <summary>
    /// Exclui um carro existente.
    /// </summary>
    /// <param name="id">O ID do carro a ser excluído.</param>
    /// <returns>Um status HTTP indicando o resultado da exclusão.</returns>
    /// <response code="204">Apaga um carro da tabela.</response>
    /// <response code="400">Se houver um erro na requisição.</response>
    /// <response code="500">Se ocorrer um erro interno do servidor.</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpDelete]
    [Route("{id}")]

    public async Task<IActionResult> DeleteCar(int id)
    {

        if (id <= 0)
        {
            return BadRequest("ID inválido");
        }

        var existingCar = await _revendaRepository.FindById(id);

        if (existingCar == null)
        {
            return NotFound("Carro não encontrado");
        }

        
        existingCar.EstaAtivo = false;

       
        await _revendaRepository.Update(existingCar);
        

        return NoContent();
    }

    

    private async Task<DataTable> GetDados()
    {
        DataTable dataTable = new DataTable();

        dataTable.TableName = "Vendas de Carro";

        dataTable.Columns.Add("Modelo", typeof(string));
        dataTable.Columns.Add("Ano", typeof(string));
        dataTable.Columns.Add("Preco", typeof(double));
        dataTable.Columns.Add("Portas", typeof(string));
        dataTable.Columns.Add("CriandoEm", typeof(DateTime));
        dataTable.Columns.Add("AtualizadoEm", typeof(DateTime));

        var dados = await _revendaRepository.FindAll();
        dados = dados.Where(t => t.EstaAtivo == true).ToList();

        if (dados != null)
        {
            foreach (var dado in dados)
            {
                dataTable.Rows.Add(
                    dado.Modelo,
                    dado.Ano,
                   dado.Preco,
                    dado.Portas,
                    dado.CriadoEm,
                    dado.AtualizadoEm
                );
            }
        }

        


        return dataTable;
    }


}
