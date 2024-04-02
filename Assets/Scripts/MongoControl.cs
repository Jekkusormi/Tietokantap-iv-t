using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using SimpleJSON;

public class MongoControl : MonoBehaviour
{

    private MongoClient client = new MongoClient("mongodb+srv://Heipparallaa:Tietokantapaivat@tietokantapaivat.ijtoj9j.mongodb.net/?retryWrites=true&w=majority&appName=Tietokantapaivat");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    void Start()
    {
        database = client.GetDatabase("HighscoreDB");
        collection = database.GetCollection<BsonDocument>("HighScoreCollection");
        
    }

    
    void Update()
    {
        
    }

    public async void SaveScoreToDatabase(string username, int score)
    {
        var document = new BsonDocument { {"username", username},{"score", score} };
        await collection.InsertOneAsync(document);
        Debug.Log("Pisteet syötetty tietokantaan");
    }

    public async Task<List<HighScore>> GetScoresFromDatabase()
    {
        Debug.Log("Haetaan pisteet tietokannasta");
        // Haetaan kaikki informaatio collectionista ja tehdään siitä uusi Bson dokumentti
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;
        // Tehdään lista Highscoreista
        List<HighScore> highScores = new List<HighScore>();
        Debug.Log("Data saatu tietokannasta");
        foreach(var score in scoresAwaited.ToList())
        {
            highScores.Add(Deserialize(score.ToString()));
            //Loopin lopputuloksen on highscores lista, joka sisältää objekteja, joiden informaationa on username ja score
        }

        return highScores;
    }

    private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();
        Debug.Log("RAW JSON: " + rawJson);
        JSONNode node = JSON.Parse(rawJson); // Tämä automatisoi json datan node objekteiksi
        highScore.username = node["username"];
        highScore.score = node["score"];
        return highScore;

    }

}


public class HighScore
{
    public string username
    {
        get; set;
    }
    public string score
    {
        get; set;
    }
}
