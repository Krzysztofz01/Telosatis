using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Telosatis.Api.LargeFileTransfer;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class LargeFileTransferAttribute : Attribute, IResourceFilter, IOrderedFilter
{
    // NOTE: Ensuring that the rules win over the [RequestSizeLimit] and [RequestFormLimits]
    public int Order => 1000;
 
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var sizeFeature = context.HttpContext.Features.Get<IHttpMaxRequestBodySizeFeature>();
        if (sizeFeature is { IsReadOnly: false })
            sizeFeature.MaxRequestBodySize = LargeFileTransferSettings.MaxUploadBytes;
 
        var formFeature = context.HttpContext.Features.Get<IFormFeature>();
        if (formFeature?.Form is null)
        {
            var formOptions = new FormOptions
            {
                MultipartBodyLengthLimit = LargeFileTransferSettings.MaxUploadBytes,
                MemoryBufferThreshold = LargeFileTransferSettings.FormMemoryBufferThreshold,
                ValueCountLimit = 16,
            };
            
            context.HttpContext.Features.Set<IFormFeature>(new FormFeature(
                context.HttpContext.Request,
                formOptions));
        }
    }
 
    public void OnResourceExecuted(ResourceExecutedContext context) { }
}
