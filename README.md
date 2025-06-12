# ORIS_semwork_EKO_API

инструкция по запуску
1. Открыть IDE
2. Создать локально бд под названием EkoDB
3. В Eko.Host/appsetting.json "Db" : "Host=localhost;Port=5432;Database=EkoDB;Username=;Password=;Include Error Detail=true" в поле Password и Username ввести ваши параметры
4. Сделай миграцию
   команды: 1. dotnet ef migrations add initial_13 -s Eko.Host -p Eko.Database
             2. dotnet ef database update -s Eko.Host -p Eko.Database
5. Запустить проект
6. перейти на localhost:5252

для доступа в админ панель нужно залогиниться как admin@gmail.com пароль admin
при миграции в бд автоматом добавляется это поля
и перейти на localhost:5252/AdminPanel/AdminPanel
