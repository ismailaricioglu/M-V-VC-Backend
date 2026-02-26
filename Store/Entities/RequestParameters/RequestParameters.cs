namespace Entities.RequestParameters
{
    /// <summary>
    /// API veya veri sorgulama iþlemleri için kullanýlan, arama terimini (SearchTerm) taþýyan soyut temel sýnýftýr.
    /// Türetilen sýnýflar, filtreleme ve veri çekme iþlemlerinde bu parametreyi kullanarak arama yapýlmasýný saðlar.
    /// </summary>
    public abstract class RequestParameters
    {
        public String? SearchTerm { get; set; }
    }

}