using System;
using System.Collections.Generic;
using System.Linq;

public class Employee {
	public Guid Id { get; set; }
	public DateTime Birthday { get; set; }
	public DateTime? BnzNotificationSended { get; set; }

	public int Age { get; set; }
}

public class Department {
	public string Name { get; set; }

	public List<string> Employees { get; set; }
}

class Program {
	static void Main() {
		var employees = new List<Employee>
		{
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1990, 10, 25), BnzNotificationSended = null, Age = 32 },
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1985, 11, 25), BnzNotificationSended = DateTime.Now.AddDays(-1), Age = 25 },
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1995, 09, 20), BnzNotificationSended = null, Age = 21 },
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1995, 10, 12), BnzNotificationSended = null, Age = 24 },
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1990, 1, 10), BnzNotificationSended = null, Age = 38 }
		};

		var employees2 = new List<Employee>
{
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1990, 10, 25), BnzNotificationSended = null , Age = 25},
			new Employee { Id = Guid.NewGuid(), Birthday = new DateTime(1985, 11, 25), BnzNotificationSended = DateTime.Now.AddDays(-1), Age = 38 }
		};

		#region Методы которые сразу материализуют результат

		#region Логический тип
		//проверяе существуют ли елементы по условию
		bool hasSend = employees.Any(e => e.BnzNotificationSended != null);
		Console.WriteLine("hasSend = " + hasSend);

		//проверяет ВСЕ ли елементы соответсвуют условию
		bool allHasSend = employees.All(e => e.BnzNotificationSended == null);
		Console.WriteLine("allHasSend = " + allHasSend);

		//Проверка содержит ли коллекция указаный елементы
		//bool hasBDate = employees.Contains(); //с моим сложным объектом это не сработает , бо оно будет проверять по ссылке , нужно оверрайдить метод
		//По сложным объектам используют Any

		//проверка что две коллекции еквивалентны 
		bool sequenceEqual = employees.SequenceEqual(employees2);
		Console.WriteLine("sequenceEqual = " + sequenceEqual);
		#endregion

		#region Числовой тип

		//Среднее значение по какому то условию
		double avg = employees.Average(e => e.Age);
		Console.WriteLine("avg = " + avg);

		//Количество записей
		int count = employees.Count();
		Console.WriteLine("count = " + count);

		//Максимальное значение 
		double max = employees.Max(e => e.Age);
		Console.WriteLine("max = " + max);

		//Минимальное значение
		double min = employees.Min(e => e.Age);
		Console.WriteLine("min = " + min);

		//Сумма
		double sum = employees.Sum(e => e.Age);
		Console.WriteLine("sum = " + sum);
		#endregion

		#region Елемент коллекции

		//Елемент по индексу
		var element = employees.ElementAt(0);
		Console.WriteLine("element = " + element.Id);

		//Так же елеменнт по индексу, либо дефолт
		var defEl = employees.ElementAtOrDefault(0);
		Console.WriteLine("defEl = " + defEl.Id);

		//Первый елемент последовательности, либо исключение
		var first = employees.First();
		Console.WriteLine("first = " + first);

		//по такой же  логикие Last или LastOrDefault
		var last = employees.Last();
		var lastOrDef = employees.LastOrDefault();

		//Возврат единственного значения коллекции. Если в выборке больше 1 елемента
		//будет исключение

		var single = employees.Single(e => e.BnzNotificationSended != null);
		Console.WriteLine("single = " + single.Id);

		//employees.SingleOrDefault просто не выбрасывает исключение

		//провести операцию с каждым елементом последовательно
		int AgregatedAge = employees
			.Select(e => e.Age)
			.Aggregate((a, b) => a + b);
		Console.WriteLine("AgregatedAge = " + AgregatedAge);

		#endregion

		#region Коллекция
		//привести коллекцию в масив
		var emppToArr = employees.Select(e => e.Id).ToArray();
		Console.WriteLine("\n");
		Console.WriteLine("Array");
		foreach (var e in emppToArr) {
			Console.WriteLine("value : " + e);
		}

		//Пример как перевести в дикшинари
		var empDictionary = employees
			.ToDictionary(e => e.Id, e => e.Age);
		Console.WriteLine("\n");
		Console.WriteLine("Dictionary");
		foreach (var kvp in empDictionary) {
			Console.WriteLine("Key (Id): " + kvp.Key);
			Console.WriteLine("Value (Age): " + kvp.Value);
		}

		//ToList так же как и масив только ту лист
		#endregion

		#endregion

		#region Методы которые формируют запрос но не возвращают результат (Lazy методы)

		#region Проекция данных

		//Select - проекция с некой обработкой каждого елемента
		var employeesAge = employees.Select(e => e.Age);
		Console.WriteLine("\n");
		foreach (var age in employeesAge)
			Console.WriteLine(age);
		employeesAge.ElementAtOrDefault(1);
		//SelectMany - "Разворачивает" вложеные коллекции вв одну плоскую
		var departments = new List<Department> {
			new Department { Name = "IT", Employees = new List<string> { "Alice", "Bob" } },
			new Department { Name = "HR", Employees = new List<string> { "Tom" } }
		};

		var depEmployees = departments.SelectMany(d => d.Employees);
		Console.WriteLine("\n");
		foreach (var emp in depEmployees)
			Console.WriteLine(emp);

		//Join - дефолтный джоин по коллекциям по какому то признаку
		var joinedGroup = employees.Join(
			employees2, // коллекция присоединения
			e => e.Birthday, // колонка джоина для employees
			e2 => e2.Birthday, // колонка джоина для employees2
			(e, e2) => new { e.Id, e.Birthday } // результат проекции
		);
		Console.WriteLine("\n");
		foreach (var r in joinedGroup) {
			Console.WriteLine($"Employee: {r.Id}, Dept: {r.Birthday}");
		}

		//Distinct - удаляет дубликаты 
		//var nonDuplicated = employees.Distinct(e => e.Age);
		//для сложных коллекций нужно указывать свой IEqualityComparer или DistinctBy

		//DistinctBy - тот же дистинкт только для сложных объектов
		var nonDuplicated = employees.DistinctBy(e => e.Age);
		Console.WriteLine("\n");
		foreach (var n in nonDuplicated) {
			Console.WriteLine($"Employee: {n.Id}, Age: {n.Age}");
		}

		//Reverse - просто разворачивает последовательность , я юзал это на списки
		int[] numbers = [1, 2, 3, 4, 5, 6];
		var reversed = numbers.Reverse();
		Console.WriteLine(string.Join(", ", reversed));

		//Cast - преобразовует елемент к указаному типу
		IEnumerable<object> objs = new object[] { 1, 2, 3 };
		var ints = objs.Cast<int>(); // Явное приведение типа
		Console.WriteLine(string.Join(", ", ints)); // 1, 2, 3

		//OfType - отбираает только те елементы которые соответсвуют уникальному типу
		IEnumerable<object> objs1 = new object[] { 1, "Hello", 2, "World" };
		var onlyStrings = objs1.OfType<string>();

		foreach (var s in onlyStrings) {
			Console.WriteLine(s);
		}

		#endregion

		#region Операции с множествами

		//Concat - объединение последовательностей 
		var concated = employees.Concat(employees2);
		Console.WriteLine("\n");
		Console.WriteLine("Concated count: " + concated.Count());

		//Exept - возвращает элементы из первой коллекции, которых нет во второй
		var exceptResult = employees.Except(employees2); // Изменений не будет , бо он сравнивает слодные елементы целком , а у меня они все разные
														//Так же есть ExceptBy(выражение)

		//Union - объединяет две коллекции, убирая дубликаты (множество).
		var unionResult = employees.Union(employees2);
		//Так же есть UnionBy(выражение)

		//Intersect / IntersectBy - только записи с первой коллекции что пересекаются по значениям с другой
		var intersectResult = employees
			.IntersectBy(employees2.Select(e => e.Birthday), e => e.Birthday);

		//Zip - сшивает коллекции. Идёт по порядку, пока не кончатся элементы в одной из коллекций
		var letters = new[] { "A", "B", "C" };
		var numbers1 = new[] { 1, 2, 3, 4 };
		Console.WriteLine("\n");
		var zipped = letters.Zip(numbers1, (l, n) => $"{l}-{n}");

		Console.WriteLine(string.Join(", ", zipped));

		#endregion

		#region Операции фильтрации

		//Where - Фильтрует элементы по условию (логическому предикату)
		var filteredEmpl = employees.Select(e => e.Age).Where(a => a > 25);
		Console.WriteLine("\n");
		foreach (var age in filteredEmpl)
			Console.WriteLine($"Возраст: {age}");

		//Skip - пропускает n первых елементов
		var skipTwo = employees.Skip(2);
		Console.WriteLine("\n");
		foreach (var st in skipTwo) {
			Console.WriteLine(st.Age);
		}

		//SkipWhile - пропускает по условию
		var skipWhileYoung = employees
			.OrderBy(e => e.Age) // важно! работает по порядку
			.SkipWhile(e => e.Age < 30);
		Console.WriteLine("\n");
		foreach (var emp in skipWhileYoung) {
			Console.WriteLine($"Age: {emp.Age}");
		}

		//Take - взять N елементов последовательно
		var firstThree = employees.Take(3);
		Console.WriteLine("\n");
		foreach (var emp in firstThree) {
			Console.WriteLine($"Age: {emp.Age}");
		}

		//TakeWhile - берет елементы пока выполняется условие
		var takeWhileYoung = employees
			.OrderBy(e => e.Age)
			.TakeWhile(e => e.Age < 30);

		foreach (var emp in takeWhileYoung) {
			Console.WriteLine($"Age: {emp.Age}");
		}
		#endregion

		#region Генераторы данных

		//Range - генерация последовательности целых чисел 
		var numbersRanged = Enumerable.Range(1, 5); // от 1, 5 элементов
		Console.WriteLine(string.Join(", ", numbersRanged));

		//Repeat - повторяет одно и то же значение несколько раз
		var repeats = Enumerable.Repeat("Hello", 3);
		Console.WriteLine(string.Join(", ", repeats));

		//Empty - создаст пустую коллекцию указаного типа
		//это ужобно если нужно вернуть пустую последовательность но при этом с каким то типом 
		var emptyEmployees = Enumerable.Empty<Employee>();
		Console.WriteLine(emptyEmployees.Any());


		#endregion

		#region сортировка данных 

		//OrderBy - обычная ссортировка по полю
		var orderedEmployeesByAge = employees.OrderBy(e => e.Age);
		Console.WriteLine("\n");
		foreach (var emp in orderedEmployeesByAge) {
			Console.WriteLine("age: " + emp.Age);
		}

		//OrderByDescending - та же сортировка по полю только по убыванию
		var orderedEmployeesByAgeDesc = employees.OrderByDescending(e => e.Age);
		Console.WriteLine("\n");
		foreach (var emp in orderedEmployeesByAgeDesc) {
			Console.WriteLine("age: " + emp.Age);
		}

		//GroupBy групировка по полю
		var groupedByYear = employees.GroupBy(e => e.Birthday.Year);
		Console.WriteLine("\n");

		foreach (var group in groupedByYear) {
			Console.WriteLine($"Год: {group.Key} (Всего: {group.Count()})");

			foreach (var emp in group) {
				Console.WriteLine($"  - Id: {emp.Id}, Age: {emp.Age}, Birthday: {emp.Birthday:d}");
			}

			Console.WriteLine(); // пустая строка между группами
		}

		#endregion

		#endregion

		#region PLINQ

		//PLINQ - Ето  реализация linq to object которая которая умеет разбивать обработку коллекций на несколько потоков
		//Полезен если : коллекция большая , каждая операция над елементом тяжелая , нужно ускорить обработку за счет многопоточности

		var pNumbers = Enumerable.Range(1, 20);

		var squares = pNumbers
			.AsParallel()
			.Where(n => n % 2 == 0)
			.Select(n => n * n)
			.ToList();

		//Важный нюанс 
		//По умолчанию PLINQ может поменять порядок елементов
		//по етому если важен порядок нужно использовать .AsOrdered()
		pNumbers.AsParallel().AsOrdered().Select(n => n * n);

		//Если нужно контролить степень паралельности то надо использовать .WithDegreeOfParallelism()

		var result = pNumbers
			.AsParallel()
			.WithDegreeOfParallelism(4) // максимум 4 потока
			.Select(x => x * 20)
			.ToList();

		#endregion
	}
}