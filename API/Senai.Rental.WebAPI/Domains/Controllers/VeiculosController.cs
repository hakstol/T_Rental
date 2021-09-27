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
    public class VeiculosController : Controller
    {
        private IVeiculoRepository _veiculoRepository { get; set; }
        public VeiculosController()
        {
            _veiculoRepository = new VeiculoRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<VeiculoDomain> listaVeiculos = _veiculoRepository.ListarTodos();

            return Ok(listaVeiculos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            VeiculoDomain veiculoBuscado = _veiculoRepository.BuscarPorId(id);

            if (veiculoBuscado == null)
            {
                return NotFound("Nenhum Veiculo Encontrado");
            }

            return Ok(veiculoBuscado);
        }

        [HttpPost]
        public IActionResult Post(VeiculoDomain novoVeiculo)
        {
            _veiculoRepository.Cadastrar(novoVeiculo);

            return StatusCode(201);
        }

        [HttpPut]
        public IActionResult PutById(VeiculoDomain veiculoAtualizado)
        {
            VeiculoDomain veiculoBuscado = _veiculoRepository.BuscarPorId(veiculoAtualizado.idVeiculo);

            if (veiculoBuscado != null)
            {
                try
                {
                    if (veiculoAtualizado.idModelo <= 0)
                    {
                        veiculoAtualizado.idModelo = veiculoBuscado.idModelo;
                    }
                    if (veiculoAtualizado.idEmpresa <= 0)
                    {
                        veiculoAtualizado.idEmpresa = veiculoBuscado.idEmpresa;
                    }
                    if (veiculoAtualizado.placa == null)
                    {
                        veiculoAtualizado.placa = veiculoBuscado.placa;
                    }

                    _veiculoRepository.AtualizarIdCorpo(veiculoAtualizado);

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
                    mensagem = "Veiculo não encontrado",
                    erro = true
                }
             );
        }

        [HttpDelete]
        public IActionResult Delete(VeiculoDomain veiculoDeletado)
        {
            _veiculoRepository.Deletar(veiculoDeletado.idVeiculo);

            return NoContent();
        }

    }
}
