// Классы таблиц: Customer, Product, Order (шаги 1–2).

public class Customer
{
    public int Id { get; set; }              // первичный ключ
    public string Name { get; set; } = "";   // имя
    public string Phone { get; set; } = "";  // телефон
    public string City { get; set; } = "";   // город
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";   // название
    public decimal Price { get; set; }       // цена коробка, ₽ (decimal — для денег)
    public int MatchesPerBox { get; set; }   // спичек в коробке
    public int Stock { get; set; }           // коробок на складе

    // Усложнение: описание может отсутствовать
    public string? Description { get; set; }
}

// Вопрос: «У некоторых спичек есть описание, например „горят даже под дождём“. А у некоторых его нет.»
// Если сравнить в CREATE TABLE "Products" две строки:
// "Name" text NOT NULL,
// "Description" text,
// У Name есть NOT NULL, а у Description нету, это происходит из-за ? у string. Он означает что "Может быть пустым" и EF разрешает в этой колонке NULL
// Без ? у EF считает поле обязательным