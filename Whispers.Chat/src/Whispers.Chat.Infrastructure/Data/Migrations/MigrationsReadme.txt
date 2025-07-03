First Run
CD C:\Users\callu\Documents\Codetest\text-forum-dos\Whispers.Chat\src\Whispers.Chat.Web

Then you can

Create New 
dotnet ef migrations add {MIGRATIONNAME} -c AppDbContext -p ../Whispers.Chat.Infrastructure/Whispers.Chat.Infrastructure.csproj -s Whispers.Chat.Web.csproj -o Data/Migrations

Update to latest
dotnet ef database update -c AppDbContext -p ../Whispers.Chat.Infrastructure/Whispers.Chat.Infrastructure.csproj -s Whispers.Chat.Web.csproj