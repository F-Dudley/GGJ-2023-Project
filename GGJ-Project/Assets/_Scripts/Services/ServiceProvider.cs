using System.Threading.Tasks;

namespace Overgrown
{
	namespace GameServices
	{
		public interface IServiceProvider
		{
			public Task InitializeService();
		}
	}
}
