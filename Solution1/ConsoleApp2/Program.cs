using Domain.Location;
using Domain.LocationsContext.ValueObjects;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Тестирование создания Location ===");

            try
            {
                // Пример КОРРЕКТНЫХ данных
                var locationId = LocationId.Create();
                var locationName = LocationName.Create("Московский офис");
                var locationAddress = LocationAddress.Create("Россия, Москва, ул. Тверская, 7");
                var ianaTimeZone = IanaTimeZone.Create("Europe/Moscow");
                var entityLifeTime = EntityLifeTime.Create(
                    createdAt: DateTime.UtcNow.AddDays(-1),
                    updatedAt: DateTime.UtcNow
                );

                // Создаем Location
                var location = new Location(
                    locationId,
                    locationAddress,
                    locationName,
                    ianaTimeZone,
                    entityLifeTime
                );

                // Выводим информацию
                Console.WriteLine($"\n✅ Успешно создана локация:");
                Console.WriteLine($"   ID: {location.Id.Value}");
                Console.WriteLine($"   Название: {location.Name.Value}");
                Console.WriteLine($"   Адрес: {location.Address.Value}");
                Console.WriteLine($"   Части адреса: {string.Join("; ", location.Address.AddressParts)}");
                Console.WriteLine($"   Временная зона: {location.TimeZone.Value}");
                Console.WriteLine($"   Создано: {location.LifeTime.CreatedAt}");
                Console.WriteLine($"   Обновлено: {location.LifeTime.UpdatedAt}");
                Console.WriteLine($"   Активно: {location.LifeTime.IsActive}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n❌ Ошибка валидации: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }

            Console.WriteLine("\n=== Тестирование НЕКОРРЕКТНЫХ данных ===");

            // Тест 1: Пустой GUID
            try
            {
                var badId = LocationId.Create(Guid.Empty);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✅ Ожидаемая ошибка (ID): {ex.Message}");
            }

            // Тест 2: Неверный формат TimeZone
            try
            {
                var badTimeZone = IanaTimeZone.Create("InvalidTimeZone");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✅ Ожидаемая ошибка (TimeZone): {ex.Message}");
            }

            // Тест 3: Пустой адрес
            try
            {
                var badAddress = LocationAddress.Create("");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✅ Ожидаемая ошибка (Адрес): {ex.Message}");
            }

            // Тест 4: Слишком короткое название
            try
            {
                var badName = LocationName.Create("AB");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✅ Ожидаемая ошибка (Название): {ex.Message}");
            }

            // Тест 5: Некорректные даты
            try
            {
                var badLifeTime = EntityLifeTime.Create(
                    DateTime.MinValue,
                    DateTime.UtcNow
                );
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✅ Ожидаемая ошибка (Даты): {ex.Message}");
            }

            Console.WriteLine("\n=== Тестирование сравнения Value Objects ===");

            // Тест сравнения record объектов
            var timeZone1 = IanaTimeZone.Create("Europe/Moscow");
            var timeZone2 = IanaTimeZone.Create("Europe/Moscow");

            Console.WriteLine($"Сравнение одинаковых TimeZone: {timeZone1 == timeZone2}"); // true
            Console.WriteLine($"Equals: {timeZone1.Equals(timeZone2)}"); // true

            Console.WriteLine("\n=== Все тесты завершены ===");
        }
    }
}


