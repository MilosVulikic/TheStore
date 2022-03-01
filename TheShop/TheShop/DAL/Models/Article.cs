using System;

namespace TheShop.DAL.Models
{
	public class Article
	{
		public int ID { get; set; }
		//public int ArticleId { get; set; }

		public string Name { get; set; }

		public int Price { get; set; }
		public bool IsSold { get; set; }

		public DateTime? SoldDate { get; set; }
		public int BuyerUserId { get; set; }
	}

}
