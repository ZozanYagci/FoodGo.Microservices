using FoodGo.CatalogService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Domain.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public string Street { get; private set; }
        public string District { get; private set; }
        public string City { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        protected Address()
        {

        }

        public Address(string street, string district, string city, double latitude, double longitude)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Sokak bilgisi boş olamaz.");
            if (string.IsNullOrWhiteSpace(district)) throw new DomainException("İlçe bilgisi boş olamaz.");
            if (string.IsNullOrWhiteSpace(city)) throw new DomainException("Şehir bilgisi boş olamaz.");
            Street = street;
            District = district;
            City = city;
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return District;
            yield return City;
            yield return Latitude;
            yield return Longitude;
        }
    }
}
