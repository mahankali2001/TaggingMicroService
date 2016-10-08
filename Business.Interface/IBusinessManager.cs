using System;
namespace Business.Interface
{
    public interface IBusinessManager : IDisposable
    {
        ITaggingBusiness GetTaggingBusiness();
    }
}
