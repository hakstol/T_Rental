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
    public class ClientesController : Controller
    {
        private IClienteRepository _clienteRepository { get; set; }
        public ClientesController()
        {
            _clienteRepository = new ClienteRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<ClienteDomain> listaClientes = _clienteRepository.ListarTodos();

            return Ok(listaClientes);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            ClienteDomain clienteBuscado = _clienteRepository.BuscarPorId(id);

            if (clienteBuscado == null)
            {
                return NotFound("Nenhum Cliente Encontrado");
            }

            return Ok(clienteBuscado);
        }

        [HttpPost]
        public IActionResult Post(ClienteDomain novoCliente)
        {
            _clienteRepository.Cadastrar(novoCliente);

            return StatusCode(201);
        }

        [HttpPut]
        public IActionResult PutIdBody(ClienteDomain clienteAtualizado)
        {
            ClienteDomain clienteBuscado = _clienteRepository.BuscarPorId(clienteAtualizado.idCliente);

            if (clienteBuscado != null)
            {
                try
                {
                    if (clienteAtualizado.nomeCliente == null)
                    {
                        clienteAtualizado.nomeCliente = clienteBuscado.nomeCliente;
                    }
                    if (clienteAtualizado.sobrenomeCliente == null)
                    {
                        clienteAtualizado.sobrenomeCliente = clienteBuscado.sobrenomeCliente;
                    }
                    if (clienteAtualizado.cnhCliente == null)
                    {
                        clienteAtualizado.cnhCliente = clienteBuscado.cnhCliente;
                    }

                    _clienteRepository.AtualizarIdCorpo(clienteAtualizado);

                    return NoContent();
                }
                catch (Exception)
                {

                    return BadRequest();
                }
            }

            return NotFound(
                new
                {
                    mensagem = "Cliente não encontrado",
                    erro = true
                }
             );
        }

        [HttpDelete]
        public IActionResult Delete(ClienteDomain clienteDeletado)
        {
            _clienteRepository.Deletar(clienteDeletado.idCliente);

            return NoContent();
        }

    }
}
