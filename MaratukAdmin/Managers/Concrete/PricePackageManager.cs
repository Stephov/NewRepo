using AutoMapper;
using MaratukAdmin.Business.Models.Common;
using MaratukAdmin.Dto.Request;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities;
using MaratukAdmin.Entities.Global;
using MaratukAdmin.Exceptions;
using MaratukAdmin.Managers.Abstract;
using MaratukAdmin.Models;
using MaratukAdmin.Repositories.Abstract;
using MaratukAdmin.Repositories.Concrete;
using System.Collections.Generic;

namespace MaratukAdmin.Managers.Concrete
{
    public class PricePackageManager : IPricePackageManager
    {
        private readonly IMainRepository<PricePackage> _mainRepository;
        private readonly IPricePackageRepository _pricePackageRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;


        public PricePackageManager(IMainRepository<PricePackage> mainRepository, IMapper mapper, IPricePackageRepository pricePackageRepository, ICityRepository cityRepository)
        {
            _mainRepository = mainRepository;
            _mapper = mapper;
            _pricePackageRepository = pricePackageRepository;
            _cityRepository = cityRepository;
        }

        public async Task<PricePackage> AddPricePackageAsync(AddPricePackage pricePackage)
        {
            var entity = _mapper.Map<PricePackage>(pricePackage);
            await _mainRepository.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeletePricePackageAsync(int id)
        {
            var entity = await _mainRepository.GetAsync(id);

            if (entity == null)
            {
                throw new ApiBaseException(StatusCodes.Status404NotFound, "Entity not found");
            }

            bool result = await _mainRepository.DeleteAsync(id);
            return result;
        }

        public async Task<List<PricePackageResponse>> GetAllPricePackagesAsync()
        {
            var results = await _mainRepository.GetAllAsync("Country");
            var response = new List<PricePackageResponse>();


            foreach (var key in results)
            {
                response.Add(new PricePackageResponse()
                {
                    Id = key.Id,
                    Name = key.Name,
                    NameEng = key.NameEng,
                    CountryId = key.CountryId,
                    Country = key.Country.NameENG
                });
            }

            return response;
        }

        public async Task<PricePackage> GetPricePackageByIdAsync(int id)
        {
            return await _mainRepository.GetAsync(id);
        }

        public async Task<PricePaskageCountry> GetPricePaskageCountryAsync(int pricePackageId)
        {
            var result = await _pricePackageRepository.GetPricePaskageCountryAsync(pricePackageId);

            var cityResult = await _cityRepository.GetCityByCountryIdAsync(result.CountryId);

            List <PricePackageCity> cityList = new List<PricePackageCity>();


            foreach (var city in cityResult)
            {
                PricePackageCity pricePackageCity = new PricePackageCity();

                pricePackageCity.CityId = city.Id;
                pricePackageCity.Name = city.Name;
                cityList.Add(pricePackageCity); 
            }
            result.Cityes = cityList;

            return result;

        }

        public async Task<PricePackage> UpdatePricePackageAsync(UpdatePricePackage pricePackage)
        {
            var entity = _mapper.Map<PricePackage>(pricePackage);

            await _mainRepository.UpdateAsync(entity);
            return entity;
        }
    }
}
