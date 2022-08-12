using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Tutor;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Funcionario,Cliente")]
    public class TutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public TutoresController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        /// <summary>
        /// Inclui um novo Tutor
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Tutores/Criar
        ///     {
        ///          "nome": "Breno",
        ///          "cpf": "4642524400",
        ///     }
        /// </remarks>
        /// <param name="tutorDTO">Objeto Tutor</param>
        /// <returns>O objeto Tutor incluido</returns>
        /// <remarks>Retorna um objeto Tutor incluído</remarks>

        [HttpPost("Criar")]
        public IActionResult Post(CriarTutorDTO tutorDTO)
        {
            try
            {
                if (tutorDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                var tutorCpf = _database.Tutores.FirstOrDefault(tutor => tutor.Cpf == tutorDTO.Cpf);
                if (tutorCpf is not null) return BadRequest(new { msg = "cpf informado já cadastrado" });

                Tutor tutor = _mapper.Map<Tutor>(tutorDTO);

                _database.Tutores.Add(tutor);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", tutor });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Exibe uma relação de todos os Tutores
        /// </summary>
        /// <returns>Retorna uma lista de objeto Tutor</returns>

        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var tutor = await _database.Tutores
                                                .Where(tutor => tutor.IsCliente == true)
                                                .Include(tutor => tutor.Animal)
                                                .AsNoTracking()
                                                .ToListAsync();

                if (tutor is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(tutor);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Obtem um Tutor pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>Objeto Tutor</returns>

        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var tutor = await _database.Tutores
                                                .AsNoTracking()
                                                .Include(tutor => tutor.Animal)
                                                .FirstOrDefaultAsync(tutor => tutor.Id == id);


                if (tutor is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(tutor);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Atualiza todos os dados do Tutor
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT api/v1/Tutores/Atualizar
        ///     {
        ///          "nome": "Breno Atualizado",
        ///          "cpf": "2222222222"
        ///     }
        /// </remarks>
        /// <param name="tutorDTO">Objeto Tutor</param>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>O objeto Tutor atualizado</returns>
        /// <remarks>Retorna um objeto Tutor atualizado</remarks>

        [HttpPut("Atualizar/{id}")]
        public IActionResult Put(AtualizarTutorDTO tutorDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var tutor = _database.Tutores.FirstOrDefault(tutor => tutor.Id == id);
                if (tutor is null) return NotFound(new { msg = "Registro não encontrado" });

                tutor = _mapper.Map(tutorDTO, tutor);

                var tutorCpf = _database.Tutores.FirstOrDefault(tut => tut.Cpf == tutor.Cpf && tut.Id != tutor.Id);
                if (tutorCpf is not null) return BadRequest(new { msg = "cpf informado já cadastrado" });

                _database.SaveChanges();
                return Ok(new { msg = "Dados atualizados com sucesso:", tutor });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }

        }

        /// <summary>
        /// Atualiza parcialmente dados do Tutor
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PUT api/v1/Tutores/Editar
        ///     {
        ///          "nome": "Breno Atualizado"
        ///     }
        /// </remarks>
        /// <param name="tutorDTO">Objeto Tutor</param>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>O objeto Tutor atualizado</returns>
        /// <remarks>Retorna um objeto Tutor atualizado</remarks>

        [HttpPatch("Editar/{id}")]
        public IActionResult Patch(AtualizarTutorDTO tutorDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var tutor = _database.Tutores.FirstOrDefault(tutor => tutor.Id == id);
                if (tutor is null) return NotFound(new { msg = "Registro não encontrado" });

                tutor.Nome = tutorDTO.Nome ?? tutor.Nome;
                tutor.Cpf = tutorDTO.Cpf ?? tutor.Cpf;

                var tutorCpf = _database.Tutores.FirstOrDefault(tut => tut.Cpf == tutor.Cpf && tut.Id != tutor.Id);
                if (tutorCpf is not null) return BadRequest(new { msg = "cpf informado já cadastrado" });

                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", tutor });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Inativa um Tutor pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Tutor</param>
        /// <returns>Objeto Tutor</returns>

        [HttpDelete("Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var tutor = _database.Tutores.FirstOrDefault(tutor => tutor.Id == id);
                var atendimentos = _database.Atendimentos.FirstOrDefault(atend => atend.TutorId == id && atend.StatusAtendimento == true);

                if (tutor is null) return NotFound(new { msg = "Registro não encontrado" });
                if (atendimentos is not null) return BadRequest(new { msg = "Este tutor possui atendimentos pendentes, impossivel deletar!" });
                if (tutor.IsCliente == false) return BadRequest(new { msg = "O cliente em questão já foi inativado!" });

                tutor.IsCliente = false;
                _database.SaveChanges();

                return Ok(new { msg = "Registro deletado:", tutor });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }
    }
}