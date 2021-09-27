using Senai.Rental.WebAPI.Domains;
using Senai.Rental.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Rental.WebAPI.Repositories
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private string stringConexao = "Data Source=LAPTOP-MIHFTFOJ\\SQLEXPRESS; initial catalog= T_Rental; user Id=sa; pwd=senai@132";
        public void AtualizarIdCorpo(VeiculoDomain veiculoAtualizado)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryUpdate = "UPDATE VEICULO SET idModelo = @idModelo, idEmpresa = @idEmpresa, placa = @placa WHERE idVeiculo = @idVeiculo";

                using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                {
                    cmd.Parameters.AddWithValue("@idModelo", veiculoAtualizado.idModelo);
                    cmd.Parameters.AddWithValue("@idEmpresa", veiculoAtualizado.idEmpresa);
                    cmd.Parameters.AddWithValue("@placa", veiculoAtualizado.placa);
                    cmd.Parameters.AddWithValue("@idVeiculo", veiculoAtualizado.idVeiculo);

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public VeiculoDomain BuscarPorId(int idVeiculo)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string querySelectAll = "SELECT idVeiculo AS ID, nomeEmpresa AS Empresa, E.idEmpresa , nomeModelo AS Modelo, MO.idModelo, nomeMarca AS Marca, MA.idMarca, placa AS Placa FROM VEICULO V LEFT JOIN EMPRESA E ON V.idEmpresa = E.idEmpresa LEFT JOIN MODELO MO ON V.idModelo = MO.idModelo LEFT JOIN MARCA MA ON MO.idMarca = MA.idMarca WHERE idVeiculo = @idVeiculo";

                con.Open();

                SqlDataReader rdr;

                using (SqlCommand cmd = new SqlCommand(querySelectAll, con))
                {
                    cmd.Parameters.AddWithValue("@idVeiculo", idVeiculo);

                    rdr = cmd.ExecuteReader();

                    if(rdr.Read())
                    {
                        VeiculoDomain veiculoBuscado = new VeiculoDomain()
                        {
                            idVeiculo = Convert.ToInt32(rdr[0]),

                            empresa = new EmpresaDomain
                            {
                                nomeEmpresa = rdr["Empresa"].ToString(),
                                idEmpresa = Convert.ToInt32(rdr[2])
                            },

                            modelo = new ModeloDomain
                            {
                                nomeModelo = rdr["Modelo"].ToString(),
                                idModelo = Convert.ToInt32(rdr[4]),

                                marca = new MarcaDomain
                                {
                                    nomeMarca = rdr["Marca"].ToString(),
                                    idMarca = Convert.ToInt32(rdr[6])
                                }
                            },

                            placa = rdr["Placa"].ToString()
                        };

                        return veiculoBuscado;
                    };

                    return null;
                }

            };
        }

        public void Cadastrar(VeiculoDomain novoVeiculo)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryInsert = "INSERT INTO VEICULO (idModelo, idEmpresa, placa) VALUES (@idModelo, @idEmpresa, @placa)";

                con.Open();

                using (SqlCommand cmd = new SqlCommand(queryInsert, con))
                {
                    cmd.Parameters.AddWithValue("@idModelo", novoVeiculo.idModelo);
                    cmd.Parameters.AddWithValue("@idEmpresa", novoVeiculo.idEmpresa);
                    cmd.Parameters.AddWithValue("@placa", novoVeiculo.placa);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Deletar(int idVeiculo)
        {
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string queryDelete = "DELETE FROM VEICULO WHERE idVeiculo = @idVeiculo";

                using (SqlCommand cmd = new SqlCommand(queryDelete, con))
                {
                    cmd.Parameters.AddWithValue("@idVeiculo", idVeiculo);

                    con.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<VeiculoDomain> ListarTodos()
        {
            List<VeiculoDomain> listaVeiculos = new List<VeiculoDomain>();

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                string querySelectAll = "SELECT idVeiculo AS Id, nomeEmpresa AS Empresa, nomeModelo AS Modelo, nomeMarca AS Marca, placa AS Placa FROM VEICULO V LEFT JOIN EMPRESA E ON V.idEmpresa = E.idEmpresa LEFT JOIN MODELO MO ON V.idModelo = MO.idModelo LEFT JOIN MARCA MA ON MO.idMarca = MA.idMarca";

                con.Open();

                SqlDataReader rdr;

                using (SqlCommand cmd = new SqlCommand(querySelectAll, con))
                {
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        VeiculoDomain veiculo = new VeiculoDomain()
                        {
                            idVeiculo = Convert.ToInt32(rdr[0]),

                            empresa = new EmpresaDomain
                            {
                                nomeEmpresa = rdr["Empresa"].ToString(),
                            },

                            modelo = new ModeloDomain
                            {
                                nomeModelo = rdr["Modelo"].ToString(),

                                marca = new MarcaDomain
                                {
                                    nomeMarca = rdr["Marca"].ToString()
                                }
                            },

                            placa = rdr["Placa"].ToString()
                        };

                        listaVeiculos.Add(veiculo);
                    }
                }

            };

            return listaVeiculos;
        }
    }
}
