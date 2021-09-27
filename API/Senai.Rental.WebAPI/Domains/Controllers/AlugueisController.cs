using Microsoft.AspNetCore.Mvc;
using Senai.Rental.WebAPI.Domains;
using Senai.Rental.WebAPI.Interfaces;
using Senai.Rental.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Rental.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlugueisController : ControllerBase
    {
        private IAluguelRepository _aluguelRepository { get; set; }
        public AlugueisController()
        {
            _aluguelRepository = new AluguelRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<AluguelDomain> listaAluguel = _aluguelRepository.ListarTodos();

            return Ok(listaAluguel);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            AluguelDomain aluguelBuscado = _aluguelRepository.BuscarPorId(id);

            if (aluguelBuscado == null)
            {
                return NotFound("Nenhum aluguel foi encontrado!");
            }

            return Ok(aluguelBuscado);
        }

        [HttpPost]
        public IActionResult Post(AluguelDomain novoAluguel)
        {

            _aluguelRepository.Cadastrar(novoAluguel);

            return StatusCode(201);
        }

        [HttpDelete("excluir/{id}")]
        public IActionResult Delete(int id)
        {
            _aluguelRepository.Deletar(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult PutIdBody(AluguelDomain aluguelAtualizado)
        {
            if (aluguelAtualizado.idAluguel <= 0 || aluguelAtualizado.idCliente <= 0 || aluguelAtualizado.idVeiculo <= 0)
            {
                return BadRequest(
                    new
                    {
                        mensagemErro = "Algum atributo não foi informado!"
                    });
            }

            AluguelDomain aluguelBuscado = _aluguelRepository.BuscarPorId(aluguelAtualizado.idAluguel);

            if (aluguelBuscado != null)
            {
                try
                {
                    _aluguelRepository.AtualizarIdCorpo(aluguelAtualizado);

                    return NoContent();
                }
                catch (Exception codErro)
                {
                    return BadRequest(codErro);
                }
            }

            return NotFound(
                    new
                    {
                        mensagem = "Aluguel não encontrado!",
                        errorStatus = true
                    }
                );
        }
    }
}
