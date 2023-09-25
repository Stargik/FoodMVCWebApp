using System;
using FoodMVCWebApp.Entities;
using FoodMVCWebApp.Interfaces;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using MassTransit.Futures.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace FoodMVCWebApp.Services
{
	public class GoogleSheetsService : IGoogleSheetsService
    {
        private readonly SpreadsheetsResource.ValuesResource googleSheetValues;
        private readonly GoogleSheetsSettings googleSheetsSettings;
        public GoogleSheetsService(GoogleSheetsHelper googleSheetsHelper, IOptions<GoogleSheetsSettings> googleSheetsSettings)
		{
            googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
            this.googleSheetsSettings = googleSheetsSettings.Value;
        }

        public async Task AddDish(Dish dish)
        {
            var range = $"{googleSheetsSettings.SheetName}!A:C";
            var valueRange = new ValueRange
            {
                Values = MapToRangeData(dish)
            };
            var appendRequest = googleSheetValues.Append(valueRange, googleSheetsSettings.SheetId, range);
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            await appendRequest.ExecuteAsync();
        }

        private IList<IList<object>> MapToRangeData(Dish dish)
        {
            var objectList = new List<object>() { dish.Id, dish.Title, dish.Recipe};
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}

