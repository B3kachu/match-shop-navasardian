// Классы таблиц: Customer, Product, Order (шаги 1–2).

public class Customer
{
    public int Id { get; set; }              // первичный ключ
    public string Name { get; set; } = "";   // имя
    public string Phone { get; set; } = "";  // телефон
    public string City { get; set; } = "";   // город
    public List<Order> Orders { get; set; } = new();   // заказы покупателя
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";   // название
    public decimal Price { get; set; }       // цена коробка, ₽ (decimal — для денег)
    public int MatchesPerBox { get; set; }   // спичек в коробке
    public int Stock { get; set; }           // коробок на складе
    public List<Order> Orders { get; set; } = new();   // заказы этого товара

    // Усложнение: описание может отсутствовать
    public string? Description { get; set; }
    
}

// Вопрос: «У некоторых спичек есть описание, например „горят даже под дождём“. А у некоторых его нет.»
// Если сравнить в CREATE TABLE "Products" две строки:
// "Name" text NOT NULL,
// "Description" text,
// У Name есть NOT NULL, а у Description нету, это происходит из-за ? у string. Он означает что "Может быть пустым" и EF разрешает в этой колонке NULL
// Без ? у EF считает поле обязательным

public class Order
{
    public int Id { get; set; }
    public int Quantity { get; set; }                 // сколько коробок

    public int CustomerId { get; set; }               // внешний ключ на покупателя
    public Customer Customer { get; set; } = null!;   // сам покупатель

    // Задание: связь с товаром
    public int ProductId { get; set; }                // внешний ключ на товар
    public Product Product { get; set; } = null!;     // сам товар

    // Усложнение: дата и время заказа
    public DateTime CreatedAt { get; set; } // выбрал тип DateTime т.к нужна дата с временем, postgreSQL получился "timestamp with time zone"
}