using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Globalization;
using System.IO;

public class BancoDeDados // Classe do Meu BD
{
    private IDbConnection BancoDados;

    private string GetDatabasePath()
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Database");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        return Path.Combine(folderPath, "BancoDeDados.Sqlite");
    }

    private IDbConnection criarEAbrirBancoDeDados()
    {
        string dbPath = GetDatabasePath();
        string idburi = $"URI=file:{dbPath}"; // Vari�vel que armazena a localiza��o do banco de dados dentro da pasta do projeto
        IDbConnection ConexaoBanco = new SqliteConnection(idburi);

        ConexaoBanco.Open(); // ".Open()" � o comando do Sqlite que inicializa/abre o banco de dados.

        using (var cmdCriarTB = ConexaoBanco.CreateCommand()) // ".CreateCommand()" utilizado para criar os objetos do banco
        {
            cmdCriarTB.CommandText = "CREATE TABLE IF NOT EXISTS POSICOES(" +
                "id INTEGER PRIMARY KEY NOT NULL, " +
                "x REAL, " +
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
        ComandoLerPosicao.CommandText = $"SELECT * FROM POSICOES WHERE id={id};";
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
        DeletarTudo.CommandText = "DELETE FROM POSICOES"; // Alerta de perigo 
        DeletarTudo.ExecuteNonQuery();
        BancoDados.Close();
    }
}
