using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
//Print

#region Main

#region Extension
public static class CollectionExtensions {
    public static void PrintContents<T>(this IEnumerable<T> collection, string title = null) {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(title)) {
            sb.AppendLine($"--- {title} ---");
        } else {
            string collectionType = collection?.GetType().Name ?? "IEnumerable";
            sb.AppendLine($"--- Содержимое {collectionType} ---");
        }

        if (collection == null) {
            sb.AppendLine("  [Коллекция: null]");
            Console.WriteLine(sb.ToString());
            return;
        }

        int index = 0;
        foreach (var item in collection) {
            sb.AppendLine($"  [{index}]: {item}");
            index++;
        }

        if (index == 0) {
            sb.AppendLine("  [Коллекция пуста]");
        }

        sb.AppendLine($"----------------- (Всего: {index})");

        Console.WriteLine(sb.ToString());
    }
}
#endregion
public class Programm {
    public static void ListWork() {
        List<int> intList = new List<int>() {
            1,2
        };

        #region LIst<T> props
        var count = intList.Count;
        var capacity = intList.Capacity;
        #endregion 

        #region List<T> methods
        intList.Add(3);
        intList.AddRange([4, 5, 6]);
        intList.InsertRange(3, [4, 4]); //вставка в середину (долго нудно фуфуфу , лучше для такой фигни линкед лист использовать)
        intList.Remove(6);
        intList.RemoveAt(0);
        //intList.Clear();
        intList.Contains(1); //O(n)
        intList.IndexOf(1); //O(n)
        intList.Reverse();
        intList.Sort();
        //intList.ToArray();
        intList.ForEach(a => { Console.WriteLine(a); });
        #endregion

        #region List<T> methods alya Nifiga Sebe sho umeet
        var firstElement = intList.Find(a => a > 4); // возвращает первый метод по суловию ()
        var listElement = intList.FindAll(a => a > 4);
        var indexelement = intList.FindIndex(a => a == 4); //Индекс первого подходящего
        var existelement = intList.Exists(a => a == 4);
        intList.RemoveAll(a => a == 4);
        var strings = intList.ConvertAll(x => $"Число: {x}");
        #endregion

        intList.PrintContents("Тестовый список");
    }

    public static void DictionaryWork() {
        var capitals = new Dictionary<string, string> {
            ["Ukraine"] = "Kyiv",
            ["France"] = "Paris",
            ["Germany"] = "Berlin"
        };

        #region Dictionary<T,T> props
        var keys = capitals.Keys;
        var values = capitals.Values;
        #endregion

        #region Methods
        capitals.Add("USA", "Washington");
        capitals.Remove("USA");
        //capitals.Clear();
        capitals.ContainsKey("USA");
        capitals.ContainsValue("Berlin");
        string value = "";
        capitals.TryGetValue("Ukraine", out value);// безопасное получение начения 
        capitals.ToList();
        capitals.ToDictionary(u => u);
        #endregion

        capitals.PrintContents();
    }

    public static void HashSetWork() {
        HashSet<int> hsInt = new HashSet<int>() { 1,2,3,4};
        HashSet<int> tempIntHs = new HashSet<int> { 2, 3, 4, 5, 6 };

        #region Props
        var comperor = hsInt.Comparer; //комперер по которому сравниваются елементы
        #endregion

        #region methods
        hsInt.Add(1); // он кста возвращает тру или фолс  не как у списка дефолтного
        //выделю уже только уникальные методы для хеш сета
        hsInt.UnionWith(tempIntHs); // объединение
        hsInt.IntersectWith(tempIntHs); //только пересечения
        hsInt.ExceptWith(tempIntHs); // удаляет пересечения
        hsInt.SymmetricExceptWith(tempIntHs); //короче семетричный ексепт, он из hsInt удалит пересечения с tempIntHs, и добавит в hsInt то чего нету в tempIntHs 
        bool tf = hsInt.SetEquals(tempIntHs); //Проврека равны ли
        bool tf1 = hsInt.IsSubsetOf(tempIntHs);//проверка на наличие подмножеств 
        bool tf2 = hsInt.IsSupersetOf(tempIntHs); // проверяет, содержит ли все элементы другого
        //hsInt.CopyTo(arr); - копирование в масив
        #endregion

        hsInt.PrintContents();
    }

    public static void LInkedListWork() {

        //LinkedList<T> — это двусвязный список: каждый элемент хранит ссылку на следующий и на предыдущий.

        /*
         * Каждый элемент — это объект LinkedListNode<T>, который содержит:
            Value — данные узла;
            Next — ссылка на следующий узел;
            Previous — ссылка на предыдущий узел.
        */
        var linked = new LinkedList<int>();

        #region Props 
        var first = linked.First;
        var last = linked.Last;
        var count = linked.Count;
        #endregion

        #region methods
        linked.AddLast(1);   // [1]
        linked.AddLast(2);   // [1] ↔ [2]
        linked.AddFirst(0);  // [0] ↔ [1] ↔ [2]

        // Получение ноды
        var firstNode = linked.First;
        var lastNode = linked.Last;
        var FindedNode = linked.Find(2); //O(n)

        linked.AddBefore(FindedNode, 4); //перед нодой
        linked.AddAfter(FindedNode, 5);//полсе ноды
        linked.Remove(5);
        linked.RemoveFirst();
        linked.RemoveLast();
        #endregion

        linked.PrintContents();
    }

    public static void QueueWork() {
        var queue = new Queue<string>();//Queue<T> — это очередь (FIFO), то есть First In — First Out:
        //например решает задачу , с переносом пакетов данных в базу. Например если 1000000 записей ппереносятся по пакетно, то можно использовать очередь

        #region methods
        queue.Enqueue("first");
        queue.Enqueue("second");
        queue.Enqueue("third");
        queue.Enqueue("fourth");
        queue.Dequeue(); // Удалить и вернуть первый елемент
        queue.Peek(); //Вернуть первый елемент   
        //Остальное в целом как и у списка
        queue.TryDequeue(out var value); //что бы не падал ексепшн если в очереди нету значений
        #endregion

        queue.PrintContents();

        //Если к коллекции обраается несколько потоков то нужно использовать Потокобезопасная альтернатива — ConcurrentQueue<T>, 
        // бо может произойти искажение данных
    }

    public static void ObservableCollection() {
        /*
         это коллекция-список, которая присылает уведомления об изменениях подписчикам (UI, коллекционные представления),
        реализуя INotifyCollectionChanged и INotifyPropertyChanged. 
        Ключевое применение — WPF/MAUI/MVVM: автоматическое обновление ItemsControl при Add/Remove/Move/Clear/Replace
         */
    }

    public static void StackWork() {
        var stack = new Stack<string>();

        #region methods
        stack.Push("A"); // [A]
        stack.Push("B"); // [A, B]
        stack.Push("C"); // [A, B, C]
        stack.TryPop(out var value);
        stack.Pop();
        #endregion 
    }
    public static void Main(string[] args) {
        //ListWork();
        //DictionaryWork();
        //HashSetWork();
        //LInkedListWork();
        //QueueWork();
    }

    /*
    Бонусом:
        SortedList<TKey, TValue> - Это как Dictionary, но всегда хранит элементы отсортированными по ключу
        Внутри реализован как два параллельных массива (keys[], values[]), поэтому поиск — быстрый, а вставка — дорогая

        SortedDictionary<TKey, TValue> То же самое, что SortedList, но реализован через красно-чёрное дерево.
        Потому вставка, удаление и поиск занимают одинаково — O(log n).
    */
}
#endregion