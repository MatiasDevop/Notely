var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithHostPort(5432)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var notesDatabase = postgres.AddDatabase("notely-notes");
var tagsDatabase = postgres.AddDatabase("notely-tags");

var tagsApi = builder.AddProject<Projects.Tags_Api>("tags-api")
    .WithReference(tagsDatabase)
    .WaitFor(tagsDatabase);

builder.AddProject<Projects.Notes_Api>("notes-api")
    .WithHttpEndpoint(5001, name: "public")
    .WithReference(notesDatabase)
    .WithReference(tagsApi)
    .WaitFor(notesDatabase)
    .WaitFor(tagsApi);



builder.Build().Run();
