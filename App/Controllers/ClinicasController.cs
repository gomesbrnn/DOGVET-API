using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Clinica;
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
    public class ClinicasController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public ClinicasController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        /// <summary>
        /// Inclui uma nova Clinica
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Clinicas/Criar
        ///     {
        ///          "nome": "Dog Vet",
        ///          "cnpj": "46425244000117",
        ///          "endereco": "Praça Camilo Pereira Carneiro, Curado - Recife"
        ///     }
        /// </remarks>
        /// <param name="clinicaDTO">Objeto Clinica</param>
        /// <returns>O objeto Clinica incluido</returns>
        /// <remarks>Retorna um objeto Clinica incluído</remarks>

        [HttpPost("Criar")]
        public IActionResult Post(CriarClinicaDTO clinicaDTO)
        {
            try
            {
                if (clinicaDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                Clinica clinica = _mapper.Map<Clinica>(clinicaDTO);

                _database.Clinicas.Add(clinica);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", clinica });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Exibe uma relação de todas as Clinicas
        /// </summary>
        /// <returns>Retorna uma lista de objeto Clinicas</returns>

        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clinica = await _database.Clinicas
                                                .Where(cli => cli.IsAtivo == true)
                                                .AsNoTracking()
                                                .ToListAsync();

                if (clinica is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(clinica);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Obtem um Clinica pelo seu Id
        /// </summary>
        /// <param name="id">Codigo da Clinica</param>
        /// <returns>Objeto Clinica</returns>

        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var clinica = await _database.Clinicas
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(cli => cli.Id == id);


                if (clinica is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(clinica);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Atualiza todos os dados da Clinica
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT api/v1/Clinicas/Atualizar
        ///     {
        ///          "nome": "Dog Vet",
        ///          "cnpj": "46425244000117",
        ///          "endereco": "Praça Camilo Pereira Carneiro, Curado - Recife"
        ///     }
        /// </remarks>
        /// <param name="clinicaDTO">Objeto Clinica</param>
        /// <param name="id">Codigo da Clinica</param>
        /// <returns>O objeto Clinica atualizado</returns>
        /// <remarks>Retorna um objeto Clinica atualizado</remarks>

        [HttpPut("Atualizar/{id}")]
        public IActionResult Put(AtualizarClinicaDTO clinicaDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var clinica = _database.Clinicas.FirstOrDefault(cli => cli.Id == id);

                if (clinica is null) return NotFound(new { msg = "Registro não encontrado" });

                clinica = _mapper.Map(clinicaDTO, clinica);
                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", clinica });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Atualiza parcialmente dados da Clinica
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PATCH api/v1/Clinicas/Editar
        ///     {
        ///          "endereco": "Praça Camilo Pereira Carneiro, Curado - Recife"
        ///     }
        /// </remarks>
        /// <param name="clinicaDTO">Objeto Clinica</param>
        /// <param name="id">Codigo da Clinica</param>
        /// <returns>O objeto Clinica atualizado</returns>
        /// <remarks>Retorna um objeto Clinica atualizado</remarks>

        [HttpPatch("Editar/{id}")]
        public IActionResult Patch(AtualizarClinicaDTO clinicaDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var clinica = _database.Clinicas.FirstOrDefault(cli => cli.Id == id);

                if (clinica is null) return NotFound(new { msg = "Registro não encontrado" });

                clinica.Nome = clinicaDTO.Nome ?? clinica.Nome;
                clinica.Cnpj = clinicaDTO.Cnpj ?? clinica.Cnpj;
                clinica.Endereco = clinicaDTO.Endereco ?? clinica.Endereco;

                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", clinica });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Inativa uma Clinica pelo seu Id
        /// </summary>
        /// <param name="id">Codigo da Clinica</param>
        /// <returns>Objeto Clinica</returns>

        [HttpDelete("Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var clinica = _database.Clinicas.FirstOrDefault(cli => cli.Id == id);
                var atendimentos = _database.Atendimentos.FirstOrDefault(atend => atend.ClinicaId == id && atend.StatusAtendimento == true);

                if (clinica is null) return NotFound(new { msg = "Registro não encontrado" });
                if (atendimentos is not null) return BadRequest(new { msg = "Esta clinica possui atendimentos pendentes, impossivel deletar!" });
                if (clinica.IsAtivo == false) return BadRequest(new { msg = "A clinica em questão já foi inativada!" });

                clinica.IsAtivo = false;
                _database.SaveChanges();

                return Ok(new { msg = "Registro deletado:", clinica });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }
    }
}