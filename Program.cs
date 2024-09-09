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
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Simmental', 10, 100, true, '2026-12-12', 1);
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Riomare', 20, 200, true, '2025-12-12', 2);
            INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
                VALUES ('Manzotin', 10, 100, true, '2026-12-12', 1);
            ";

            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        while (true)
        {
            Console.WriteLine("1 - Inserisci prodotto");
            Console.WriteLine("2 - Visualizza prodotti");
            Console.WriteLine("3 - Elimina prodotto");
            Console.WriteLine("4 - Esci");
            Console.WriteLine("Scegli un'opzione");
            string scelta = Console.ReadLine()!;
            if (scelta == "1")
                InserisciProdotto();
            else if (scelta == "2")
                VisualizzaProdotti();
            else if (scelta == "3")
                EliminaProdotto();
            else if (scelta == "4")
                break;
        }
    }

    static void InserisciProdotto()
    {
        Console.WriteLine("Inserisci nome prodotto");
        string nome = Console.ReadLine()!;
        Console.WriteLine("Inserisci prezzo");
        string prezzo = Console.ReadLine()!;
        Console.WriteLine("Inserisci quantità");
        string quantita = Console.ReadLine()!;
        Console.WriteLine("Inserisci scadenza");
        string scadenza = Console.ReadLine()!;
        Console.WriteLine("Inserisci id categoria");
        string id_categoria = Console.ReadLine()!;
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = $"INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) VALUES ('{nome}',{prezzo}, {quantita}, true, '{scadenza}', {id_categoria})";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.ExecuteNonQuery();
        connection.Close();
    }

    static void VisualizzaProdotti()
    {
        SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Versione=3;");
        connection.Open();
        string sql = "SELECT strftime('%d/%m/%Y', scadenza) as scadenza, prodotti.nome AS nome, categorie.nome AS categoria, * FROM prodotti JOIN categorie ON prodotti.id_categoria == categorie.id";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"id: {reader["id"]}, nome: {reader["nome"]}, prezzo:{reader["prezzo"]}, quantita: {reader["quantita"]}, stato: {reader["stato"]}, scadenza: {reader["scadenza"]}, categoria: {reader["categoria"]}");
        }
        connection.Close();
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