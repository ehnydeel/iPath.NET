var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.iPath_Blazor_Server>("ipath-blazor-server");

builder.Build().Run();
