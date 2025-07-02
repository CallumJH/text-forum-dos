var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Whispers_Chat_Web>("web");

builder.Build().Run();
