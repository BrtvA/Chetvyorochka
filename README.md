# Chetvyorochka

В данном ASP.NET приложении реализовано два типа типа пользователей:

1. Администратор

- просмотр текущего каталога продуктов
- добавление и редактирование категорий продуктов
- добавление и редактирование продуктов в каталоге

2. Покупатели

- пополнение индивидуального счета
- добавление(удаление) продукта в корзину
- покупка товаров, находящихся в корзине (реализовано в виде считывания средств со счета пользователя и уменьшения количества товаров в каталоге)

# Пример запуска в Docker

1. Создаем docker image:
```
docker build -f src/Chetvyorochka.PL/Dockerfile -t brtva/chetvyorochka:dev2 .
```

2. Содаем docker network:
```
docker network create --driver bridge --subnet 172.28.0.0/16 --gateway 172.28.5.254 chetnet
```

3. Запускаем docker контейнеры в созданной сети с использованием environment variables:

- для СУБД postgres:
```
docker run --network chetnet --name postgres -e POSTGRES_PASSWORD=pgpassword -d -p 5432:5432 postgres
```

- для ASP.NET приложения:
```
docker run --network chetnet --name chetvyorochka -e ConnectionStrings:ProductDB="Server=172.28.5.254;Database=productdb;User Id=postgres;Password=pgpassword;" -e User:Login="admin" -e User:Name="Иван" -e User:LastName="Иванов" -e User:Password=1234 -p 5000:80 brtva/chetvyorochka:dev2
```
