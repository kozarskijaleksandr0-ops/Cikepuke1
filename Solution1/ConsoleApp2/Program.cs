using System;
using DirectoryService.Domain.DepartmentsContext;
using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.PositionsContext;
using DirectoryService.Domain.PositionsContext.ValueObjects;
using DirectoryService.Domain.Shared;
using Domain.Location;
using Domain.LocationsContext.ValueObjects;

namespace ConsoleApp2
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

            Console.WriteLine("\n=== Тестирование создания Department ===");

            try
            {
                // Создаем корневое подразделение (IT отдел)
                var itDepartment = new Department(
                    id: DepartmentId.Create(),
                    name: DepartmentName.Create("IT отдел"),
                    identifier: DepartmentIdentifier.Create("it"),
                    isActive: true,
                    lifeTime: EntityLifeTime.Create(
                        createdAt: DateTime.UtcNow.AddMonths(-1),
                        updatedAt: DateTime.UtcNow
                    )
                );

                Console.WriteLine($"\n✅ Создано корневое подразделение:");
                Console.WriteLine($"   Название: {itDepartment.Name.Value}");
                Console.WriteLine($"   Идентификатор: {itDepartment.Identifier.Value}");
                Console.WriteLine($"   Путь: {itDepartment.Path.Value}");
                Console.WriteLine($"   Глубина: {itDepartment.Path.Depth}");
                Console.WriteLine($"   Родитель: {(itDepartment.ParentId == null ? "Нет" : itDepartment.ParentId.Value)}");

                // Создаем дочернее подразделение (Разработка)
                var devDepartment = itDepartment.CreateChild(
                    childName: DepartmentName.Create("Разработка"),
                    childIdentifier: DepartmentIdentifier.Create("dev-team")
                );

                Console.WriteLine($"\n✅ Создано дочернее подразделение:");
                Console.WriteLine($"   Название: {devDepartment.Name.Value}");
                Console.WriteLine($"   Идентификатор: {devDepartment.Identifier.Value}");
                Console.WriteLine($"   Путь: {devDepartment.Path.Value}");
                Console.WriteLine($"   Глубина: {devDepartment.Path.Depth}");
                Console.WriteLine($"   Родитель: {devDepartment.ParentId?.Value}");

                // Создаем подразделение второго уровня (Frontend)
                var frontendDepartment = devDepartment.CreateChild(
                    childName: DepartmentName.Create("Frontend разработка"),
                    childIdentifier: DepartmentIdentifier.Create("frontend")
                );

                Console.WriteLine($"\n✅ Создано подразделение второго уровня:");
                Console.WriteLine($"   Название: {frontendDepartment.Name.Value}");
                Console.WriteLine($"   Идентификатор: {frontendDepartment.Identifier.Value}");
                Console.WriteLine($"   Путь: {frontendDepartment.Path.Value}");
                Console.WriteLine($"   Глубина: {frontendDepartment.Path.Depth}");
                Console.WriteLine($"   Родитель: {frontendDepartment.ParentId?.Value}");

                // Тест некорректного идентификатора
                Console.WriteLine("\n=== Тест некорректных данных ===");
                try
                {
                    var badIdentifier = DepartmentIdentifier.Create("Неправильный_ID"); // Кириллица
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"✅ Ожидаемая ошибка (идентификатор): {ex.Message}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n❌ Ошибка валидации: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }

            // ВЫЗЫВАЕМ TestPosition() - добавить эту строку
            TestPosition();

            Console.WriteLine("\n=== Все тесты завершены ===");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        } // ЗАКРЫВАЕМ метод Main здесь

        // ПЕРЕНЕСТИ метод TestPosition() СЮДА (вне метода Main)
        static void TestPosition()
        {
            Console.WriteLine("\n=== Тестирование создания Position ===");

            try
            {
                // Создаем позицию Разработчик
                var developerPosition = new Position(
                    id: PositionId.Create(),
                    name: PositionName.Create("Разработчик"),
                    description: PositionDescription.Create("Занимается разработкой программного обеспечения"),
                    isActive: true,
                    lifeTime: EntityLifeTime.Create(
                        createdAt: DateTime.UtcNow.AddMonths(-6),
                        updatedAt: DateTime.UtcNow
                    )
                );

                Console.WriteLine($"\n✅ Создана позиция 'Разработчик':");
                Console.WriteLine($"   ID: {developerPosition.Id.Value}");
                Console.WriteLine($"   Название: {developerPosition.Name.Value}");
                Console.WriteLine($"   Описание: {developerPosition.Description.Value}");
                Console.WriteLine($"   Активна: {developerPosition.IsActive}");
                Console.WriteLine($"   Создана: {developerPosition.LifeTime.CreatedAt}");
                Console.WriteLine($"   Обновлена: {developerPosition.LifeTime.UpdatedAt}");

                // Создаем позицию Менеджер по продажам
                var salesManagerPosition = new Position(
                    id: PositionId.Create(),
                    name: PositionName.Create("Менеджер по продажам"),
                    description: PositionDescription.Create("Отвечает за продажи и работу с клиентами"),
                    isActive: true,
                    lifeTime: EntityLifeTime.Create(
                        createdAt: DateTime.UtcNow.AddMonths(-3),
                        updatedAt: DateTime.UtcNow
                    )
                );

                Console.WriteLine($"\n✅ Создана позиция 'Менеджер по продажам':");
                Console.WriteLine($"   Название: {salesManagerPosition.Name.Value}");
                Console.WriteLine($"   Описание: {salesManagerPosition.Description.Value}");

                // Создаем неактивную позицию
                var inactivePosition = new Position(
                    id: PositionId.Create(),
                    name: PositionName.Create("Устаревшая позиция"),
                    description: PositionDescription.Create("Эта позиция больше не используется"),
                    isActive: false,
                    lifeTime: EntityLifeTime.Create(
                        createdAt: DateTime.UtcNow.AddYears(-1),
                        updatedAt: DateTime.UtcNow.AddMonths(-6)
                    )
                );

                Console.WriteLine($"\n✅ Создана неактивная позиция:");
                Console.WriteLine($"   Название: {inactivePosition.Name.Value}");
                Console.WriteLine($"   Активна: {inactivePosition.IsActive}");

                // Тест некорректных данных
                Console.WriteLine("\n=== Тест некорректных данных Position ===");

                try
                {
                    var badName = PositionName.Create(""); // Пустое название
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"✅ Ожидаемая ошибка (пустое название): {ex.Message}");
                }

                try
                {
                    var tooLongDescription = PositionDescription.Create(new string('a', 501)); // 501 символ
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"✅ Ожидаемая ошибка (длинное описание): {ex.Message}");
                }

                try
                {
                    var badId = PositionId.Create(Guid.Empty); // Пустой GUID
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"✅ Ожидаемая ошибка (пустой ID): {ex.Message}");
                }

                // Тест сравнения record объектов
                Console.WriteLine("\n=== Тест сравнения Position объектов ===");
                var position1 = PositionName.Create("Разработчик");
                var position2 = PositionName.Create("Разработчик");
                Console.WriteLine($"Сравнение одинаковых PositionName: {position1 == position2}"); // true

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n❌ Ошибка валидации: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
        }
    } // Конец класса Program
} // Конец namespace