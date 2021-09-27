using Senai.Rental.WebAPI.Domains;
using Senai.Rental.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Rental.WebAPI.Repositories
{
    public class AluguelRepository : IAluguelRepository
    {
        private string stringConexao = "Data Source=LAPTOP-MIHFTFOJ\\SQLEXPRESS; initial catalog= T_Rental; user Id=sa; pwd=senai@132";
        public void AtualizarIdCorpo(AluguelDomain aluguelAtualizado)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryUpdateBody = "UPDATE ALUGUEL SET idVeiculo = @idVeiculo, idCliente = @idCliente, dataInicio = @dataInicio, dataFim = @dataFim WHERE idAluguel = @idAluguel";
                using (SqlCommand cmd = new SqlCommand(queryUpdateBody, con))
                {
                    cmd.Parameters.AddWithValue("@idVeiculo", aluguelAtualizado.idVeiculo);
                    cmd.Parameters.AddWithValue("@dataInicio", aluguelAtualizado.dataInicio);
                    cmd.Parameters.AddWithValue("@dataFim", aluguelAtualizado.dataFim);
                    cmd.Parameters.AddWithValue("@idCliente", aluguelAtualizado.idCliente);
                    cmd.Parameters.AddWithValue("@idAluguel", aluguelAtualizado.idAluguel);
                    con.Open();

                    cmd.ExecuteNonQuery();
                }

            }
        }

        public AluguelDomain BuscarPorId(int idAluguel)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string querySelectById = @"SELECT CL.nomeCliente AS Nome, CL.sobrenomeCliente AS Sobrenome, VL.placa AS Placa, MO.nomeModelo AS Modelo, MA.nomeMarca AS Marca, AL.dataInicio AS Inicio, AL.dataFim AS Fim FROM ALUGUEL AL
                  INNER JOIN CLIENTE CL 
                  ON AL.idCliente = CL.idCliente
                  INNER JOIN VEICULO VL
                  ON AL.idVeiculo = VL.idVeiculo
                  INNER JOIN MODELO MO
                  ON VL.idModelo = MO.idModelo
                  INNER JOIN MARCA MA
                  ON MO.idMarca = MA.idMarca
                  WHERE idAluguel = @idAluguel";
                con.Open();

                SqlDataReader rdr;

                using (SqlCommand cmd = new SqlCommand(querySelectById, con))
                {
                    cmd.Parameters.AddWithValue("@idAluguel", idAluguel);
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        AluguelDomain aluguelBuscado = new AluguelDomain
                        {
                            cliente = new ClienteDomain
                            {
                                nomeCliente = rdr["Nome"].ToString(),
                                sobrenomeCliente = rdr["Sobrenome"].ToString()
                            },

                            veiculo = new VeiculoDomain
                            {
                                placa = rdr["Placa"].ToString(),
                                modelo = new ModeloDomain
                                {
                                    nomeModelo = rdr["Modelo"].ToString(),
                                    marca = new MarcaDomain
                                    {
                                        nomeMarca = rdr["Marca"].ToString()
                                    }
                                }
                            },
                            dataInicio = Convert.ToDateTime(rdr["Inicio"]),
                            dataFim = Convert.ToDateTime(rdr["Fim"])
                        };
                        return aluguelBuscado;
                    }
                    return null;
                }
            }
        }

        public void Deletar(int idAluguel)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryDelete = "DELETE FROM ALUGUEL WHERE idAluguel = @idAluguel";

                using (SqlCommand cmd = new SqlCommand(queryDelete, con))
                {
                    cmd.Parameters.AddWithValue("@idAluguel", idAluguel);

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Cadastrar(AluguelDomain novoAluguel)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryInsert = "INSERT INTO ALUGUEL (idVeiculo, idCliente,dataInicio,dataFim) VALUES (@idCliente,@idVeiculo,@dataInicio,@dataFim)";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(queryInsert, con))
                {
                    cmd.Parameters.AddWithValue("@idVeiculo", novoAluguel.idVeiculo);
                    cmd.Parameters.AddWithValue("@idCliente", novoAluguel.idCliente);
                    cmd.Parameters.AddWithValue("@dataInicio", novoAluguel.dataInicio);
                    cmd.Parameters.AddWithValue("@dataFim", novoAluguel.dataFim);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<AluguelDomain> ListarTodos()
        {
            List<AluguelDomain> listaAlugueis = new List<AluguelDomain>();

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string querySelectAll = @"SELECT CL.nomeCliente AS Nome, CL.sobrenomeCliente AS Sobrenome, VL.placa AS Placa, MO.nomeModelo AS Modelo, MA.nomeMarca AS Marca, AL.dataInicio AS Inicio, AL.dataFim AS Fim FROM ALUGUEL AL
                  INNER JOIN CLIENTE CL 
                  ON AL.idCliente = CL.idCliente
                  INNER JOIN VEICULO VL
                  ON AL.idVeiculo = VL.idVeiculo
                  INNER JOIN MODELO MO
                  ON VL.idModelo = MO.idModelo
                  INNER JOIN MARCA MA
                  on MO.idMarca = MA.idMarca";
                con.Open();
                SqlDataReader rdr;

                using (SqlCommand cmd = new SqlCommand(querySelectAll, con))
                {
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        AluguelDomain aluguel = new AluguelDomain
                        {
                            cliente = new ClienteDomain
                            {
                                nomeCliente = rdr["Nome"].ToString(),
                                sobrenomeCliente = rdr["Sobrenome"].ToString()
                            },

                            veiculo = new VeiculoDomain
                            {
                                placa = rdr["Placa"].ToString(),
                                modelo = new ModeloDomain
                                {
                                    nomeModelo = rdr["Modelo"].ToString(),
                                    marca = new MarcaDomain
                                    {
                                        nomeMarca = rdr["Marca"].ToString()
                                    }
                                }
                            },
                            dataInicio = Convert.ToDateTime(rdr["Inicio"]),
                            dataFim = Convert.ToDateTime(rdr["Fim"])
                        };

                        listaAlugueis.Add(aluguel);
                    }
                }
            }
            return listaAlugueis;
        }
    }
}
