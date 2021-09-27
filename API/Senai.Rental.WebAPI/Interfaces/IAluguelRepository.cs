using Senai.Rental.WebAPI.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Rental.WebAPI.Interfaces
{
    interface IAluguelRepository
    {
        List<AluguelDomain> ListarTodos();
        AluguelDomain BuscarPorId(int idAluguel);
        void Cadastrar(AluguelDomain novoAluguel);
        void AtualizarIdCorpo(AluguelDomain aluguelAtualizado);
        void Deletar(int idAluguel);
    }
}
