using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace archivos_secuenciales_indexados
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string indexPath = "index.txt";
            string dataPath = "data.dat";

            // Crear índice si no existe
            if (!File.Exists(indexPath))
            {
                File.Create(indexPath).Close();
            }

            // Menú principal
            while (true)
            {
                Console.WriteLine("1. Agregar empleado");
                Console.WriteLine("2. Buscar empleado por ID");
                Console.WriteLine("3. Mostrar todos los empleados");
                Console.WriteLine("4. Salir");
                Console.Write("Selecciona una opción: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee(dataPath, indexPath);
                        break;
                    case "2":
                        SearchEmployee(dataPath, indexPath);
                        break;
                    case "3":
                        DisplayAllEmployees(dataPath);
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                        break;
                }
            }
        }

        static void AddEmployee(string dataPath, string indexPath)
        {
            Console.Write("Ingrese el ID del empleado: ");
            int employeeId = int.Parse(Console.ReadLine());

            Console.Write("Ingrese el nombre del empleado: ");
            string employeeName = Console.ReadLine();

            Console.Write("Ingrese el salario del empleado: ");
            double employeeSalary = double.Parse(Console.ReadLine());

            using (StreamWriter dataWriter = new StreamWriter(dataPath, true))
            using (StreamWriter indexWriter = new StreamWriter(indexPath, true))
            {
                // Escribir datos en el archivo de datos
                dataWriter.WriteLine($"{employeeId},{employeeName},{employeeSalary}");

                // Escribir índice en el archivo de índice
                indexWriter.WriteLine($"{employeeId},{dataWriter.BaseStream.Length - 1}");
            }

            Console.WriteLine("Empleado agregado con éxito.");
        }

        static void SearchEmployee(string dataPath, string indexPath)
        {
            Console.Write("Ingrese el ID del empleado a buscar: ");
            int searchId = int.Parse(Console.ReadLine());

            string line;

            using (StreamReader indexReader = new StreamReader(indexPath))
            {
                while ((line = indexReader.ReadLine()) != null)
                {
                    string[] indexData = line.Split(',');
                    int employeeId = int.Parse(indexData[0]);
                    long offset = long.Parse(indexData[1]);

                    if (employeeId == searchId)
                    {
                        using (StreamReader dataReader = new StreamReader(dataPath))
                        {
                            dataReader.BaseStream.Seek(offset, SeekOrigin.Begin);
                            string dataLine = dataReader.ReadLine();
                            string[] employeeData = dataLine.Split(',');

                            Console.WriteLine($"ID: {employeeData[0]}, Nombre: {employeeData[1]}, Salario: {employeeData[2]}");
                        }
                        return;
                    }
                }
            }

            Console.WriteLine($"Empleado con ID {searchId} no encontrado.");
        }

        static void DisplayAllEmployees(string dataPath)
        {
            using (StreamReader dataReader = new StreamReader(dataPath))
            {
                string line;
                while ((line = dataReader.ReadLine()) != null)
                {
                    string[] employeeData = line.Split(',');
                    Console.WriteLine($"ID: {employeeData[0]}, Nombre: {employeeData[1]}, Salario: {employeeData[2]}");
                }
            }
        }
    }
    
}
