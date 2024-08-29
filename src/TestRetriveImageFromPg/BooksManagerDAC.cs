using System;
using System.IO;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;

namespace TestRetriveImageFromPg
{
	
	public class BooksManagerDAC
	{
		static string strConnString = "Server=127.0.0.1;Port=5432;Database=BookSample;User ID=martin;Password=Pa$$W0rd";
		public static void Create (Book b)
		{
			string commandText = "Insert into books(title,publishyear,picture)Values(:title,:publishyear,:picture)";
			byte[] bytesFromImage = GetPhoto(b.ImagePath);
			using(NpgsqlConnection conn = GetConnection())
			{
				using(NpgsqlCommand cmd = new NpgsqlCommand(commandText,conn))
				{
					var paramTitle = new NpgsqlParameter("title", NpgsqlDbType.Varchar);
                    paramTitle.SourceColumn = "title";
                    paramTitle.Value = b.Title;
                    cmd.Parameters.Add(paramTitle);
                    
                    var paramPubYear = new NpgsqlParameter("publishyear", NpgsqlDbType.Smallint);
                    paramPubYear.SourceColumn = "publishyear";
                    paramPubYear.Value = b.PublisherYear;
                    cmd.Parameters.Add(paramPubYear);
                    
                    var pPicture = new NpgsqlParameter("picture", NpgsqlDbType.Bytea);
                    pPicture.SourceColumn = "picture";
                    pPicture.Value = bytesFromImage;
                    cmd.Parameters.Add(pPicture);
					int r = cmd.ExecuteNonQuery();
					Console.WriteLine("{0} affected",r);
					}
			}
		}
		
		public static Book SelectById(int id,string fileName){
			string commandText = "SELECT bookid,title,publishyear,picture " +
				" FROM Books WHERE bookid = " + id.ToString();
			Book b = null;
			using(NpgsqlDataReader reader = GetReader(commandText))
			{
				
				int colBookId =	reader.GetOrdinal("bookid");
				int colTitle =	reader.GetOrdinal("title");
				int colPublishYear = reader.GetOrdinal("publishyear");
				int colPicture = reader.GetOrdinal("picture");
				
				while(reader.Read()){
					b = new Book{
						BookId = reader.GetInt32(colBookId),
						Title = reader.GetString(colTitle)
					};
					if(!reader.IsDBNull(colPublishYear))
						b.PublisherYear = reader.GetInt16(colPublishYear);
					if(!reader.IsDBNull(colPicture))
					{
						Console.WriteLine("Retrieving image...");
						RetrieveImage(reader,colPicture,fileName);
					}
				}
			}
			return b;
		}
		
		static void RetrieveImage(NpgsqlDataReader reader,int columnImage,string fileName){
			byte[] result = (byte[])reader.GetValue(columnImage);
			using(FileStream fis = new FileStream(fileName,FileMode.OpenOrCreate,FileAccess.Write))
			{
				using(BinaryWriter writer = new BinaryWriter(fis))
				{
				writer.Write(result);
				writer.Flush();
				}
			}
		}
		
		static byte[] GetPhoto(string filename){
			byte[] photo = null;
			using(FileStream fis = new FileStream(filename,FileMode.Open,FileAccess.Read))
			{
			BinaryReader reader = new BinaryReader(fis);
			 photo = reader.ReadBytes((int)fis.Length);
				reader.Close();
			}
			return photo;
		}
		
		public static List<Book> SelectAll(){
			var resp = new List<Book>();
			Book b = null;
			string commandText = "SELECT bookid,title FROM Books ";
			using(NpgsqlDataReader reader = GetReader(commandText))
			{
				while(reader.Read()){
					b = new Book();
					b.BookId = Convert.ToInt32(reader["bookid"]);
					b.Title = reader["title"].ToString();
					resp.Add(b);
				}
			}
			return resp;
		}
		
		
		static NpgsqlConnection GetConnection(){
			NpgsqlConnection conn = new NpgsqlConnection(strConnString);
			conn.Open();
			return conn;
		}
		
		
		
		static NpgsqlDataReader GetReader(string commandText){
			NpgsqlDataReader resp = null;
				NpgsqlConnection conn = GetConnection();
					using (NpgsqlCommand cmd = new NpgsqlCommand(commandText, conn))
					{
						resp = cmd.ExecuteReader(CommandBehavior.CloseConnection | 
				                         CommandBehavior.SequentialAccess);
					}
				return resp;
		}
		
		
	}
}

