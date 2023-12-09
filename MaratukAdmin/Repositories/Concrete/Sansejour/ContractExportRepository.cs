using MaratukAdmin.Dto.Request.Sansejour;
using MaratukAdmin.Dto.Response;
using MaratukAdmin.Entities.Sansejour;
using MaratukAdmin.Infrastructure;
using MaratukAdmin.Repositories.Abstract.Sansejour;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing.Printing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System;
using System.Collections.Generic;
using Faker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using MaratukAdmin.Dto.Response.Sansejour;

namespace MaratukAdmin.Repositories.Concrete.Sansejour
{
    public class ContractExportRepository : IContractExportRepository
    {
        protected readonly MaratukDbContext _dbContext;

        public ContractExportRepository(MaratukDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<bool> DeleteSyncedDataByDateAsync(DateTime exportDate)
        {
            bool deleteResult;
            try
            {
                //Contracts
                deleteResult = await DeleteSyncSejourContractsByDateAsync(exportDate);
                if (!deleteResult)
                { return false; }
                // Hotels
                //deleteResult = await DeleteSyncSejourHotelsByDateAsync(exportDate);
                deleteResult = await DeleteSyncSejourHotelsByDateRAWAsync(exportDate);
                if (!deleteResult)
                { return false; }
                // AppOrders
                deleteResult = await DeleteSyncSejourSpoAppOrdersByDateRAWAsync(exportDate);
                if (!deleteResult)
                { return false; }
                // Offers
                deleteResult = await DeleteSyncSejourSpecialOffersByDateRAWAsync(exportDate);
                if (!deleteResult)
                { return false; }
                //// Rates
                deleteResult = await DeleteSyncSejourRatesByDateRAWAsync(exportDate);
                if (!deleteResult)
                { return false; }
                //// ChildAges
                //deleteResult = await DeleteSyncSejourChildAgesByDateRAWAsync(exportDate);
                //if (!deleteResult)
                //{ return false; }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<List<SyncSejourContractExportView>> GetSyncSejourContractsByDateAsync(DateTime exportDate)
        {
            try
            {
                return await _dbContext.SyncSejourContractExportView.Where(c => c.ExportDate == exportDate).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSyncSejourContractsByDateAsync(DateTime exportDate)
        {
            var entity = await GetSyncSejourContractsByDateAsync(exportDate);
            if (entity != null)
            {
                _dbContext.RemoveRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task AddlNewSejourContractsAsync(SyncSejourContractExportView contract)
        {
            try
            {
                await _dbContext.SyncSejourContractExportView.AddAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region *** SyncSejourHotel ***
        public async Task<List<SyncSejourHotel>> GetSyncSejourHotelsByDateAsync(DateTime syncDate, string? hotelCode = null)
        {
            try
            {
                return await _dbContext.SyncSejourHotel.Where(c => c.SyncDate == syncDate && c.HotelCode == (hotelCode ?? c.HotelCode)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSyncSejourHotelsByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            var entity = await GetSyncSejourHotelsByDateAsync(exportDate, hotelCode);
            if (entity != null)
            {
                _dbContext.RemoveRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteSyncSejourHotelsByDateRAWAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                string sqlQuery = @$"DELETE FROM {nameof(SyncSejourHotel)} 
                                    WHERE {nameof(SyncSejourHotel.SyncDate)} = '{exportDate}'
                                    AND {nameof(SyncSejourHotel.HotelCode)} = " + ((hotelCode == null)
                                                                                ? nameof(SyncSejourHotel.HotelCode)
                                                                                : "'" + hotelCode + "'");
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task AddNewSejourHotelsAsync(SyncSejourHotel syncHotel)
        {
            try
            {
                await _dbContext.SyncSejourHotel.AddAsync(syncHotel);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region *** SyncSejourSpoAppOrder ***
        public async Task<List<SyncSejourSpoAppOrder>> GetSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                return await _dbContext.SyncSejourSpoAppOrder.Where(c => c.SyncDate == exportDate && c.HotelCode == (hotelCode ?? c.HotelCode)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSyncSejourSpoAppOrdersByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            var entity = await GetSyncSejourSpoAppOrdersByDateAsync(exportDate, hotelCode);
            if (entity != null)
            {
                _dbContext.RemoveRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteSyncSejourSpoAppOrdersByDateRAWAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                string sqlQuery = @$"DELETE FROM {nameof(SyncSejourSpoAppOrder)} 
                                    WHERE {nameof(SyncSejourSpoAppOrder.SyncDate)} = '{exportDate}'
                                    AND {nameof(SyncSejourSpoAppOrder.HotelCode)} = " + ((hotelCode == null)
                                                                                ? nameof(SyncSejourSpoAppOrder.HotelCode)
                                                                                : "'" + hotelCode + "'");
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task AddNewSejourSpoAppOrdersAsync(List<SyncSejourSpoAppOrder> contract)
        {
            try
            {
                await _dbContext.SyncSejourSpoAppOrder.AddRangeAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region *** SyncSejourSpecialOffer ***
        public async Task<List<SyncSejourSpecialOffer>> GetSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                return await _dbContext.SyncSejourSpecialOffer.Where(c => c.SyncDate == exportDate && c.HotelCode == (hotelCode ?? c.HotelCode)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSyncSejourSpecialOffersByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            var entity = await GetSyncSejourSpecialOffersByDateAsync(exportDate, hotelCode);
            if (entity != null)
            {
                _dbContext.RemoveRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteSyncSejourSpecialOffersByDateRAWAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                string sqlQuery = @$"DELETE FROM {nameof(SyncSejourSpecialOffer)} 
                                    WHERE {nameof(SyncSejourSpecialOffer.SyncDate)} = '{exportDate}'
                                    AND {nameof(SyncSejourSpecialOffer.HotelCode)} = " + ((hotelCode == null)
                                                                                ? nameof(SyncSejourSpecialOffer.HotelCode)
                                                                                : "'" + hotelCode + "'");
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task AddNewSejourSpecialOffersAsync(List<SyncSejourSpecialOffer> contract)
        {
            try
            {
                await _dbContext.SyncSejourSpecialOffer.AddRangeAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region *** SyncSejourRate ***
        public async Task<List<SyncSejourRate>> GetSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                return await _dbContext.SyncSejourRate.Where(c => c.SyncDate == exportDate && c.HotelCode == (hotelCode ?? c.HotelCode)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteSyncSejourRatesByDateAsync(DateTime exportDate, string? hotelCode = null)
        {
            var entity = await GetSyncSejourRatesByDateAsync(exportDate, hotelCode);
            if (entity != null)
            {
                _dbContext.RemoveRange(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteSyncSejourRatesByDateRAWAsync(DateTime exportDate, string? hotelCode = null)
        {
            try
            {
                string sqlQuery = @$"DELETE FROM {nameof(SyncSejourRate)} 
                                    WHERE {nameof(SyncSejourRate.SyncDate)} = '{exportDate}'
                                    AND {nameof(SyncSejourRate.HotelCode)} = " + ((hotelCode == null)
                                                                                ? nameof(SyncSejourRate.HotelCode)
                                                                                : "'" + hotelCode + "'");
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task AddNewSejourRatesAsync(List<SyncSejourRate> contract)
        {
            try
            {
                await _dbContext.SyncSejourRate.AddRangeAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region *** SyncSejourAccomodationTypes ***
        public async Task<List<SyncSejourAccomodationType>> GetSyncSejourAccomodationTypeByCodeAsync(string? code = null)
        {
            try
            {
                return await _dbContext.SyncSejourAccomodationType.Where(c => c.Code == ((code == null) ? c.Code : code)).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HashSet<string>> GetHashSyncSejourAccomodationTypeByCodeAsync(string? code = null)
        {
            try
            {
                //return await _dbContext.SyncSejourAccomodationTypes.Where(c => c.Code == ((code == null) ? c.Code : code)).ToListAsync();
                //return await new HashSet<string>(_dbContext.SyncSejourAccomodationTypes.Select(r => $"{r.Code}-{r.Name}"));
                var existingRecords = await Task.FromResult(await _dbContext.SyncSejourAccomodationType
                    .Where(c => c.Code == ((code == null) ? c.Code : code))
                    .Select(r => $"{r.Code}-{r.Name}")
                    .ToListAsync());

                return existingRecords.ToHashSet();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<SyncSejourAccomodationType>> GetSyncSejourAccomodationTypesFromRatesBySyncDateAsync(DateTime syncDate)
        {
            try
            {
                //return await _dbContext.SyncSejourRate.Where(c => c.SyncDate == syncDate)
                //    .Select(accmdType => new SyncSejourAccomodationTypes()
                //    {
                //        Code = accmdType.AccmdMenTypeCode ?? "",
                //        Name = accmdType.AccmdMenTypeName ?? "",
                //        Description = "",
                //        IsDeleted = false
                //    })
                //    .ToListAsync();

                return await _dbContext.SyncSejourRate
                            .Where(r => r.SyncDate == syncDate && r.RoomChdPax > 0)
                            .OrderBy(r => r.AccmdMenTypeCode)
                            .Select(r => new SyncSejourAccomodationType()
                            {
                                Code = r.AccmdMenTypeCode ?? "",
                                Name = r.AccmdMenTypeName ?? ""
                            })
                            .Distinct()
                            .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddNewSyncSejourAccomodationTypesAsync(List<SyncSejourAccomodationType> accmdTypes)
        {
            try
            {
                var existingRecords = await GetHashSyncSejourAccomodationTypeByCodeAsync();

                foreach (var accmdRow in accmdTypes)
                {
                    string accmdMenTypeCode = accmdRow.Code;
                    string accmdMenTypeName = accmdRow.Name;

                    string combinedKey = $"{accmdMenTypeCode}-{accmdMenTypeName}";

                    // Check if record is unique
                    if (!existingRecords.Contains(combinedKey))
                    {
                        SyncSejourAccomodationType newRecord = new()
                        {
                            Code = accmdMenTypeCode,
                            Name = accmdMenTypeName,
                            Description = "",
                            IsDeleted = false,
                            IsDescribed = false,
                            UpdateDate = DateTime.Now
                        };

                        await _dbContext.SyncSejourAccomodationType.AddAsync(newRecord);
                        existingRecords.Add(combinedKey);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteSyncSejourAccomodationTypeByCodeAsync(string code)
        {
            var entity = await GetSyncSejourAccomodationTypeByCodeAsync(code);
            if (entity != null || entity.Count == 1)
            {
                //_dbContext.RemoveRange(entity);
                entity.FirstOrDefault().IsDeleted = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteSyncSejourAccomodationTypeByCodeRAWAsync(string code)
        {
            try
            {
                //string sqlQuery = @$"DELETE FROM {nameof(SyncSejourAccomodationTypes)} 
                //                    WHERE {nameof(SyncSejourAccomodationTypes.SyncDate)} = '{exportDate}'
                //                    AND {nameof(SyncSejourAccomodationTypes.HotelCode)} = " + ((hotelCode == null)
                //                                                                ? nameof(SyncSejourAccomodationTypes.HotelCode)
                //                                                                : "'" + hotelCode + "'");

                string sqlQuery = @$"UPATE {nameof(SyncSejourAccomodationType)} 
                                    SET {nameof(SyncSejourAccomodationType.IsDeleted)} = 1
                                    WHERE {nameof(SyncSejourAccomodationType.Code)} = " + code;
                await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task AddNewSejourChildAgesAsync(List<SyncSejourAccomodationType> contract)
        {
            try
            {
                await _dbContext.SyncSejourAccomodationType.AddRangeAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddNewSejourChildAgesAsync(SyncSejourAccomodationType contract)
        {
            try
            {
                await _dbContext.SyncSejourAccomodationType.AddAsync(contract);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region *** AccomodationDescription ***
        public async Task<List<SyncSejourAccomodationDescription>> GetSyncSejourAccomodationDescriptionAsync(string? code = null)
        {
            try
            {
                //return await _dbContext.SyncSejourAccomodationDescription
                //                .Where(c => c.Code == ((code == null) ? c.Code : code))
                //                .ToListAsync();

                IQueryable<SyncSejourAccomodationDescription> query = _dbContext.SyncSejourAccomodationDescription;

                if (code != null)
                {
                    query = query.Where(entity => entity.Code == code);
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddNewSyncSejourAccomodationDescriptionsAsync(List<SyncSejourAccomodationDescription> accmdList)
        {
            try
            {
                await _dbContext.SyncSejourAccomodationDescription.AddRangeAsync(accmdList);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(int, int)> DescribeListOfAccomodationTypesAsync(List<SyncSejourAccomodationType> accmdTypeList)
        {
            int totalaccmdDescrCount = 0;
            int describedAccmdDescrCount = 0;
            try
            {
                if (accmdTypeList == null || accmdTypeList.Count == 0)
                { totalaccmdDescrCount = 0; describedAccmdDescrCount = 0; }
                else
                {
                    // Get full list of already described AccomodationTypes
                    List<SyncSejourAccomodationDescription> accmdDescrList = await GetSyncSejourAccomodationDescriptionAsync();

                    foreach (var accmdType in accmdTypeList)
                    {
                        //if (accmdDescrList != null && accmdDescrList.Count > 0)
                        //{
                        // Find current Code
                        var accmdDescr = accmdDescrList.Find(a => a.Code == accmdType.Code);
                        // Current Code wasn't described
                        if (accmdDescr == null)
                        {
                            // Describe current AccomodationType
                            List<SyncSejourAccomodationDescription> describedAccmd = DescribeAccomodationType(accmdType.Code);
                            describedAccmd.ForEach(item =>
                            {
                                item.Code = accmdType.Code;
                                item.Name = accmdType.Name;
                            });

                            await AddNewSyncSejourAccomodationDescriptionsAsync(describedAccmd);

                            describedAccmdDescrCount++;
                        }

                        totalaccmdDescrCount++;
                        //}
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return await Task.FromResult((totalaccmdDescrCount, describedAccmdDescrCount));
        }

        public List<SyncSejourAccomodationDescription> DescribeAccomodationType(string input)
        {
            List<SyncSejourAccomodationDescription> ageRanges = new();
            try
            {
                if (string.IsNullOrEmpty(input) || input.Length < 4)
                { return ageRanges; }

                int adultCount = int.Parse(input.Substring(0, 2));
                int childrenCount = int.Parse(input.Substring(2, 2));
                int ageStartIndex = 4;

                if (input.Length >= ageStartIndex + childrenCount * 4)
                {
                    for (int i = 0; i < childrenCount; i++)
                    {
                        double minAge = double.Parse(input.Substring(ageStartIndex, 2));
                        double maxAge = double.Parse(input.Substring(ageStartIndex + 2, 2));
                        ageRanges.Add(new SyncSejourAccomodationDescription()
                        {
                            ChMinAge = minAge,
                            ChMaxAge = maxAge
                        });

                        ageStartIndex += 4;
                    }
                }
            }
            catch (Exception)
            {
                ageRanges = new();
            }

            return ageRanges;
        }
        #endregion

        #region *** SEARCH ***
        public async Task<DateTime?> GetMaxSyncDateAsync()
        {
            try
            {
                return await Task.FromResult(_dbContext.SyncSejourContractExportView.Max(e => e.ExportDate));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime? GetMaxSyncDate()
        {
            try
            {
                return _dbContext.SyncSejourContractExportView.Max(e => e.ExportDate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SyncSejourRate>> SearchRoomOldAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                DateTime? exportDate = (searchRequest.ExportDate == null) ? await GetMaxSyncDateAsync() : searchRequest.ExportDate;
                DateTime? accomodationDateFrom = searchRequest.AccomodationDateFrom;
                DateTime? accomodationDateTo = searchRequest.AccomodationDateTo;

                int roomPax = searchRequest.TotalCount;
                int adultPax = searchRequest.AdultCount;
                int? childPax = searchRequest.ChildCount;
                double? childAge1 = (searchRequest.ChildAges != null && searchRequest.ChildAges.Count > 0) ? searchRequest.ChildAges[0] : null;
                double? childAge2 = (searchRequest.ChildAges != null && searchRequest.ChildAges.Count > 1) ? searchRequest.ChildAges[1] : null;
                double? childAge3 = (searchRequest.ChildAges != null && searchRequest.ChildAges.Count > 2) ? searchRequest.ChildAges[2] : null;
                double? childAge4 = (searchRequest.ChildAges != null && searchRequest.ChildAges.Count > 3) ? searchRequest.ChildAges[3] : null;
                double? childAge5 = (searchRequest.ChildAges != null && searchRequest.ChildAges.Count > 4) ? searchRequest.ChildAges[4] : null;


                int pageNumber = searchRequest.PageNumber;
                int pageSize = searchRequest.PageSize;


                var distinctAccomodationDescriptions = _dbContext.SyncSejourAccomodationDescription
                    .Where(accomodationDescription =>
                        (childAge1 == null || (childAge1 >= accomodationDescription.ChMinAge && childAge1 <= accomodationDescription.ChMaxAge)) &&
                        (childAge2 == null || (childAge2 >= accomodationDescription.ChMinAge && childAge2 <= accomodationDescription.ChMaxAge)) &&
                        (childAge3 == null || (childAge3 >= accomodationDescription.ChMinAge && childAge3 <= accomodationDescription.ChMaxAge)) &&
                        (childAge4 == null || (childAge4 >= accomodationDescription.ChMinAge && childAge4 <= accomodationDescription.ChMaxAge)) &&
                        (childAge5 == null || (childAge5 >= accomodationDescription.ChMinAge && childAge5 <= accomodationDescription.ChMaxAge))
                    )
                    .Select(accomodationDescription => new { accomodationDescription.Code, accomodationDescription.Name })
                    .Distinct();

                var q1 = distinctAccomodationDescriptions.ToList();

                var query = _dbContext.SyncSejourRate
                    .Join(distinctAccomodationDescriptions,
                        rate => rate.AccmdMenTypeCode,
                        accomodationDescription => accomodationDescription.Code,
                        (rate, accomodationDescription) => new { Rate = rate, AccomodationDescription = accomodationDescription })
                    .Where(joinResult =>
                        joinResult.Rate.SyncDate == exportDate &&
                        joinResult.Rate.RoomPax == roomPax &&
                        joinResult.Rate.RoomAdlPax == adultPax &&
                        joinResult.Rate.RoomChdPax == childPax &&
                        joinResult.Rate.AccomodationPeriodBegin >= accomodationDateFrom &&
                        joinResult.Rate.AccomodationPeriodEnd <= accomodationDateTo)
                      //.Select(joinResult => joinResult.Rate);
                      .OrderBy(joinResult => joinResult.Rate.Id)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(joinResult => joinResult.Rate);


                return await query.ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SyncSejourRate>> SearchRoomAsync(SearchRoomRequest searchRequest)
        //public List<SyncSejourAccomodationDescription> SearchRoomNewAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                int pageNumber = searchRequest.PageNumber;
                int pageSize = searchRequest.PageSize;

                DateTime? exportDate = (searchRequest.ExportDate == null) ? await GetMaxSyncDateAsync() : searchRequest.ExportDate;
                DateTime? accomodationDateFrom = searchRequest.AccomodationDateFrom;
                DateTime? accomodationDateTo = searchRequest.AccomodationDateTo;
                int roomPax = searchRequest.TotalCount;
                int adultPax = searchRequest.AdultCount;
                int? childPax = searchRequest.ChildCount;

                float? childAge1 = searchRequest.ChildAges.Count > 0 ? searchRequest.ChildAges[0] : null;
                float? childAge2 = searchRequest.ChildAges.Count > 1 ? searchRequest.ChildAges[1] : null;
                float? childAge3 = searchRequest.ChildAges.Count > 2 ? searchRequest.ChildAges[2] : null;
                float? childAge4 = searchRequest.ChildAges.Count > 3 ? searchRequest.ChildAges[3] : null;
                float? childAge5 = searchRequest.ChildAges.Count > 4 ? searchRequest.ChildAges[4] : null;

                string hotelCodes = searchRequest.HotelCodes != null ? string.Join(",", searchRequest.HotelCodes) : string.Empty;

                //var results = await _dbContext.SyncSejourRate.FromSqlRaw("EXEC dbo.Sp_Search_Room " +
                //                                                    "@exportDate, @accomodationDateFrom, @accomodationDateTo, " +
                //                                                    "@roomPax, @adultPax, @childPax, " +
                //                                                    "@childAge1, @childAge2, @childAge3, @childAge4, @childAge5,"
                //                                                    , exportDate, accomodationDateFrom, accomodationDateTo,
                //                                                    roomPax, adultPax, childPax,
                //                                                    childAge1, childAge2, childAge3, childAge4, childAge5).ToListAsync();

                var result = await _dbContext.SyncSejourRate.FromSqlRaw("EXEC dbo.Sp_Search_Room " +
                                                                    "@exportDate, @accomodationDateFrom, @accomodationDateTo, " +
                                                                    "@roomPax, @adultPax, @childPax, " +
                                                                    "@childAge1, @childAge2, @childAge3, @childAge4, @childAge5, " +
                                                                    "@hotelCodes, @pageNumber, @pageSize",
                                                                    new SqlParameter("exportDate", exportDate),
                                                                    new SqlParameter("accomodationDateFrom", accomodationDateFrom),
                                                                    new SqlParameter("accomodationDateTo", accomodationDateTo),
                                                                    new SqlParameter("roomPax", roomPax),
                                                                    new SqlParameter("adultPax", adultPax),
                                                                    new SqlParameter("childPax", childPax),
                                                                    new SqlParameter("childAge1", childAge1 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge2", childAge2 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge3", childAge3 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge4", childAge4 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge5", childAge5 ?? (object)DBNull.Value),
                                                                    new SqlParameter("hotelCodes", hotelCodes),
                                                                    new SqlParameter("pageNumber", pageNumber),
                                                                    new SqlParameter("pageSize", pageSize))
                                                                    .ToListAsync();
                //var result = query
                //            .OrderBy(rate => rate.Id)
                //            .Skip((pageNumber - 1) * pageSize)
                //            .Take(pageSize)
                //            .ToList();

                return result;

                /*
                List<float?>? childAges = searchRequest.ChildAges;

                var childTable = new List<float?>();
                childTable.AddRange(childAges);

                int neededChildsTotal = childTable.Count(c => c.HasValue);

                var childQuery =
                    from childAge in childTable
                    where childAge != null
                    group childAge by childAge into grouped
                    select new
                    {
                        ChildAge = grouped.Key,
                        NeededChildCount = grouped.Count()
                    };

                var accDescrQuery =
                    from n in _dbContext.SyncSejourAccomodationDescription
                    group n by new { n.Code, n.ChildAge } into grouped
                    select new
                    {
                        Code = grouped.Key.Code,
                        ChildAge = grouped.Key.ChildAge,
                        FoundChildCount = grouped.Count()
                    };

                var accDescrList = accDescrQuery.ToList();
                var childList = childQuery.ToList();

                var result =
                    from CH in childQuery
                    where CH.ChildAge.HasValue
                    group CH by CH.ChildAge into CHGroup
                    join N in accDescrQuery
                        on CHGroup.Key equals N.ChildAge
                    where N.FoundChildCount >= CHGroup.Count() && CHGroup.Count() > 0
                        select new 
                        {
                            ChildAge = CHGroup.Key,
                            NeededChildCount = CHGroup.Count(),
                            FoundChildCount = N.FoundChildCount
                        };

                var childs = new List<float?>();

                childs = searchRequest.ChildAges;

                var res =
                    from childAge in childQuery
                    where childAge != null
                    group childAge by childAge into grouped
                    select new
                    {
                        ChildAge = grouped.Key,
                        NeededChildCount = grouped.Count()
                    };

                var query = from child in childQuery
                            where child.ChildAge.HasValue
                            join accomodation in _dbContext.SyncSejourAccomodationDescription
                                on child.ChildAge equals accomodation.ChildAge
                            join rate in _dbContext.SyncSejourRate
                                on accomodation.Code equals rate.AccmdMenTypeCode
                            where rate.SyncDate == (_dbContext.SyncSejourContractExportView.Max(e => e.ExportDate) ?? DateTime.Now)
                                  && rate.RoomChdPax == neededChildsTotal
                            group new { child, accomodation, rate } by new { accomodation.Code, rate.SyncDate} into grouped
                            let sumNeededChildCount = grouped.Sum(x => x.child.ChildAge != null ? 1 : 0)
                            let sumFoundChildCount = grouped.Sum(x => x.accomodation.ChildAge != null ? 1 : 0)
                            where grouped.Count() == neededChildsTotal || sumFoundChildCount == neededChildsTotal
                            select new SyncSejourRate
                            {
                                 AccmdMenTypeCode = grouped.Key.Code,
                                 SyncDate = grouped.Key.SyncDate,
                                 Price = grouped(x => x.r.Price)

                                //SumNeededChildCount = sumNeededChildCount,
                                //SumFoundChildCount = sumFoundChildCount
                            };

                var groupedResult =
                    from item in result
                    join n in _dbContext.SyncSejourAccomodationDescription
                    on item.ChildAge equals n.ChildAge
                    where item.NeededChildCount > 0 && item.NeededChildCount <= 5
                    join r in (
                        from distinctRate in _dbContext.SyncSejourRate
                        where distinctRate.SyncDate == (exportDate ?? _dbContext.SyncSejourContractExportView.Max(se => se.ExportDate))
                            && distinctRate.RoomChdPax == neededChildsTotal
                        select distinctRate
                    )
                    on n.Code equals r.AccmdMenTypeCode
                    group new { item, n, r } by new { n.Code, r.SyncDate } into grouped
                    let sumNeededChildCount = grouped.Sum(x => x.item.NeededChildCount)
                    let sumFoundChildCount = grouped.Sum(x => x.item.FoundChildCount)
                    where grouped.Count() == neededChildsTotal || sumFoundChildCount == neededChildsTotal
                    select new SyncSejourRate
                    {
                        AccmdMenTypeCode = grouped.Key.Code,
                        SyncDate = grouped.Key.SyncDate,
                        //SumNeededChildCount = sumNeededChildCount,
                        //SumFoundChildCount = sumFoundChildCount,
                        Price = grouped.Key(x => x. r.Price) // Add Price from SyncSejourRate
                    };
                */

                //select new
                //{
                //    SyncDate = exportDate,
                //    Code,
                //    SumNeededChildCount,
                //    SumFoundChildCount
                //};

                /*
                var accDescrList = accDescrQuery.ToList();
                var childList = childQuery.ToList();

                //var childsAndDescr =
                //    from n in accDescrList
                //    join ch in childList
                //    on n.ChildAge equals ch.ChildAge
                //    select new { n, ch };
                var childsAndDescr =
                    from n in accDescrQuery
                    join ch in childQuery
                    on n.ChildAge equals ch.ChildAge
                    select new { n, ch };

                var distinctRates =
                    from r in _dbContext.SyncSejourRate
                    where r.SyncDate == exportDate
                            &&  r.RoomChdPax == neededChildsTotal
                    select new 
                    {
                        SyncDate = exportDate,
                        AccmdMenTypeCode = r.AccmdMenTypeCode,
                        RoomChdPax = r.RoomChdPax
                    };

                var preGroupResult =
                    from chd in childsAndDescr
                    join dr in distinctRates
                        on chd.n.Code equals dr.AccmdMenTypeCode
                    group chd.n by new { chd.n.Code }  into grouped
                    select new 
                    {
                        Code = grouped.Key.Code,
                        SumNeededChildCount = grouped.Count(NeededChildCount)

                    };

                var groupedResult =
                    (from item in childsAndDescr
                     where item.ch.NeededChildCount > 0 && item.ch.NeededChildCount <= item.n.FoundChildCount
                     join rate in _dbContext.SyncSejourRate
                     on item.n.Code equals rate.AccmdMenTypeCode
                     into rateGroup
                     from rate in rateGroup.DefaultIfEmpty()  // Perform a left join
                     where rate != null
                     group new { item.n, item.ch, rate } by new { item.n.Code, rate.SyncDate } into grouped
                     let sumNeededChildCount = grouped.Sum(x => x.ch.NeededChildCount)
                     let sumFoundChildCount = grouped.Sum(x => x.n.FoundChildCount)
                     where grouped.Count() == childTable.Count || sumFoundChildCount == childTable.Count
                     select new SyncSejourRate()
                     {
                         AccmdMenTypeCode = grouped.Key.Code,
                         SyncDate = grouped.Key.SyncDate,
                         //SumNeededChildCount = sumNeededChildCount,
                         //SumFoundChildCount = sumFoundChildCount
                     })
                     .OrderBy(rate => rate.Id)
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();
                */

                //(from item in childsAndDescr
                // where item.ch.NeededChildCount > 0 && item.ch.NeededChildCount <= item.n.FoundChildCount
                // join rate in _dbContext.SyncSejourRate
                // on item.n.Code equals rate.AccmdMenTypeCode
                // into rateGroup
                // from rate in rateGroup.DefaultIfEmpty()  // Perform a left join
                // where rate != null
                // group new { item.n, item.ch, rate } by new { item.n.Code, rate.SyncDate } into grouped
                // let sumNeededChildCount = grouped.Sum(x => x.ch.NeededChildCount)
                // let sumFoundChildCount = grouped.Sum(x => x.n.FoundChildCount)
                // where grouped.Count() == childTable.Count || sumFoundChildCount == childTable.Count
                // select new SyncSejourRate()
                // {
                //     AccmdMenTypeCode = grouped.Key.Code,
                //     SyncDate = grouped.Key.SyncDate,
                //     //SumNeededChildCount = sumNeededChildCount,
                //     //SumFoundChildCount = sumFoundChildCount
                // })
                // .OrderBy(rate => rate.Id)
                // .Skip((pageNumber - 1) * pageSize)
                // .Take(pageSize)
                // .ToList();


                //.OrderBy(joinResult => joinResult.Rate.Id)
                //.Skip((pageNumber - 1) * pageSize)
                //.Take(pageSize)
                //.Select(joinResult => joinResult.Rate);
                /*
                                var result1 =
                                    (
                                from item in result
                                where item.ch.NeededChildCount > 0 && item.ch.NeededChildCount <= item.n.FoundChildCount
                                group item by item.n.Code into grouped
                                let sumNeededChildCount = grouped.Sum(x => x.ch.NeededChildCount)
                                let sumFoundChildCount = grouped.Sum(x => x.n.FoundChildCount)
                                where grouped.Count() == childTable.Count || sumFoundChildCount == childTable.Count
                                select new SyncSejourRate()
                                {
                                    AccmdMenTypeCode = grouped.Key
                                    //Code = grouped.Key,
                                    //SumNeededChildCount = sumNeededChildCount,
                                    //SumFoundChildCount = sumFoundChildCount
                                }
                                    ).ToList();
                                    //(
                                    //from child in childQuery
                                    //join n in nQuery
                                    //on child.ChildAge equals n.ChildAge
                                    //where child.NeededChildCount > 0 && child.NeededChildCount <= n.FoundChildCount
                                    // select new SyncSejourRate()
                                    // {
                                    //     AccmdMenTypeCode = n.Code
                                    //     //child.SyncDate,
                                    //     //n.Code
                                    // }
                */

                //return await Task.FromResult(groupedResult);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SyncSejourRate>> SearchRoomLowestPricesAsync(SearchRoomRequest searchRequest)
        //public List<SyncSejourAccomodationDescription> SearchRoomNewAsync(SearchRoomRequest searchRequest)
        {
            try
            {
                int pageNumber = searchRequest.PageNumber;
                int pageSize = searchRequest.PageSize;

                DateTime? exportDate = (searchRequest.ExportDate == null) ? await GetMaxSyncDateAsync() : searchRequest.ExportDate;
                DateTime? accomodationDateFrom = searchRequest.AccomodationDateFrom;
                DateTime? accomodationDateTo = searchRequest.AccomodationDateTo;
                int roomPax = searchRequest.TotalCount;
                int adultPax = searchRequest.AdultCount;
                int? childPax = searchRequest.ChildCount;

                float? childAge1 = searchRequest.ChildAges.Count > 0 ? searchRequest.ChildAges[0] : null;
                float? childAge2 = searchRequest.ChildAges.Count > 1 ? searchRequest.ChildAges[1] : null;
                float? childAge3 = searchRequest.ChildAges.Count > 2 ? searchRequest.ChildAges[2] : null;
                float? childAge4 = searchRequest.ChildAges.Count > 3 ? searchRequest.ChildAges[3] : null;
                float? childAge5 = searchRequest.ChildAges.Count > 4 ? searchRequest.ChildAges[4] : null;

                string hotelCodes = searchRequest.HotelCodes != null ? string.Join(",", searchRequest.HotelCodes) : string.Empty;

                var result = await _dbContext.SyncSejourRate.FromSqlRaw("EXEC dbo.Sp_Search_Room_LowestPrices " +
                                                                    "@exportDate, @accomodationDateFrom, @accomodationDateTo, " +
                                                                    "@roomPax, @adultPax, @childPax, " +
                                                                    "@childAge1, @childAge2, @childAge3, @childAge4, @childAge5, " +
                                                                    "@hotelCodes, @pageNumber, @pageSize",
                                                                    new SqlParameter("exportDate", exportDate),
                                                                    new SqlParameter("accomodationDateFrom", accomodationDateFrom),
                                                                    new SqlParameter("accomodationDateTo", accomodationDateTo),
                                                                    new SqlParameter("roomPax", roomPax),
                                                                    new SqlParameter("adultPax", adultPax),
                                                                    new SqlParameter("childPax", childPax),
                                                                    new SqlParameter("childAge1", childAge1 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge2", childAge2 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge3", childAge3 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge4", childAge4 ?? (object)DBNull.Value),
                                                                    new SqlParameter("childAge5", childAge5 ?? (object)DBNull.Value),
                                                                    new SqlParameter("hotelCodes", hotelCodes),
                                                                    new SqlParameter("pageNumber", pageNumber),
                                                                    new SqlParameter("pageSize", pageSize))
                                                                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SyncSejourRate>> SearchRoomLowestPricesMockAsync(SearchFligtAndRoomRequest searchFligtAndRoomRequest)
        {
            List<SyncSejourRate> retValue = GenerateFakeRooms(searchFligtAndRoomRequest);

            return await Task.FromResult(retValue);
        }

        private List<SyncSejourRate> GenerateFakeRooms(SearchFligtAndRoomRequest searchFligtAndRoomRequest)
        {
            var fakeRooms = new List<SyncSejourRate>();

            try
            {
                // Generate 10 fake records
                for (int i = 1; i <= 10; i++)
                {
                    var fakeFlight = new SyncSejourRate
                    {
                        SyncDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        HotelCode = Faker.StringFaker.Numeric(3),
                        HotelSeasonBegin = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom, 
                        HotelSeasonEnd = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateTo, 
                        RecID = Faker.StringFaker.Numeric(8),
                        CreateDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom, 
                        ChangeDate = (DateTime)searchFligtAndRoomRequest.RoomAccomodationDateFrom, 
                        AccomodationPeriodBegin = searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        AccomodationPeriodEnd = searchFligtAndRoomRequest.RoomAccomodationDateFrom,
                        Room = Faker.StringFaker.Alpha(3),
                        RoomDesc = Faker.Lorem.Words(1).FirstOrDefault(),
                        RoomType = Faker.StringFaker.Alpha(3),
                        RoomTypeDesc = "STANDARD",
                        Board = "AI",
                        BoardDesc = "ALL INCLUSIVE",
                        RoomPax = searchFligtAndRoomRequest.RoomAdultCount + searchFligtAndRoomRequest.RoomChildCount,
                        RoomAdlPax = searchFligtAndRoomRequest.RoomAdultCount,
                        RoomChdPax = searchFligtAndRoomRequest.RoomChildCount,
                        AccmdMenTypeCode = Faker.StringFaker.Numeric(12),
                        AccmdMenTypeName = searchFligtAndRoomRequest.RoomAdultCount.ToString() + "Ad + " + searchFligtAndRoomRequest.RoomChildCount.ToString() + "Ch(Mek)(Erku)(Ereq)",
                        ReleaseDay = 0,
                        PriceType = "ROOM",
                        Price = Faker.NumberFaker.Number(1, 5000),
                        WeekendPrice = null,
                        WeekendPercent = 0,
                        AccomLengthDay = "0-999",
                        Option = "Stay",
                        SpoNoApply = null,
                        SPOPrices = 2,
                        SPODefinit = "EEA// 04/08/2023 M",
                        NotCountExcludingAccomDate = "N"
                    };

                    fakeRooms.Add(fakeFlight);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return fakeRooms;
        }
        #endregion
    }
}
