using Core.Web.Repository;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Core.Web.Hubs
{
    public class CartHub : Hub
    {
        CartRepository cartRepository;
        public CartHub(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            cartRepository = new CartRepository(connectionString);

        }
        public async Task SendCartCountAndSum()
        {
            try
            {
                var cartItem = cartRepository.GetShoppingCartItemCount();
                await Clients.All.SendAsync("ReceivedCartCountAndSum", cartItem);
            }catch(Exception ex)
            {
                Serilog.Log.Error("CartHub =>SendCartCountAndSum: " + ex.Message);
            }


        }
    }
}
