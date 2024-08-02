using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;

public class BancoDeDados //Classe do Meu BD
{
    private IDbConnection BancoDados;

    private IDbConnection criarEAbrirBancoDeDados()
    {
        string idburi = "URI=file:BancoDeDados.Sqlite"; // variável que armazena a localização do banco de dados dentro da pasta do projeto
        IDbConnection ConexaoBanco = new SqliteConnection(idburi);
        // caso o banco já exista, a conexão será feita, se o banco não existir ele irá criar um novo.

        ConexaoBanco.Open(); // ".Open()" é o comando do Sqlite que inicializa/abre o banco de dados.

        using (var cmdCriarTB = ConexaoBanco.CreateCommand()) //".CreateCommand()" utilizado para criar os objetos do banco
        {
            // Deleta a tabela se ela já existir
            cmdCriarTB.CommandText = "DROP TABLE IF EXISTS POSICOES;";
            cmdCriarTB.ExecuteNonQuery();

            // Cria a tabela com a estrutura correta
            cmdCriarTB.CommandText = "CREATE TABLE IF NOT EXISTS POSICOES(" +
                "id INTEGER PRIMARY KEY NOT NULL, " +
                "x REAL, " +  //inserindo a tabela de posicoes no bd
                "y REAL, " +
                "z REAL" +
                ");";
            cmdCriarTB.ExecuteNonQuery();
        }

        return ConexaoBanco;
    }

    public void InserirPosicao(int id, float x, float y, float z)
    {
        CultureInfo cultura = CultureInfo.InvariantCulture;
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand InserDados = BancoDados.CreateCommand();
        InserDados.CommandText = $"INSERT OR REPLACE INTO POSICOES(id, x, y, z) " +
                                 $"VALUES({id}, {x.ToString(cultura)}, {y.ToString(cultura)}, {z.ToString(cultura)})";

        InserDados.ExecuteNonQuery();
        BancoDados.Close();
    }

    public IDataReader LerPosicao(int id)
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand ComandoLerPosicao = BancoDados.CreateCommand();
        ComandoLerPosicao.CommandText = $"SELECT * FROM POSICOES WHERE ID = {id};";
        IDataReader Leitura = ComandoLerPosicao.ExecuteReader();
        return Leitura;
    }

    public void FecharConexao()
    {
        BancoDados.Close();
    }

    public void CriarBanco()
    {
        BancoDados = criarEAbrirBancoDeDados();
        BancoDados.Close();
    }

    public void NovoJogo()
    {
        BancoDados = criarEAbrirBancoDeDados();
        IDbCommand DeletarTudo = BancoDados.CreateCommand();
        DeletarTudo.CommandText = "DELETE FROM POSICOES"; //Alerta de perigo 
        DeletarTudo.ExecuteNonQuery();
        BancoDados.Close();
    }
}
