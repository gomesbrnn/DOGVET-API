using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Animal;
using App.Helpers;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario,Cliente")]
    public class AnimaisController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AnimaisController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        /// <summary>
        /// Inclui um novo Animal
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Animais/Criar
        ///     {
        ///         "nome": "Bob",
        ///         "raca": "Pug",
        ///         "peso": "4kg",
        ///         "dataNascimento": "2022-01-04T17:42:57.012Z",
        ///         "tutorId": 1
        ///     }
        /// </remarks>
        /// <param name="animalDTO">Objeto Animal</param>
        /// <returns>O objeto Animal incluido</returns>
        /// <remarks>Retorna um objeto Animal incluído</remarks>

        [HttpPost("Criar")]
        public IActionResult Post(CriarAnimalDTO animalDTO)
        {
            try
            {
                if (animalDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                Animal animal = _mapper.Map<Animal>(animalDTO);

                _database.Animais.Add(animal);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", animal });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Exibe uma relação de todos os Animais
        /// </summary>
        /// <returns>Retorna uma lista de objeto Animais</returns>

        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var animal = await _database.Animais
                                                .Where(animal => animal.Status == true)
                                                .AsNoTracking()
                                                .ToListAsync();

                if (animal is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(animal);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem um Animal pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Animal</param>
        /// <returns>Objeto Animal</returns>

        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var animal = await _database.Animais
                                                .Include(animal => animal.Tutor)
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(animal => animal.Id == id);


                if (animal is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(animal);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem uma relação de Animais pelo Id do Tutor
        /// </summary>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>Objeto Animal</returns>

        [HttpGet("Tutor/{id}")]
        public async Task<IActionResult> GetAnimaisTutor(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var animal = await _database.Animais
                                                .Where(animal => animal.TutorId == id)
                                                .Include(animal => animal.Tutor)
                                                .AsNoTracking()
                                                .ToListAsync();


                if (animal is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(animal);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Exibe uma relação de cinco cachorros de uma API externa
        /// </summary>
        /// <returns>O objeto DogInfo</returns>

        [HttpGet("DogApi")]
        public async Task<IActionResult> GetDogs()
        {
            DogApiRequestHelper requestHelper = new();

            List<DogInfo> cachorros = await requestHelper.GetDogsAsync();

            return Ok(new { cachorros });
        }

        /// <summary>
        ///  Obtem um ou mais cachorros por nome de Raça
        /// </summary>
        /// <param name="raca">Nome da Raça do cachorro</param>
        /// <returns>O objeto DogInfo</returns>

        [HttpGet("DogApi/{raca}")]
        public async Task<IActionResult> GetDogsByRaca(string raca)
        {
            DogApiRequestHelper requestHelper = new();

            List<DogInfo> cachorro = await requestHelper.GetDogsByNaneAsync(raca);

            return Ok(new { cachorro });
        }

        /// <summary>
        /// Exibe uma relação de cinco imagens de cachorros de uma API externa
        /// </summary>
        /// <returns>O objeto DogImages</returns>

        [HttpGet("DogApi/Imagens")]
        public async Task<IActionResult> GetImagesAsync()
        {
            DogApiRequestHelper requestHelper = new();

            List<DogImages> imagens = await requestHelper.GetImagesAsync();

            return Ok(new { imagens });
        }

        /// <summary>
        /// Atualiza todos os dados do Animal
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT api/v1/Animais/Atualizar
        ///     {
        ///          "raca": "Pug Diferenciado",
        ///          "peso": "6kg",
        ///     }
        /// </remarks>
        /// <param name="animalDTO">Objeto Animal</param>
        /// <param name="id">Codigo do Animal</param>
        /// <returns>O objeto Animal atualizado</returns>
        /// <remarks>Retorna um objeto Animal atualizado</remarks>

        [HttpPut("Atualizar/{id}")]
        public IActionResult Put(AtualizarAnimalDTO animalDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var animal = _database.Animais.FirstOrDefault(animal => animal.Id == id);

                if (animal is null) return NotFound(new { msg = "Registro não encontrado" });

                animal = _mapper.Map(animalDTO, animal);
                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", animal });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Inativa um Animal pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Animal</param>
        /// <returns>Objeto Animal</returns>

        [HttpDelete("Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var animal = _database.Animais.FirstOrDefault(animal => animal.Id == id);
                var atendimentos = _database.Atendimentos.FirstOrDefault(atend => atend.AnimalId == id && atend.StatusAtendimento == true);

                if (animal is null) return NotFound(new { msg = "Registro não encontrado" });
                if (atendimentos is not null) return BadRequest(new { msg = "Este animal possui atendimentos pendentes, impossivel deletar!" });
                if (animal.Status == false) return BadRequest(new { msg = "O animal em questão já foi inativado!" });

                animal.Status = false;
                _database.SaveChanges();

                return Ok(new { msg = "Registro deletado:", animal });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }
    }
}