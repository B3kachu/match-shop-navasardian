using Microsoft.EntityFrameworkCore;

Console.WriteLine("Магазин спичек «Чирк»: проект запущен!");

// Дальше — код по шагам задания.
using var db = new ShopContext();
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

Console.WriteLine("=== Шаг 3 ===");

// Пример: два покупателя
var ivan = new Customer { Name = "Иван Петров", Phone = "+7 900 111-22-33", City = "Омск" };
var olga = new Customer { Name = "Ольга Смирнова", Phone = "+7 900 444-55-66", City = "Томск" };
db.Customers.Add(ivan);
db.Customers.Add(olga);
db.SaveChanges();

// Задание: ещё три покупателя 
var anna  = new Customer { Name = "Анна Козлова",  Phone = "+7 900 777-88-99", City = "Омск" };
var pavel = new Customer { Name = "Павел Орлов",   Phone = "+7 900 222-33-44", City = "Новосибирск" };
var maria = new Customer { Name = "Мария Белова",  Phone = "+7 900 555-66-77", City = "Томск" };
db.Customers.AddRange(anna, pavel, maria);

// Задание: пять видов спичек, минимум два дешевле 100 ₽ 
var household = new Product { Name = "Хозяйственные", Price = 25m,  MatchesPerBox = 60,  Stock = 200 };
var hunting   = new Product { Name = "Охотничьи",     Price = 180m, MatchesPerBox = 20,  Stock = 40,
                              Description = "Горят даже под дождём" };
var fireplace = new Product { Name = "Каминные",      Price = 150m, MatchesPerBox = 50,  Stock = 30 };
var souvenir  = new Product { Name = "Сувенирные",    Price = 90m,  MatchesPerBox = 30,  Stock = 15 };
var tourist   = new Product { Name = "Туристические", Price = 120m, MatchesPerBox = 40,  Stock = 25 };
db.Products.AddRange(household, hunting, fireplace, souvenir, tourist);
db.SaveChanges();

// Усложнение: заказы за неделю.
// В заказ передаём сами объекты, а не Id: CustomerId и ProductId EF заполнит сам.
// Ответ: Передать в заказ сами объекты: Customer = ivan, Product = fireplace. Писать CustomerId = 1 не нужно, номер EF подставит сам при сохранении.
db.Orders.AddRange(
    new Order { Customer = ivan,  Product = fireplace, Quantity = 3,  CreatedAt = DateTime.UtcNow },
    new Order { Customer = ivan,  Product = household, Quantity = 10, CreatedAt = DateTime.UtcNow }, // у Ивана два заказа
    new Order { Customer = olga,  Product = hunting,   Quantity = 2,  CreatedAt = DateTime.UtcNow },
    new Order { Customer = anna,  Product = tourist,   Quantity = 1,  CreatedAt = DateTime.UtcNow }
);
// Вопрос 2: чем DateTime.Now отличается от DateTime.UtcNow?
// Ответ: Now - местное время, а UTCNow - UTC, единое время для планеты.PostgreSQL принимает только UTC, поэтому местное время Npsql не пропускает
// у Павла и Марии заказов нет
db.SaveChanges();

Console.WriteLine("=== Шаг 4 ===");

// Пример: покупатели из Омска по алфавиту
var omsk = db.Customers
    .Where(c => c.City == "Омск")
    .OrderBy(c => c.Name)
    .ToList();

foreach (var c in omsk)
    Console.WriteLine($"{c.Name}, {c.Phone}");

// Задание 1: спички дешевле 100 ₽, от дешёвых к дорогим
var cheap = db.Products
    .Where(p => p.Price < 100)
    .OrderBy(p => p.Price)
    .ToList();

Console.WriteLine("Дешевле 100 ₽:");
foreach (var p in cheap)
    Console.WriteLine($"{p.Name} — {p.Price} ₽");

// Задание 2: сколько видов спичек есть на складе
int inStock = db.Products.Count(p => p.Stock > 0);
Console.WriteLine($"Видов на складе: {inStock}");

// Задание 3: три самых дорогих товара
var top3 = db.Products
    .OrderByDescending(p => p.Price)
    .Take(3)
    .ToList();

Console.WriteLine("Самые дорогие:");
foreach (var p in top3)
    Console.WriteLine($"{p.Name} — {p.Price} ₽");

// Усложнение: все заказы и общая выручка
var orders = db.Orders
    .Include(o => o.Customer)   // подтянуть покупателя (JOIN)
    .Include(o => o.Product)    // подтянуть товар (JOIN)
    .OrderBy(o => o.Id)
    .ToList();

decimal total = 0;
foreach (var o in orders)
{
    decimal sum = o.Product.Price * o.Quantity;   // сумма заказа
    total += sum;
    Console.WriteLine($"{o.Customer.Name} — {o.Product.Name} — {o.Quantity} шт. — {sum} ₽");
}
Console.WriteLine($"Общая выручка: {total} ₽");