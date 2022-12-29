using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Core.Web.Extensions
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this IHtmlHelper html, string area, string controller = null, string action = null, string cssClass = null)
        {
            if (!string.IsNullOrWhiteSpace(area))
                area = area.ToLower();

            if (!string.IsNullOrWhiteSpace(controller))
                controller = controller.ToLower();

            if (!string.IsNullOrWhiteSpace(action))
                action = action.ToLower();

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentArea = (string)html.ViewContext.RouteData.DataTokens["area"];
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            currentArea = currentArea?.ToLower();
            currentAction = currentAction?.ToLower();
            currentController = currentController?.ToLower();


            if (String.IsNullOrEmpty(area))
                area = currentArea;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return area == currentArea && controller == currentController && action == currentAction ? cssClass : String.Empty;
        }

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (!string.IsNullOrWhiteSpace(controller))
                controller = controller.ToLower();

            if (!string.IsNullOrWhiteSpace(action))
                action = action.ToLower();

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            currentAction = currentAction?.ToLower();
            currentController = currentController?.ToLower();

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this IHtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];

            currentAction = currentAction?.ToLower();

            return currentAction;
        }

        public static JsonResult DataTablesJson<T>(this Controller controller, IEnumerable<T> items, int totalRecords, int totalDisplayRecords, int sEcho)
        {
            var data = new JQueryDataTablesResponse<T>(items ?? new List<T> { }, totalRecords, totalDisplayRecords, sEcho);

            var result = new JsonResult(data);

            return result;
        }

        public static string CheckBoxList(this IHtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo)
        {
            return htmlHelper.CheckBoxList(name, listInfo, null);
        }

        public static string CheckBoxList(this IHtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo, object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(name, listInfo, new RouteValueDictionary(htmlAttributes));
        }

        public static string CheckBoxList(this IHtmlHelper htmlHelper, string name, List<CheckBoxListInfo> listInfo, IDictionary<string, object> htmlAttributes)
        {
            if (listInfo.Count < 1)
                throw new ArgumentException("The list must contain at least one value", nameof(listInfo));

            StringBuilder sb = new StringBuilder();
            foreach (CheckBoxListInfo info in listInfo)
            {
                TagBuilder builder = new TagBuilder("input");

                if (info.IsChecked) builder.MergeAttribute("checked", "checked");
                //builder.InnerHtml = info.DisplayText;
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);

                sb.Append("<div class = " + "col-sm-3" + ">");
                sb.Append("<div class = " + "form-group" + ">");
                sb.Append("<label class=" + "control - label" + ">" + info.DisplayText + "</label>");
                sb.Append("<div class = " + "input-group" + ">");
                sb.Append(builder.ToString());
                sb.Append("</div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }

            return sb.ToString();
        }
    }
}
