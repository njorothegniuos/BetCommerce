using Core.web.Mvc.DTOs.Response;
using Dapper;
using Microsoft.Data.SqlClient;
using System;

namespace Core.Web.Repository
{
    public class CartRepository
    {
        string connectionString;
        public CartRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ShoppingCartItemCount GetShoppingCartItemCount()
        {
            var query = $"SELECT COUNT(S.Id) AS ItemCount,SUM(P.Price) AS Total FROM ShoppingCarts S INNER JOIN Products P ON S.ProductId=P.Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var data = connection.QueryFirstAsync<ShoppingCartItemCount>(query).Result;

                    return data;
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error("CartRepository => GetShoppingCartItemCount: " + ex.Message);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }



    }
}
