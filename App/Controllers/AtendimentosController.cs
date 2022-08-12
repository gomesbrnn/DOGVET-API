using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Atendimento;
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
    public class AtendimentosController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AtendimentosController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        /// <summary>
        /// Inclui um novo Atendimento
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Atendimentos/Criar
        ///     {
        ///         "clinicaId": 1,
        ///         "veterinarioId": 1,
        ///         "tutorId": 1,
        ///         "animalId": 1,
        ///         "dataHora": "2022-05-15T17:05:47.123Z",
        ///         "dadosDoDia": "Peso atual do animal nessa consulta: 7kg",
        ///         "diagnostico": "Saudável",
        ///         "comentarios": "Manter cuidados atuais"
        ///     }
        /// </remarks>
        /// <param name="atendimentoDTO">Objeto Atendimento</param>
        /// <returns>O objeto Atendimento incluido</returns>
        /// <remarks>Retorna um objeto Atendimento incluído</remarks>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpPost("Criar")]
        public IActionResult Post(CriarAtendimentoDTO atendimentoDTO)
        {
            try
            {
                if (atendimentoDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                var clinica = _database.Clinicas.FirstOrDefault(clinica => clinica.Id == atendimentoDTO.ClinicaId);
                var veterinario = _database.Veterinarios.FirstOrDefault(veterinario => veterinario.Id == atendimentoDTO.VeterinarioId);
                var tutor = _database.Tutores.FirstOrDefault(tutor => tutor.Id == atendimentoDTO.TutorId);
                var animal = _database.Animais.FirstOrDefault(animal => animal.Id == atendimentoDTO.AnimalId);

                if (clinica is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });
                if (veterinario is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });
                if (tutor is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });
                if (animal is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                Atendimento atendimento = _mapper.Map<Atendimento>(atendimentoDTO);

                _database.Atendimentos.Add(atendimento);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", atendimento });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Exibe uma relação de todos os Atendimentos
        /// </summary>
        /// <returns>Retorna uma lista de objeto Atendimentos</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var atendimento = await _database.Atendimentos
                                                        .Where(atend => atend.StatusAtendimento == true)
                                                        .Include(atend => atend.Clinica)
                                                        .Include(atend => atend.Veterinario)
                                                        .Include(atend => atend.Tutor)
                                                        .Include(atend => atend.Animal)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                if (atendimento is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(atendimento);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem um Atendimento pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Atendimento</param>
        /// <returns>Objeto Atendimento</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> GetAtendimentosCliente(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = await _database.Atendimentos
                                                        .Include(atend => atend.Clinica)
                                                        .Include(atend => atend.Veterinario)
                                                        .Include(atend => atend.Tutor)
                                                        .Include(atend => atend.Animal)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(atend => atend.Id == id);

                if (atendimento is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(atendimento);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem uma relação de Atendimentos pelo Id do Veterinário
        /// </summary>
        /// <param name="id">Codigo do Veterinario</param>
        /// <returns>Objeto Atendimento</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpGet("Veterinario/{id}")]
        public async Task<IActionResult> GetAtendimentosVeterinario(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = await _database.Atendimentos
                                                        .Where(atend => atend.VeterinarioId == id)
                                                        .Include(atend => atend.Clinica)
                                                        .Include(atend => atend.Veterinario)
                                                        .Include(atend => atend.Tutor)
                                                        .Include(atend => atend.Animal)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                if (atendimento is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(atendimento);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem uma relação de Atendimentos pelo Id do Tutor
        /// </summary>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>Objeto Atendimento</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario,Cliente")]
        [HttpGet("Tutor/{id}")]
        public async Task<IActionResult> GetAtendimentosTutor(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = await _database.Atendimentos
                                                        .Where(atend => atend.TutorId == id)
                                                        .Include(atend => atend.Clinica)
                                                        .Include(atend => atend.Veterinario)
                                                        .Include(atend => atend.Tutor)
                                                        .Include(atend => atend.Animal)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                if (atendimento is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(atendimento);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem uma relação de Atendimentos pelo Id do Animal
        /// </summary>
        /// <param name="id">Codigo do Animal</param>
        /// <returns>Objeto Atendimento</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario,Cliente")]
        [HttpGet("Animal/{id}")]
        public async Task<IActionResult> GetAtendimentosAnimal(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = await _database.Atendimentos
                                                        .Where(atend => atend.AnimalId == id)
                                                        .Include(atend => atend.Clinica)
                                                        .Include(atend => atend.Veterinario)
                                                        .Include(atend => atend.Tutor)
                                                        .Include(atend => atend.Animal)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                if (atendimento is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(atendimento);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Atualiza parcialmente dados do Atendimento
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PATCH api/v1/Atendimentos/Atualizar
        ///     {
        ///         "diagnostico": "Estava saudável, mas apresentou problemas de saúde no fim da consulta.",
        ///     }
        /// </remarks>
        /// <param name="atendimentoDTO">Objeto Atendimento</param>
        /// <param name="id">Codigo do Atendimento</param>
        /// <returns>O objeto Atendimento atualizado</returns>
        /// <remarks>Retorna um objeto Atendimento atualizado</remarks>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpPatch("Atualizar/{id}")]
        public IActionResult Patch(AtualizarAtendimentoDTO atendimentoDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = _database.Atendimentos.FirstOrDefault(atendimento => atendimento.Id == id);

                if (atendimento is null) return NotFound(new { msg = "Registro não encontrado" });

                atendimento.DadosDoDia = atendimentoDTO.DadosDoDia ?? atendimento.DadosDoDia;
                atendimento.Diagnostico = atendimentoDTO.Diagnostico ?? atendimento.Diagnostico;
                atendimento.Comentarios = atendimentoDTO.Comentarios ?? atendimento.Comentarios;

                _database.SaveChanges();

                var atendimentoDb = _database.Atendimentos
                                                          .Include(atend => atend.Clinica)
                                                          .Include(atend => atend.Veterinario)
                                                          .Include(atend => atend.Tutor)
                                                          .Include(atend => atend.Animal)
                                                          .AsNoTracking()
                                                          .FirstOrDefault(atend => atend.Id == id);

                return Ok(new { msg = "Dados atualizados com sucesso:", atendimentoDb });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }


        /// <summary>
        /// Finaliza um Atendimento pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Atendimento</param>
        /// <returns>Objeto Atendimento</returns>

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
        [HttpDelete("Finalizar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var atendimento = _database.Atendimentos.FirstOrDefault(atend => atend.Id == id);
                if (atendimento is null) return NotFound(new { msg = "Registro não encontrado" });
                if (atendimento.StatusAtendimento == false) return BadRequest(new { msg = "O atendimento em questão já foi finalizado!" });

                atendimento.StatusAtendimento = false;
                _database.SaveChanges();

                var atendimentoDb = _database.Atendimentos
                                                          .Include(atend => atend.Clinica)
                                                          .Include(atend => atend.Veterinario)
                                                          .Include(atend => atend.Tutor)
                                                          .Include(atend => atend.Animal)
                                                          .AsNoTracking()
                                                          .FirstOrDefault(atend => atend.Id == id);

                return Ok(new { msg = "Consula finalizada, até a próxima!:", atendimentoDb });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

    }
}