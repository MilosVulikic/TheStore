using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheShop.DAL.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IArticleRepository Articles { get; }
		int Complete();
	}
}
