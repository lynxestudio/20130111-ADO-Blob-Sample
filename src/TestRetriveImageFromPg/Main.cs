using System;

namespace TestRetriveImageFromPg
{
	class MainClass
	{
        static string answer = "y";
		public static void Main (string[] args)
		{
            string[] options = { "Create a new book", "Query a book by Id" };
            string option = null;
            do
            {
                try
                {
                    Console.Clear();
                    Utilities.SetTitle("Program to add blob data type to a PostgreSQL Database");
                    Utilities.DisplayMenu(options);
                    option = Utilities.Scanf("Select an option");
                    switch(option)
                    {
                        case "1":
                            Add();
                            break;
                        case "2":
                            Query();
                            break;
                        default:
                            Console.WriteLine("\tPlease choose an option:");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.PrintLabel("Error",ex.Message);
                }
            } while (!option.Equals("0"));

		}
		
		static void Query()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Utilities.SetTitle("Querying the database");
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(" Querying database .......");
                    BooksManagerDAC.SelectAll().ForEach(b => Console.WriteLine("\t{0}\t{1}",
                                                                               b.BookId, b.Title));
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Retrieve database record.....");
                    int id = Convert.ToInt32(Utilities.Scanf("Id"));
                    string fileName = Utilities.Scanf("Filename");
                    Book myBook = BooksManagerDAC.SelectById(id, fileName);
                    Utilities.PrintLabel("Title", myBook.Title);
                    Utilities.PrintLabel("Year", myBook.PublisherYear.ToString());
                    Console.WriteLine("Done.");
                }
                catch (Exception ex)
                {
                    Utilities.PrintLabel("Error", ex.Message);
                }
                finally
                {
                    answer = Utilities.Scanf("Continue? (y/n)");
                }
                if (answer.ToLower().Equals("n"))
                    break;
            } while (answer.ToLower().Equals("y"));
		}
		
		static void Add()
        {
            do
            {
                try
                {
                    Console.Clear();
                    Utilities.SetTitle("Adding a book ");
                    Book b = new Book
                    {
                        Title = Utilities.Scanf("Title"),
                        PublisherYear = Convert.ToInt16(Utilities.Scanf("Publish year")),
                        ImagePath = Utilities.Scanf("Path to image")
                    };
                    BooksManagerDAC.Create(b);
                    Utilities.PrintMessage("(1) record affacted");
                }
                catch (Exception ex)
                {
                    Utilities.PrintMessage(ex.Message);
                }
                finally
                {
                    answer = Utilities.Scanf("Continue? (y/n)");
                }
                if (answer.ToLower().Equals("n"))
                    break;

            } while (answer.ToLower().Equals("y"));
		}
	}
}

