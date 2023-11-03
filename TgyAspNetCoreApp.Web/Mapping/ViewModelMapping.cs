using AutoMapper;
using TgyAspNetCoreApp.Web.Models;
using TgyAspNetCoreApp.Web.ViewModels;

namespace TgyAspNetCoreApp.Web.Mapping
{
    public class ViewModelMapping:Profile
    {
        public ViewModelMapping()
        {
            CreateMap<Product,ProductViewModel>().ReverseMap();
            CreateMap<Product,ProductUpdateViewModel>().ReverseMap();
            CreateMap<Visitor,VisitorViewModel>().ReverseMap();
        }
    }
}
