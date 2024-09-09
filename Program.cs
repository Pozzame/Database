using System.Data.SQLite;

string path = @"database.db";
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
            categoria TEXT UNIQUE,
            descrizione text
        );
        INSERT INTO categorie (categoria, descrizione) 
            VALUES ('Carne', 'La carne');
        INSERT INTO categorie (categoria, descrizione) 
            VALUES ('Pesce', 'Il pesce');
        INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
            VALUES (Simmental, 10, 100, true, 2026-12-12, 1);
        INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
            VALUES (Riomare, 20, 200, true, 2025-12-12, 2);
        INSERT INTO prodotti (nome, prezzo, quantita, stato, scadenza, id_categoria) 
            VALUES (Manzotin, 10, 100, true, 2026-12-12, 1);
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
