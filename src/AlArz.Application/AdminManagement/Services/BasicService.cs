using AlArz.Interface;
using AlArz.Interfaces;
using AlArz.Managers;
using AlArz.Shared;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace AdminManagement.Settings
{
    public class BasicService : ApplicationService
    {
        public async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(AdminSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }

        public List<Y> MappingListHandel<T, Y>(List<T> inputs, List<Y> resultFromDataBase)
        {
            if (resultFromDataBase.Count != inputs.Count)
            {
                throw new Exception();
            }

            var list = new List<Y>();

            for (int i = 0; i < inputs.Count; i++)
            {
                list.Add(ObjectMapper.Map(inputs[i], resultFromDataBase[i]));
            }

            return list;
        }

        public void CustomLoggerInformation(string message, Guid userId, object input, object output, string apiName, string controllerName)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            var resultFromUserInput = JsonConvert.SerializeObject(input);

            var resultFromBackEndOutput = JsonConvert.SerializeObject(output);

            Logger.LogInformation($"{message}," + "   " +
                $"{LoggerConsts.InputMessage},{resultFromUserInput}" + "    " +
                $",{LoggerConsts.OutPutMessage},{resultFromBackEndOutput}" + "    " +
                $",{LoggerConsts.APIName},{apiName}," + "    " +
                $"{LoggerConsts.ControllerName},{controllerName}" + "    " +
                $"{LoggerConsts.UserId},{userId}");
        }

    }
}
