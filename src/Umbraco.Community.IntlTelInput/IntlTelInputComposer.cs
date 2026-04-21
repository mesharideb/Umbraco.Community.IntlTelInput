using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace Umbraco.Community.IntlTelInput;

public sealed class IntlTelInputComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
        => builder.WithCollectionBuilder<FieldCollectionBuilder>().Add<IntlTelInputField>();
}
