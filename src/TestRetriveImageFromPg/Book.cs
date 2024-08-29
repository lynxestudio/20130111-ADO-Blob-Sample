using System;
namespace TestRetriveImageFromPg
{
	public class Book{
		public int BookId{set;get;}
		public string Title{ set; get;}
		public short PublisherYear {set;get;}
		public string ImagePath {set;get;}
		public byte[] ImageBytes {set;get;}
	}
}

