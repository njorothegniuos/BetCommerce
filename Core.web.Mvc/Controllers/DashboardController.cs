using Core.web.Mvc.DTOs.Response;
using Core.web.Mvc.Identity;
using Core.web.Mvc.Models;
using Core.Web.DTOs.Request;
using Core.Web.DTOs.Response;
using Core.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.web.Mvc.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Dashboard(JQueryDataTablesModel jQueryDataTablesModel)
        {
            int totalRecordCount = 0;

            PageCollectionInfo<ShoppingCartResponse> _pageCollectionInfo = new PageCollectionInfo<ShoppingCartResponse>();

            //ToDo: make API call Refactor:use HttpRequestAppService
            using (var pclient = new HttpClient())
            {
                var api_url = _configuration["WebApi:WebApiClientUrl"];
                api_url = api_url + "v1/product/list";

                pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await pclient.GetAsync(api_url);

                var res = await response.Content.ReadAsStringAsync();

                res = HttpUtility.HtmlDecode(res);

                var responseData = JsonConvert.DeserializeObject<List<ShoppingCartResponse>>(res, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (responseData != null)
                {
                    _pageCollectionInfo.ItemsCount = responseData.Count;
                    _pageCollectionInfo.PageCollection = responseData;
                }
            }

            if (_pageCollectionInfo != null && _pageCollectionInfo.ItemsCount > 0)
            {
                totalRecordCount = _pageCollectionInfo.ItemsCount;

                return this.DataTablesJson(_pageCollectionInfo.PageCollection, totalRecordCount, jQueryDataTablesModel.iDisplayLength, jQueryDataTablesModel.sEcho);
            }
            return this.DataTablesJson(new List<ProductDTO>(), totalRecordCount, jQueryDataTablesModel.iDisplayLength, jQueryDataTablesModel.sEcho);
        }

        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cart(JQueryDataTablesModel jQueryDataTablesModel)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            int totalRecordCount = 0;

            PageCollectionInfo<ProductDTO> _pageCollectionInfo = new PageCollectionInfo<ProductDTO>();

            //ToDo: make API call Refactor:use HttpRequestAppService
            using (var pclient = new HttpClient())
            {
                var api_url = _configuration["WebApi:WebApiClientUrl"];
                api_url = api_url + $"v1/shoppingCart/shoppingCartByUserId/{currentUser.Id}";

                pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await pclient.GetAsync(api_url);

                var res = await response.Content.ReadAsStringAsync();

                res = HttpUtility.HtmlDecode(res);

                var responseData = JsonConvert.DeserializeObject<List<ProductDTO>>(res, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (responseData != null)
                {
                    _pageCollectionInfo.ItemsCount = responseData.Count;
                    _pageCollectionInfo.PageCollection = responseData;
                }
            }

            if (_pageCollectionInfo != null && _pageCollectionInfo.ItemsCount > 0)
            {
                totalRecordCount = _pageCollectionInfo.ItemsCount;

                return this.DataTablesJson(_pageCollectionInfo.PageCollection, totalRecordCount, jQueryDataTablesModel.iDisplayLength, jQueryDataTablesModel.sEcho);
            }
            return this.DataTablesJson(new List<ProductDTO>(), totalRecordCount, jQueryDataTablesModel.iDisplayLength, jQueryDataTablesModel.sEcho);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(Guid id)
        {
            var responseData = new ProductDTO();

            //ToDo: make API call Refactor:use HttpRequestAppService
            using (var pclient = new HttpClient())
            {
                var api_url = _configuration["WebApi:WebApiClientUrl"];
                api_url = api_url + $"v1/product/{id}";

                pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await pclient.GetAsync(api_url);

                var res = await response.Content.ReadAsStringAsync();

                res = HttpUtility.HtmlDecode(res);

                responseData = JsonConvert.DeserializeObject<ProductDTO>(res, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return PartialView("_productDetails", responseData);
            }
        }
        [HttpPost]

        [HttpGet]
        public async Task<IActionResult> CartProductDetails(Guid id)
        {
            var responseData = new ProductDTO();

            //ToDo: make API call Refactor:use HttpRequestAppService
            using (var pclient = new HttpClient())
            {
                var api_url = _configuration["WebApi:WebApiClientUrl"];
                api_url = api_url + $"v1/shoppingCart/shoppingCartById/{id}";

                pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await pclient.GetAsync(api_url);

                var res = await response.Content.ReadAsStringAsync();

                res = HttpUtility.HtmlDecode(res);

                responseData = JsonConvert.DeserializeObject<ProductDTO>(res, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return PartialView("_cartProductDetails", responseData);
            }
        }

        public async Task<IActionResult> AddToCart(Guid id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id !=Guid.Empty)
            {
                //ToDo: make API call Refactor:use HttpRequestAppService
                using (var pclient = new HttpClient())
                {
                    var api_url = _configuration["WebApi:WebApiClientUrl"];
                    api_url = api_url + "v1/shoppingCart/create";

                    pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                    CreateShoppingCartRequest request = new CreateShoppingCartRequest(new Guid(currentUser.Id),id, 1);

                    var data = JsonConvert.SerializeObject(request);
                    var content = new StringContent(data, Encoding.UTF8, "application/json"); ;

                    var response = await pclient.PostAsync(api_url, content);

                    var res = await response.Content.ReadAsStringAsync();

                    res = HttpUtility.HtmlDecode(res);

                    var responseData = JsonConvert.DeserializeObject<Guid>(res);
                }

                return Json("Item added to cart");
            }
            return Json("");
        }

        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (id != Guid.Empty)
            {
                //ToDo: make API call Refactor:use HttpRequestAppService
                using (var pclient = new HttpClient())
                {
                    var api_url = _configuration["WebApi:WebApiClientUrl"];
                    api_url = api_url + "v1/shoppingCart/removeFromShoppingCartById";

                    pclient.DefaultRequestHeaders.Add("Accept", "application/json");

                    DeleteFromShoppingCartRequest request = new DeleteFromShoppingCartRequest(id,new Guid(currentUser.Id), id, 1);

                    var data = JsonConvert.SerializeObject(request);
                    var content = new StringContent(data, Encoding.UTF8, "application/json"); ;

                    var response = await pclient.PostAsync(api_url, content);

                    var res = await response.Content.ReadAsStringAsync();

                    res = HttpUtility.HtmlDecode(res);

                    var responseData = JsonConvert.DeserializeObject<Guid>(res);
                }

                return Json("Item removed from cart");
            }
            return Json("");
        }
    }
}
