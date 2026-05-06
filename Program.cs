using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactBookApp
{
    public class Contact
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public override string ToString()
        {
            return $"{(FirstName ?? "").PadRight(12)} {(LastName ?? "").PadRight(15)} | Phone: {(Phone ?? "").PadRight(14)} | Email: {Email ?? ""}";
        }
    }

    public class DuplicateSet
    {
        private int[] parents;
        public DuplicateSet(int n)
        {
            parents = new int[n];
            for (int i = 0; i < n; i++) parents[i] = i; 
        }

        public int FindRoot(int i)
        {
            if (parents[i] == i) return i;
            return parents[i] = FindRoot(parents[i]);
        }

        public void Union(int a, int b)
        {
            int rootA = FindRoot(a);
            int rootB = FindRoot(b);
            if (rootA != rootB) parents[rootB] = rootA; 
        }
    }

    class Program
    {
        static List<Contact> allContacts = new List<Contact>();
        static List<Contact> filteredView = new List<Contact>();

        static void Main(string[] args)
        {
            SeedData(); 
            filteredView = new List<Contact>(allContacts);
            MainMenu();
        }

        static void MainMenu()
        {
            bool active = true;
            while (active)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    CONTACT BOOK: DSU & GRAPH THEORY   ");
                Console.WriteLine("========================================");
                Console.WriteLine($" Total: {allContacts.Count} | View: {filteredView.Count}");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("1. View All Contacts");
                Console.WriteLine("2. Filter Contacts (Search)");
                Console.WriteLine("3. Sort by Last Name");
                Console.WriteLine("4. RUN DSU (Detect Duplicates)");
                Console.WriteLine("5. Exit");
                Console.Write("\nSelect an option: ");

                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1": 
                        filteredView = new List<Contact>(allContacts);
                        ShowTable(); 
                        break;
                    case "2": FilterSearch(); break;
                    case "3": SortByLastName(); break;
                    case "4": RunDeduplicationAlgorithm(); break;
                    case "5": active = false; break;
                }
            }
        }

        static void RunDeduplicationAlgorithm()
        {
            Console.Clear();
            Console.WriteLine("Starting Find-Union Algorithm (DSU)...\n");

            int n = allContacts.Count;
            DuplicateSet dsu = new DuplicateSet(n);
            
            var phoneTracker = new Dictionary<string, int>();
            var emailTracker = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


            for (int i = 0; i < n; i++)
            {
                var c = allContacts[i];
                if (!string.IsNullOrEmpty(c.Phone))
                {
                    if (phoneTracker.ContainsKey(c.Phone!)) dsu.Union(phoneTracker[c.Phone!], i);
                    else phoneTracker[c.Phone!] = i;
                }
                if (!string.IsNullOrEmpty(c.Email))
                {
                    if (emailTracker.ContainsKey(c.Email!)) dsu.Union(emailTracker[c.Email!], i);
                    else emailTracker[c.Email!] = i;
                }
            }

            var groups = new Dictionary<int, List<int>>();
            for (int i = 0; i < n; i++)
            {
                int root = dsu.FindRoot(i);
                if (!groups.ContainsKey(root)) groups[root] = new List<int>();
                groups[root].Add(i);
            }

            bool found = false;
            foreach (var cluster in groups.Values)
            {
                if (cluster.Count > 1)
                {
                    found = true;
                    Console.WriteLine("⚠️ DUPLICATE CLUSTER DETECTED:");
                    foreach (int idx in cluster) Console.WriteLine($"   -> {allContacts[idx]}");
                    Console.WriteLine();
                }
            }

            if (!found) Console.WriteLine("No duplicates were found.");
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        static void FilterSearch()
        {
            Console.Write("Enter search term: ");
            string term = (Console.ReadLine() ?? "").ToLower();
            filteredView = allContacts.Where(c => 
                (c.FirstName ?? "").ToLower().Contains(term) || 
                (c.LastName ?? "").ToLower().Contains(term)).ToList();
            ShowTable();
        }

        static void SortByLastName()
        {
            filteredView = filteredView.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            ShowTable();
        }

        static void ShowTable()
        {
            Console.Clear();
            Console.WriteLine("#  | First Name   | Last Name       | Phone          | Email");
            Console.WriteLine("------------------------------------------------------------------");
            for (int i = 0; i < filteredView.Count; i++)
            {
                Console.WriteLine($"{(i + 1).ToString().PadRight(2)} | {filteredView[i]}");
            }
            Console.WriteLine("\n[Press Enter to return to menu]");
            Console.ReadLine();
        }


        static void SeedData()
        {
            Random rnd = new Random();
            string GenPhone() => $"787-{rnd.Next(100, 999)}-{rnd.Next(1000, 9999)}";

            string myPhone = "787-000-0000";
            allContacts.Add(new Contact { FirstName = "Luisander", LastName = "Arroyo Rivera", Phone = myPhone, Email = "larroyo@gmail.com" });

            allContacts.Add(new Contact { FirstName = "Carlos", LastName = "Perez", Phone = GenPhone(), Email = "cperez88@gmail.com" });
            allContacts.Add(new Contact { FirstName = "Maria", LastName = "Santiago", Phone = GenPhone(), Email = "msantiago@outlook.com" });
            
            allContacts.Add(new Contact { FirstName = "Wanda", LastName = "Rivera", Phone = GenPhone(), Email = "wanda100@gmail.com" }); 
            
            string sharedPhone = GenPhone();
            allContacts.Add(new Contact { FirstName = "Elena", LastName = "Medina", Phone = sharedPhone, Email = "emedina@yahoo.com" });
            allContacts.Add(new Contact { FirstName = "Israel", LastName = "Ortiz", Phone = sharedPhone, Email = "israel71@icloud.com" });

            allContacts.Add(new Contact { FirstName = "Jose", LastName = "Ortiz", Phone = GenPhone(), Email = "jortiz_@gmail.com" });
            allContacts.Add(new Contact { FirstName = "Sofia", LastName = "Rivera", Phone = GenPhone(), Email = "srivera@gmail.com" });
        }
    }
}