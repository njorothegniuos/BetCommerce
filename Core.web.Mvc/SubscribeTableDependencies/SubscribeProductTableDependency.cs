using Core.web.Mvc.Models;
using Core.Web.Hubs;
using TableDependency.SqlClient;

namespace Core.Web.SubscribeTableDependencies
{
    public class SubscribeProductTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<ShoppingCarts> tableDependency;
        CartHub dashboardHub;

        public SubscribeProductTableDependency(CartHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<ShoppingCarts>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<ShoppingCarts> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                dashboardHub.SendCartCountAndSum();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Serilog.Log.Error($"{nameof(Products)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
