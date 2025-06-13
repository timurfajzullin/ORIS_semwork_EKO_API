# ORIS_semwork_EKO_API

Настройки appsetting.json 
база данных postrges 
"Db" : "Host=localhost;Port=5432;Database=EkoDB;Username=;Password=;Include Error Detail=true"

Сделать миграцию
команды: 1. dotnet ef migrations add initial_13 -s Eko.Host -p Eko.Database
   2. dotnet ef database update -s Eko.Host -p Eko.Database

как развернуть ollama и закрузить модель deepseekR1
ducker pull ollama/ollama
cpu | docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
gpu | docker run -d --gpus=all -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama
контейнер запускается по порту 11434|
загрузка модели, я использовал модель deepseekR1 : 7b
docker exec ollama ollama run deepseek-r1:7b

для доступа в админ панель нужно залогиниться как admin@gmail.com пароль admin
при миграции в бд автоматом добавляется это поля
и перейти на /AdminPanel/AdminPanel

тз 
https://docs.google.com/document/d/1Urkl-lQN89xTwbjc7ZPZ8oEw8Xf3U-Nhxs20znT4ztk/edit?tab=t.0
