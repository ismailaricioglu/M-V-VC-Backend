using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace StoreApp.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /// <summary>
            /// Product nesnesini ProductDto nesnesine dönüştürür.
            /// Bu işlem, AutoMapper kütüphanesi kullanılarak gerçekleştirilir.
            /// </summary>
            /// <details>
            /// Product nesnesi, DTO nesnesine dönüştürülür ve döndürülür.
            /// Bu işlem, AutoMapper kütüphanesi kullanılarak gerçekleştirilir.
            /// </details>
            CreateMap<ProductDtoForInsertion, Product>();

            /// <summary>
            /// Product nesnesini ProductDto nesnesine dönüştürür.
            /// Bu işlem, AutoMapper kütüphanesi kullanılarak gerçekleştirilir.
            /// </summary>
            /// <details>
            /// Product nesnesi, DTO nesnesine dönüştürülür ve döndürülür.
            /// Bu işlem, AutoMapper kütüphanesi kullanılarak gerçekleştirilir.
            /// ReverseMap() metodu, iki yönlü dönüşüm sağlar.
            /// Yani, Product nesnesi ProductDto nesnesine ve tam tersine dönüştürülebilir.
            /// </details>
            CreateMap<ProductDtoForUpdate, Product>().ReverseMap();

            /// <summary>
            /// Hesap oluşturma işlemi için <see cref="UserDtoForCreation"/> nesnesini <see cref="IdentityUser"/> nesnesine eşler.
            /// </summary>
            /// <details>
            /// DTO'dan IdentityUser modeline dönüşümü basitleştirmek için AutoMapper kullanılır.
            /// Özellikle kullanıcı kayıt işlemleri sırasında kullanışlıdır; yeni bir kullanıcı sisteme kaydolurken
            /// girilen bilgiler IdentityUser yapısına dönüştürülerek kimlik sistemine kaydedilir.
            /// </details>
            CreateMap<UserDtoForCreation, IdentityUser>();


            /// <summary>
            /// <see cref="UserDtoForUpdate"/> nesnesi ile <see cref="IdentityUser"/> nesnesi arasında çift yönlü eşleme yapılır.
            /// </summary>
            /// <details>
            /// AutoMapper kullanılarak hem güncelleme işlemlerinde IdentityUser'dan DTO'ya,
            /// hem de DTO'dan IdentityUser'a dönüşüm sağlanır. Bu sayede kullanıcı bilgileri hem alınabilir hem de güncellenebilir.
            /// </details>
            CreateMap<UserDtoForUpdate, IdentityUser>().ReverseMap();

        }
    }
}