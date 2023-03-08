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

Пример команды для создания docker контейнера:
```
docker build -f src/Chetvyorochka.PL/Dockerfile -t brtva/chetvyorochka:dev2 .
```

Пример команды для запуска docker контейнера с использованием environment variables:
```
docker run -e ConnectionStrings:ProductDB="Host=localhost;Database=productdb2;User Id=postgres;Password=pgpassword;" -e User:Login="admin" -e User:Name="Иван" -e User:LastName="Иванов" -e User:Password=1234 -p 5000:80 brtva/chetvyorochka:dev2
```
