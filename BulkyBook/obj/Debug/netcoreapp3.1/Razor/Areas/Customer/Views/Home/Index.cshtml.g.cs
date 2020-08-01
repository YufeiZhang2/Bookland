#pragma checksum "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd01e48e7d0ec4972a5a327e8bde2e41a5ba9d24"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Customer_Views_Home_Index), @"mvc.1.0.view", @"/Areas/Customer/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/_ViewImports.cshtml"
using BulkyBook;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/_ViewImports.cshtml"
using BulkyBook.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd01e48e7d0ec4972a5a327e8bde2e41a5ba9d24", @"/Areas/Customer/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"978115df24d3f50d13b66a59ae01f12d3d33ebf7", @"/Areas/Customer/Views/_ViewImports.cshtml")]
    public class Areas_Customer_Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BulkyBook.Models.Product>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("background: #F4D07A;color: black"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n<div class=\"row pb-3 backgroundWhite\">\n");
#nullable restore
#line 4 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
     foreach (var product in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"        <div class=""col-lg-3 col-md-6"" >
                <div class=""row p-2"">
                    <div class=""col-12  p-1"" style=""border:1px solid lightgray; border-radius: 5px;"">
                        <div class=""card"">
                            <img");
            BeginWriteAttribute("src", " src=\"", 383, "\"", 406, 1);
#nullable restore
#line 10 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
WriteAttributeValue("", 389, product.ImageUrl, 389, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"card-img-top rounded\" style=\" padding-bottom: 10px; height: 380px\"/>\n                            <div class=\"pl-1\">\n                                <p class=\"card-title h5\"><b style=\"color:#2c3e50\">");
#nullable restore
#line 12 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
                                                                             Write(product.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></p>\n                                <p class=\"card-title text-primary\">by <b>");
#nullable restore
#line 13 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
                                                                    Write(product.Author);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></p>\n                            </div>\n                            <div style=\"padding-left:5px;\">\n                                <p>List Price: <b");
            BeginWriteAttribute("class", " class=\"", 877, "\"", 885, 0);
            EndWriteAttribute();
            WriteLiteral(">$");
#nullable restore
#line 16 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
                                                       Write(product.ListPrice.ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></p>\n                            </div>\n                            <div style=\"padding-left:5px;\">\n                                <p style=\"color:maroon\">As low as: <b");
            BeginWriteAttribute("class", " class=\"", 1096, "\"", 1104, 0);
            EndWriteAttribute();
            WriteLiteral(">$");
#nullable restore
#line 19 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
                                                                           Write(product.Price100.ToString("0.00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</b></p>\n                            </div>\n                        </div>\n                        <div >\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dd01e48e7d0ec4972a5a327e8bde2e41a5ba9d247349", async() => {
                WriteLiteral("\n                                Details\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 23 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
                                                                                           WriteLiteral(product.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n                        </div>\n                    </div>\n                </div>\n            </div>\n");
#nullable restore
#line 30 "/Users/zhangyufei/Documents/repo/BulkyBook/BulkyBook/Areas/Customer/Views/Home/Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BulkyBook.Models.Product>> Html { get; private set; }
    }
}
#pragma warning restore 1591
