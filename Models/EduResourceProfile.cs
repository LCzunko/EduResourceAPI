using AutoMapper;
using EduResourceAPI.Models.DTOs;
using EduResourceAPI.Models.Entities;

namespace EduResourceAPI.Models
{
    public class EduResourceProfile : Profile
    {
        public EduResourceProfile()
        {
            #region Author maps
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<Author, AuthorReadDTO>();
            #endregion

            #region Category maps
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<Category, CategoryReadDTO>();
            #endregion

            #region Material maps
            CreateMap<MaterialCreateDTO, Material>();
            CreateMap<Material, MaterialReadDTO>();
            #endregion

            #region Review maps
            CreateMap<ReviewCreateDTO, Review>();
            CreateMap<Review, ReviewReadDTO>();
            #endregion
        }
    }
}
