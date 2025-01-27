using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add HTTPS redirection
app.UseHttpsRedirection();


//create Class instance this is empty team instance

Team? team = null;


//template endpoint we can add here our index html (homepage)
app.MapGet("/helo", () => "Hello World!")
    .WithName("hello")
    .WithOpenApi();

//create team if it does not exist

app.MapPost("/createteam", ([FromBody] Team newTeam) =>
{

    //check if team created or not 
    if (team != null)
    {
        return Results.Ok(new { Message = $"team with name {team.Name} is already created" });
    }

    //create new team
    team = newTeam;

    return Results.Ok(new { Message = $"Classroom '{newTeam.Name} created" });

});


app.MapGet("/team", () =>
{

    if (team == null)
    {
        return Results.BadRequest(new { Message = "You have not created a team yet" });
    }

    return Results.Ok(team.players);


});

//add player 

app.MapPost("/addplayer", ([FromBody] Player player) =>
{

    //check first if team exists 
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var existedPlayer = team.players.FirstOrDefault(s => s.Id == player.Id);
    if (existedPlayer != null)
    {

        return Results.BadRequest(new { Message = $"Player with Id : {player.Id} already exists" });
    }

    //check name is empty or not 
    if (string.IsNullOrWhiteSpace(player.Name))
    {
        return Results.BadRequest(new { Message = $"Player name is required" });
    }

    //again let us check the latest Id number and add 1 
    int newId = team.players.Any() ? team.players.Max(p => p.Id) + 1 : 1;
    //create a player or update
    player.Id = newId;

    // add to class
    team.AddPlayer(player);
    return Results.Ok(new { Message = $"player with  id {player.Id} added" });

});

//UPDATE BY ID 
app.MapPut("/updateplayer/{id:int}", ([FromRoute] int id, [FromBody] Player updatedPlayer) =>
{
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }
    var existedPlayer = team.players.FirstOrDefault(s => s.Id == id);
    if (existedPlayer == null)
    {
        return Results.NotFound(new { Message = $"Player with Id {id} not found" });
    }

    //check against empty entry 
    if (!string.IsNullOrWhiteSpace(updatedPlayer.Name))
    {
        existedPlayer.Name = updatedPlayer.Name;

    }

    existedPlayer.Age = updatedPlayer.Age;

    return Results.Ok(new { Message = $"Player with id {id} updated" });

});

// Find a player by ID
app.MapGet("/findplayer/{id:int}", ([FromRoute] int id) =>
{
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a class first" });
    }

    var player = team.players.FirstOrDefault(s => s.Id == id);
    if (player == null)
    {
        return Results.NotFound(new { Message = $"Player with Id {id} not found" });
    }

    return Results.Ok(player);
});

//Find player by star
app.MapGet("/findstar/{star:int}", ([FromRoute] int star) =>
{
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a class first" });
    }

    var player = team.players.FirstOrDefault(s => s.Star == star);
    if (player == null)
    {
        return Results.NotFound(new { Message = $"Players with {star} stars not found" });
    }

    return Results.Ok(player);
});

app.MapDelete("/deleteplayer", ([FromBody] int id) =>
{
    // Ensure the team exists
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    // Find the player by ID
    var existedPlayer = team.players.FirstOrDefault(s => s.Id == id);

    if (existedPlayer == null)
    {
        return Results.NotFound(new { Message = $"Player with Id: {id} not found" });
    }

    // Remove the matched player
    team.players.Remove(existedPlayer);

    return Results.Ok(new { Message = $"Player with Id: {id} deleted" });
});

//player things 
app.MapGet("/findyoungest", () =>
{

    // Ensure the team exists
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var youngestPlayerAge = team.players.Min(s => s.Age);
    var youngestPlayer = team.players.Where(s => s.Age == youngestPlayerAge);

    return Results.Ok(new { Message = youngestPlayer });



});

app.MapGet("/findoldest", () =>
{

    // Ensure the team exists
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }
    var oldestPlayerAge = team.players.Max(s => s.Age);
    var oldestPlayer = team.players.Where(s => s.Age == oldestPlayerAge);
    return Results.Ok(new { Message = oldestPlayer });

});

app.MapGet("/findmaximumage", () =>
{

    // Ensure the team exists
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }

    var maximumage = team.players.Max(s => s.Age);

    return Results.Ok(new { Message = maximumage });



});

app.MapGet("/findminimumage", () =>
{

    // Ensure the team exists
    if (team == null)
    {
        return Results.BadRequest(new { Message = "You must create a team first" });
    }
    var minimumage = team.players.Min(s => s.Age);
    
    return Results.Ok(new { Message = minimumage });

});






app.Run();
