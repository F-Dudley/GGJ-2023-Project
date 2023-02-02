using System.Threading.Tasks;

namespace Overgrown
{
	namespace Services
	{
		public interface IServiceProvider
		{
			public Task InitializeService();
		}
	}
}
