using System.Collections;
using System.Data.SQLite;
class Program
{
    static string path = @"C:\Users\Pozzame\Documents\Corso_2024\Database\database.db";
    static void Main(string[] args)
    {

        if (!File.Exists(path))
        {
            SQLiteConnection.CreateFile(path);
            SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
            connection.Open();
            string sql = @"CREATE TABLE prodotti 
            (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                nome TEXT UNIQUE, 
                prezzo real not null, 
                quantita integer check (quantita >=0), 
                stato bolean, 
                scadenza date, 
                id_categoria integer, 
                foreign key (id_categoria) 
                    references categorie (id)
            );
            CREATE TABLE categorie 
            (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                nome TEXT UNIQUE,
                descrizione text
            );
            INSERT INTO categorie (nome, descrizione) 
                VALUES ('Carne', 'La carne');
            INSERT INTO categorie (nome, descrizione) 
                VALUES ('Pesce', 'Il pesce');
            INSERT INTO categorie (nome, descrizione) 
                VALUES ('Formaggi', 'Il pesce');
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Simmental', 10, 100, true, '2026-12-12', 1);
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Riomare', 20, 200, true, '2025-12-12', 2);
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Manzotin', 10, 100, true, '2026-12-12', 1);
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Babybel', 5, 500, true, '2025-12-12', 3);
            ";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        while (true)
        {
            Console.WriteLine("1 - Inserisci prodotto");
            Console.WriteLine("2 - Visualizzazioni prodotti");
            Console.WriteLine("3 - Elimina prodotto");
            Console.WriteLine("4 - Modifica prezzo prodotto");
            Console.WriteLine("5 - Visualitta Categorie");
            Console.WriteLine("6 - Esci");
            Console.WriteLine("Scegli un'opzione");
            int scelta = Convert.ToInt32(Console.ReadLine());
            switch (scelta)
            {
                case 1:
                    InserisciProdotto();
                    break;
                case 2:
                    Console.WriteLine("1 - Visualizza tutto");
                    Console.WriteLine("2 - Ordina per prezzo");
                    Console.WriteLine("3 - Ordina per quantita");
                    Console.WriteLine("4 - Visualizza il più costoso");
                    Console.WriteLine("5 - Visualizza il meno costoso");
                    Console.WriteLine("6 - Visualizza un prodotto");
                    scelta = Convert.ToInt32(Console.ReadLine());
                    VisualizzaProdotti(scelta);
                    break;
                case 3:
                    EliminaProdotto();
                    break;
                case 4:
                    ModificaPrezzo();
                    break;
                case 5:
                    VisualizzaCategorie();
                    break;
                default:
                    return;
            }
        }
    }

    static void ModificaPrezzo()
    {
        Console.WriteLine("Inserisci il nome del prodotto da modificare");
        string nome = Console.ReadLine()!;
        Console.WriteLine("Nuovo prezzo?");
        string prezzo = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = $"UPDATE prodotti SET prezzo = {prezzo} WHERE nome = '{nome}';";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    static void InserisciProdotto()
    {
        Console.WriteLine("Inserisci nome prodotto");
        string nome = Console.ReadLine()!;
        Console.WriteLine("Inserisci prezzo");
        string prezzo = Console.ReadLine()!;
        Console.WriteLine("Inserisci quantità");
        string quantita = Console.ReadLine()!;
        Console.WriteLine("Inserisci scadenza [YYYY-mm-dd]");
        string scadenza = Console.ReadLine()!;
        Console.WriteLine("Scegli categoria:");
        VisualizzaCategorie();
        string id_categoria = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = $"INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) VALUES ('{nome}',{prezzo}, {quantita}, true, '{scadenza}', {id_categoria})";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    static void VisualizzaCategorie()
    {
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = "SELECT * FROM categorie";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
            Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, descrizione: {reader["descrizione"]}");
        connection.Close();
    }

    static void VisualizzaProdotti(int scelta)
    {
        switch (scelta)
        {
            case 1:
                {
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = "SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
            case 2:
                {
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = "SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id ORDER BY prezzo";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
            case 3:
                {
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = "SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id ORDER BY quantita";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
            case 4:
                {
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = "SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id ORDER BY prezzo DESC LIMIT 1";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
            case 5:
                {
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = "SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id ORDER BY prezzo ASC LIMIT 1";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
            case 6:
                {
                    Console.WriteLine("Inserisci il nome del prodotto da visualizzare");
                    string nome = Console.ReadLine()!;
                    SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
                    connection.Open();
                    string sql = $"SELECT strftime('%d/%m/%Y', scadenza) AS scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id WHERE prodotti.nome = '{nome}'";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
                    connection.Close();
                    break;
                }
        }
    }
    static void EliminaProdotto()
    {
        Console.WriteLine("inserisci il nome del prodotto da eliminare");
        string nome = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = $"DELETE FROM prodotti WHERE nome = '{nome}'";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }
}
