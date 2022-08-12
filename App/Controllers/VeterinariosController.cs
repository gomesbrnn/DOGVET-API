using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Veterinario;
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
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario")]
    public class VeterinariosController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public VeterinariosController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }


        /// <summary>
        /// Inclui um novo Veterinario
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Veterinarios/Criar
        ///     {
        ///          "nome": "Piton",
        ///          "numeroRegistro": "23436588032"
        ///     }
        /// </remarks>
        /// <param name="veterinarioDTO">Objeto Veterinario</param>
        /// <returns>O objeto Veterinario incluido</returns>
        /// <remarks>Retorna um objeto Veterinario incluído</remarks>

        [HttpPost("Criar")]
        public IActionResult Post(CriarVeterinarioDTO veterinarioDTO)
        {
            try
            {
                var vetRegistro = _database.Veterinarios.FirstOrDefault(vet => vet.numeroRegistro == veterinarioDTO.numeroRegistro);
                if (vetRegistro is not null) return BadRequest(new { msg = "numero de registro informado já existente" });

                if (veterinarioDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });
                Veterinario veterinario = _mapper.Map<Veterinario>(veterinarioDTO);

                _database.Veterinarios.Add(veterinario);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", veterinario });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }


        /// <summary>
        /// Exibe uma relação de todos os Veterinarios
        /// </summary>
        /// <returns>Retorna uma lista de objeto Veterinarios</returns>

        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var veterinario = await _database.Veterinarios
                                                        .Where(veterinario => veterinario.IsFuncionario == true)
                                                        .AsNoTracking()
                                                        .ToListAsync();

                if (veterinario is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(veterinario);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Obtem um Veterinario pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Veterinario</param>
        /// <returns>Objeto Veterinario</returns>

        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var veterinario = await _database.Veterinarios
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(veterinario => veterinario.Id == id);


                if (veterinario is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(veterinario);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Atualiza todos os dados do Veterinario
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT api/v1/Veterinarios/Atualizar
        ///     {
        ///          "nome": "Piton Atualizado",
        ///          "numeroRegistro": "00000000"
        ///     }
        /// </remarks>
        /// <param name="veterinarioDTO">Objeto Veterinario</param>
        /// <param name="id">Codigo do Veterinario</param>
        /// <returns>O objeto Veterinario atualizado</returns>
        /// <remarks>Retorna um objeto Veterinario atualizado</remarks>

        [HttpPut("Atualizar/{id}")]
        public IActionResult Put(AtualizarVeterinarioDTO veterinarioDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var veterinario = _database.Veterinarios.FirstOrDefault(veterinario => veterinario.Id == id);
                if (veterinario is null) return NotFound(new { msg = "Registro não encontrado" });

                veterinario = _mapper.Map(veterinarioDTO, veterinario);

                var vetRegistro = _database.Veterinarios.FirstOrDefault(vet => vet.numeroRegistro == veterinario.numeroRegistro && vet.Id != veterinario.Id);
                if (vetRegistro is not null) return BadRequest(new { msg = "cpf informado já cadastrado" });

                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", veterinario });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Desliga um Veterinario pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Veterinario</param>
        /// <returns>Objeto Veterinario</returns>

        [HttpDelete("Desligar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var veterinario = _database.Veterinarios.FirstOrDefault(veterinario => veterinario.Id == id);
                var atendimentos = _database.Atendimentos.FirstOrDefault(atend => atend.VeterinarioId == id && atend.StatusAtendimento == true);

                if (veterinario is null) return NotFound(new { msg = "Registro não encontrado" });
                if (atendimentos is not null) return BadRequest(new { msg = "Este veterinario possui atendimentos pendentes, impossivel desliga-lo!" });
                if (veterinario.IsFuncionario == false) return BadRequest(new { msg = "O veterinário em questão já foi desligado!" });

                veterinario.IsFuncionario = false;
                _database.SaveChanges();

                return Ok(new { msg = "Registro deletado:", veterinario });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }
    }
}