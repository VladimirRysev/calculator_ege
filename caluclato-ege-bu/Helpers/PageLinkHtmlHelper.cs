using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc.Core.Common;
using X.PagedList.Web.Common;

namespace Calculator_ege_bu.Helpers
{
    public static class PageLinkHtmlHelper
    {
        public static HtmlString PagerCenter(this IHtmlHelper html, IPagedList pagedList, Func<int, string> generatePageUrl)
        {
            var result = "<div class='text-center'>"+
                "<div class='d-inline-block'>"+
                    html.PagerInLine(pagedList, generatePageUrl)+
                "</div>"+
            "</div>";
            return new HtmlString(result);
        }

        public static HtmlString PagerRight(this IHtmlHelper html, IPagedList pagedList, Func<int, string> generatePageUrl)
        {
            var result = "<div class='d-inline-block float-right'>"+
                html.PagerInLine(pagedList, generatePageUrl)+
            "</div>";
            return new HtmlString(result);
        }
        public static HtmlString PagerInLine(this IHtmlHelper html, IPagedList pagedList, Func<int,string> pageUrl)
        {
             var htmlString = html.PagedListPager(pagedList, pageUrl, new PagedListRenderOptions {
                LiElementClasses = new[] { "page-item" },
                PageClasses = new[] { "page-link" },
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded,
                DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded
            });
            return htmlString;
        }
    }
}
