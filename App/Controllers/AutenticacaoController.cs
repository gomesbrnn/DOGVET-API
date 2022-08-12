using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.DTO.Autenticacao;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IMapper _mapper;

        public AutenticacaoController(ApplicationDbContext database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra um novo Usuario
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Autenticacao/Registro
        ///     {
        ///          "email": "developer@gft.com",
        ///          "senha": "developer"
        ///     }
        /// </remarks>
        /// <param name="usuarioDTO">Objeto Autenticacao</param>
        /// <param name="IsFuncionario">O usuário é um funcionário?</param>
        /// <returns>O objeto Autenticacao incluido</returns>
        /// <remarks>Retorna um objeto Autenticacao incluído</remarks>

        [HttpPost("Registro")]
        public IActionResult Post(CriarAutenticacaoDTO usuarioDTO, bool IsFuncionario)
        {
            try
            {
                if (usuarioDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                var usuarioDb = _database.Autenticacoes.FirstOrDefault(user => user.Email == usuarioDTO.Email);
                if (usuarioDb != null) return BadRequest(new { msg = "O E-mail informado já se encontra em uso." });

                Autenticacao usuario = usuario = _mapper.Map<Autenticacao>(usuarioDTO);
                usuario.IsFuncionario = IsFuncionario;

                _database.Autenticacoes.Add(usuario);
                _database.SaveChanges();

                return Ok(new { msg = "Registro criado:", usuario });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Realiza o login do usuario
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     POST api/v1/Autenticacao/Login
        ///     {
        ///          "email": "developer@gft.com",
        ///          "senha": "developer"
        ///     }
        /// </remarks>
        /// <param name="usuarioDTO">Objeto Autenticacao</param>
        /// <returns>O objeto Autenticacao incluido</returns>
        /// <remarks>Retorna um objeto Autenticacao incluído</remarks>

        [HttpPost("Login")]
        public IActionResult PostAutenticacoesLogin(CriarAutenticacaoDTO usuarioDTO)
        {
            try
            {
                if (usuarioDTO is null) return NotFound(new { msg = "Dados incompletos ou invalidos" });

                Autenticacao usuario = _database.Autenticacoes.FirstOrDefault(
                   user => user.Email.Equals(usuarioDTO.Email));

                if (usuario is null || usuario.Senha != usuarioDTO.Senha) return Unauthorized();

                _mapper.Map(usuario, usuarioDTO);
                List<Claim> claims = new();

                if (usuarioDTO.IsFuncionario == true)
                {
                    claims.Add(new Claim("Email", usuarioDTO.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Funcionario"));
                }
                else
                {
                    claims.Add(new Claim("Email", usuarioDTO.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
                }

                string chaveDeSeguranca = "senha_mais_bala_do_oeste";
                var chaveSimetrica = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveDeSeguranca));
                var credencialDeAcesso = new SigningCredentials(chaveSimetrica, SecurityAlgorithms.HmacSha256Signature);

                var JWT = new JwtSecurityToken(
                    issuer: "dogvetapi.com",
                    expires: DateTime.Now.AddHours(1),
                    audience: "usuario_comun",
                    signingCredentials: credencialDeAcesso,
                    claims: claims
                );

                return Ok(new { msg = "Login realizado com sucesso, utilize seu token para autenticação:", token = new JwtSecurityTokenHandler().WriteToken(JWT) });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Exibe uma relação de todos os Usuarios
        /// </summary>
        /// <returns>Retorna uma lista de objeto Autenticacao</returns>

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Listar")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _database.Autenticacoes
                                                .Where(user => user.Status == true)
                                                .AsNoTracking()
                                                .ToListAsync();

                if (user is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Obtem um Usuario pelo seu Id
        /// </summary>
        /// <param name="id">Codigo da Usuario</param>
        /// <returns>Objeto Autenticacao</returns>

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Listar/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var user = await _database.Autenticacoes
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(user => user.Id == id);


                if (user is null) return NotFound(new { msg = "Registros não encontrados" });

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Atualiza parcialmente dados do Usuário
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        ///
        ///     PATCH api/v1/Autenticacao/Atualizar
        ///     {
        ///          "senha": "developerAtualizado"
        ///     }
        /// </remarks>
        /// <param name="autenticacaoDTO">Objeto Autenticacao</param>
        /// <param name="id">Codigo do Usuario</param>
        /// <returns>O objeto Autenticacao atualizado</returns>
        /// <remarks>Retorna um objeto Autenticacao atualizado</remarks>

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("Atualizar/{id}")]
        public IActionResult Patch(AtualizarAutenticacaoDTO autenticacaoDTO, int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var autenticacao = _database.Autenticacoes.FirstOrDefault(user => user.Id == id);
                if (autenticacao is null) return NotFound(new { msg = "Registro não encontrado" });

                autenticacao.Email = autenticacaoDTO.Email ?? autenticacao.Email;
                autenticacao.Senha = autenticacaoDTO.Senha ?? autenticacao.Senha;

                var autenticacaoDb = _database.Autenticacoes.FirstOrDefault(aut => aut.Email == autenticacao.Email && aut.Id != id);
                if (autenticacaoDb is not null) return BadRequest(new { msg = "O E-mail informado já se encontra em uso." });

                _database.SaveChanges();

                return Ok(new { msg = "Dados atualizados com sucesso:", autenticacao });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

        /// <summary>
        /// Inativa um Usuario pelo seu Id
        /// </summary>
        /// <param name="id">Codigo do Usuario</param>
        /// <returns>Objeto Autenticacao</returns>

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("Deletar/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) return NotFound(new { msg = "Id invalido!" });

                var autenticacao = _database.Autenticacoes.FirstOrDefault(user => user.Id == id);
                if (autenticacao is null) return NotFound(new { msg = "Registro não encontrado" });
                if (autenticacao.Status == false) return BadRequest(new { msg = "O usuario em questão já foi inativada!" });

                autenticacao.Status = false;
                _database.SaveChanges();

                return Ok(new { msg = "Registro deletado:", autenticacao });
            }
            catch (Exception)
            {
                return StatusCode(500, new { msg = "Não foi possivel tratar sua solicitação" });
            }
        }

    }
}