using Core.Web.DTOs.Request;
using Core.Web.DTOs.Response;
using Core.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Dashboard(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;

            int searchRecordCount = 0;

          //  var sortedColumns = (from s in jQueryDataTablesModel.GetSortedColumns() select s.PropertyName).ToList();

          //  sortedColumns.Add("CreatedDate");
            //ToDo: make API call to get list of products
            var pageCollectionInfo =  new PageCollectionInfo<ProductDTO>();

            if (pageCollectionInfo != null && pageCollectionInfo.PageCollection.Any())
            {
                totalRecordCount = pageCollectionInfo.ItemsCount;

                searchRecordCount = !string.IsNullOrWhiteSpace(jQueryDataTablesModel.sSearch) ? pageCollectionInfo.PageCollection.Count : totalRecordCount;

                return this.DataTablesJson(pageCollectionInfo.PageCollection, totalRecordCount, searchRecordCount, jQueryDataTablesModel.sEcho);
            }

            return this.DataTablesJson(new List<ProductDTO>(), totalRecordCount, searchRecordCount, jQueryDataTablesModel.sEcho);
        }

        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cart(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;

            int searchRecordCount = 0;

            //  var sortedColumns = (from s in jQueryDataTablesModel.GetSortedColumns() select s.PropertyName).ToList();

            //  sortedColumns.Add("CreatedDate");
            //ToDo: make API call to get list of products
            var pageCollectionInfo = new PageCollectionInfo<ProductDTO>();

            if (pageCollectionInfo != null && pageCollectionInfo.PageCollection.Any())
            {
                totalRecordCount = pageCollectionInfo.ItemsCount;

                searchRecordCount = !string.IsNullOrWhiteSpace(jQueryDataTablesModel.sSearch) ? pageCollectionInfo.PageCollection.Count : totalRecordCount;

                return this.DataTablesJson(pageCollectionInfo.PageCollection, totalRecordCount, searchRecordCount, jQueryDataTablesModel.sEcho);
            }

            return this.DataTablesJson(new List<ProductDTO>(), totalRecordCount, searchRecordCount, jQueryDataTablesModel.sEcho);
        }
    }
}
