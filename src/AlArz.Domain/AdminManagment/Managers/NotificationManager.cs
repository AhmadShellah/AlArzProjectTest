using AlArz.Localization;
using Application.Contracts.EntityDto;
using Application.Exceptions;
using Domain.Entity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class NotificationManager : DomainService
    {
        private readonly IRepository<Notification, Guid> _NotificationsRepository;
        private readonly IRepository<Config, Guid> _config;
        private readonly IStringLocalizer<AlArzResource> _localizer;
        public NotificationManager
            (
            IRepository<Notification, Guid> NotificationsRepository,
            IRepository<Config, Guid> config,
            IStringLocalizer<AlArzResource> localizer
            )
        {
            _NotificationsRepository = NotificationsRepository;
            _config = config;
            _localizer = localizer;
        }

        public async Task<List<Notification>> GetNotificationsToBeSent()
        {
            return await _NotificationsRepository.GetListAsync(c => !c.IsSent && c.TryCount < 3);
        }

        public async Task<Notification> Create(Notification notification)
        {
            try
            {
                GuidGenerator.Create();

                var result = await _NotificationsRepository.InsertAsync(notification);

                return result;
            }
            catch (Exception)
            {
                throw new NotCreatedException();
            }

        }

        public async Task<List<Notification>> GetNotificationByUserId(Guid userId)
        {
            return await _NotificationsRepository.GetListAsync(c => c.User.HasValue && c.User.Value == userId
            && c.TryCount <= 3 && c.IsSent == false);
        }

        public async Task UpdateSendNotification(Notification notifications)
        {
            await _NotificationsRepository.UpdateAsync(notifications);
        }

        public async Task<Notification> GetAsync(Guid id)
        {
            var result = await _NotificationsRepository.GetAsync(id);
            if (result == null)
            {
                throw new NotFoundException();
            }
            return result;
        }

        public async Task CreateMany(List<Notification> notifications)
        {
            try
            {
                GuidGenerator.Create();

                await _NotificationsRepository.InsertManyAsync(notifications);
            }
            catch (Exception)
            {
                throw new NotCreatedException();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _NotificationsRepository.DeleteAsync(id);
        }

        public async Task<bool> UpdateIsReadToNotification(Guid id)
        {
            var resultFromDataBase = await GetAsync(id);

            resultFromDataBase.IsRead = true;

            return resultFromDataBase.IsRead;
        }

        public async Task<int> GetIsNotReadCount()
        {
            return await _NotificationsRepository.CountAsync(c => !c.IsRead && c.IsSent);
        }

        public async Task<PagedResultDto<Notification>> GetByFilter(PagedSortedRequestDto input)
        {
            Expression<Func<Notification, bool>> expression = s => s.IsSent == true;
            var qResult = (await _NotificationsRepository.GetQueryableAsync()).Where(expression);


            var result = new PagedResultDto<Notification>();

            result.TotalCount = qResult.Count();


            if (input != null)
            {
                if (input.Sorting != null)
                {
                    Expression<Func<Notification, object>> sorting = s => "";

                    if (input.Sorting.ToLower().Equals("body"))
                    {
                        sorting = s => s.Body.ToString();
                    }

                    if (input.Sorting.ToLower().Equals("name"))
                    {
                        sorting = s => s.Name;
                    }
                    if (input.Sorting.ToLower().Equals("title"))
                    {
                        sorting = s => s.Title;
                    }
                    if (input.Sorting.ToLower().Equals("recordid"))
                    {
                        sorting = s => s.RecordId;
                    }

                    if (input.SortDescending == true)
                    {
                        qResult = qResult.OrderByDescending(sorting);
                    }

                    else
                        qResult = qResult.OrderBy(sorting);

                }

                result.Items = qResult
                                      .Skip(input.SkipCount)
                                      .Take(input.MaxResultCount)
                                      .ToList();

            }

            return result;
        }


        //public async Task<List<Guid>> GetUsersByAuthorityCodes(List<string> AuthorityCodes, bool IsAll = false)
        //{
        //    try
        //    {
        //        var resultFromDataBase = (await _useb n rPlace.GetQueryableAsync())
        //        .Where(x =>                                                //IsAll =true return all local comm 
        //        (!IsAll && AuthorityCodes.Contains(x.AuthorityCode)) || (IsAll && !string.IsNullOrEmpty(x.AuthorityCode)));
        //        if (resultFromDataBase.Any())
        //        {
        //            return resultFromDataBase.Select(x => x.UserId).ToList();
        //        }
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.BadRequest);
        //    }
        //}


        //public async Task<List<Guid>> GetUsersByPlaceIDs(List<Guid> placeIDs, bool IsAll = false)
        //{
        //    try
        //    {
        //        Expression<Func<UserPlace, bool>> expression = s => true;
        //        expression.And(x => (!IsAll && placeIDs.Contains(x.PlaceId)) || (IsAll));

        //        var resultFromDataBase = (await _userPlace.GetQueryableAsync()).Where(expression);
        //        if (resultFromDataBase.Any())
        //        {
        //            return resultFromDataBase.Select(x => x.UserId).ToList();
        //        }

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.BadRequest);
        //    }
        //}

        ////SendNotification using json 
        ////When you want to send a Notification Please Follow up
        ////Go To Json File Called Notification.json the path= Baladiyat.Domain.Shared
        ////in the notifications array please add your Notification
        ////Please When You Want To Send a Notification Using Managers Classes
        //public async Task SendNotifiaction(string name, Guid? role, List<Guid> users, Guid? user, object obj)
        //{
        //    try
        //    {
        //        //var notification = new Notification();

        //        var item = await PrepareNotification(name, obj);
        //        if (item == null)
        //            throw new BusinessException(BaladiyatDomainErrorCodesAdmin.NotificationIsNotCreated);
        //        //await MappingManualAsync(notification, item, role);
        //        var lookupFromDataBase = await _lookupService.GetLookupByCode((int)ModuleTypeLookup.NotificationEvent, (int)LookupTypes.ModuleType);
        //        if (lookupFromDataBase == null)
        //            throw new BusinessException(BaladiyatDomainErrorCodesAdmin.LookupNotFound);
        //        item.TypeId = lookupFromDataBase.Id;
        //        if (users != null && users.Any())
        //        {
        //            await AddUser(users, item);
        //            return;
        //        }

        //        await Create(item);
        //    }

        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.NotificationIsNotCreated);
        //    }

        //}

        //public async Task SendNotifiactionByUsers(string name, List<Guid> placesId, List<string> codes, object obj, bool IsAll = false)
        //{
        //    try
        //    {
        //        var item = await PrepareNotification(name, obj);
        //        if (item == null)
        //        {
        //            throw new BusinessException(BaladiyatDomainErrorCodesAdmin.NotificationIsNotCreated);
        //        }

        //        var lstOfUsers = new List<Guid>();

        //        if (placesId != null && placesId.Count() > 0)
        //        {
        //            lstOfUsers = await GetUsersByPlaceIDs(placesId, IsAll);
        //        }

        //        else
        //            lstOfUsers = await GetUsersByAuthorityCodes(codes, IsAll);


        //        if (lstOfUsers != null && lstOfUsers.Any())
        //        {
        //            await AddUser(lstOfUsers, item);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //#region  ALL private Methods Perpare Notification 

        //private async Task MappingManualAsync(Notification notification, Notification item, Guid? role)
        //{
        //    if (notification == null && item == null)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.NotificationIsNotCreated);
        //    }
        //    var lookupFromDataBase = await _lookupService.GetLookupByCode((int)LookupTypes.NotificationEvent, (int)ModuleTypeLookup.NotificationEvent);
        //    if (lookupFromDataBase == null)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.LookupNotFound);
        //    }

        //    notification.Body = item.Body;
        //    notification.Name = item.Name;
        //    notification.IsSent = item.IsSent;
        //    notification.Image = item.Image;
        //    notification.Title = item.Title;
        //    notification.TryCount = 0;
        //    notification.Event = item.Event;
        //    notification.IsRead = item.IsRead;
        //    notification.RecordId = item.RecordId;
        //    if (role != null)
        //    {
        //        notification.Role = role.Value;
        //    }
        //    if (lookupFromDataBase != null)
        //    {
        //        notification.TypeId = lookupFromDataBase.Id;
        //    }

        //}

        //private async Task<string> GetFilePathByConfig()
        //{
        //    try
        //    {

        //        const string DoaminSharedFromApplication = "Baladiyat.Domain.Shared";
        //        const string FileNameFromApplication = "Notification.json";

        //        string pathFileFromApplication = Environment.CurrentDirectory;

        //        pathFileFromApplication = pathFileFromApplication.Replace("Baladiyat.Web", DoaminSharedFromApplication);

        //        string meargeFullPath = Path.Combine(pathFileFromApplication, FileNameFromApplication);
        //        if (meargeFullPath == null)
        //        {
        //            throw new BusinessException(BaladiyatDomainErrorCodesAdmin.PathFileNotFoundNotification);
        //        }

        //        return meargeFullPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "path from NotificationManager");
        //        throw;
        //    }

        //}

        //private async Task AddUser(List<Guid> users, Notification notification)
        //{
        //    List<Notification> notifications = new List<Notification>();
        //    var _users = users.Distinct().ToList();
        //    foreach (var item in _users)
        //    {
        //        var newerNotification = new Notification();
        //        newerNotification.user = item;
        //        await MappingManualAsync(newerNotification, notification, null);
        //        notifications.Add(newerNotification);
        //    }

        //    await CreateMany(notifications);
        //}

        //private async Task<Notification> PrepareNotification(string name, object obj)
        //{
        //    try
        //    {
        //        var notification = await GetNotificationsJsonFile(name);
        //        if (notification == null)
        //        {
        //            return null;
        //        }
        //        notification.Title = PrepareNotificationText(notification.Title, obj);
        //        notification.Body = PrepareNotificationText(notification.Body, obj);

        //        return notification;
        //    }
        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.BadRequest);
        //    }
        //}

        //private string PrepareNotificationText(string text, Object obj)
        //{
        //    try
        //    {

        //        Dictionary<string, Object> dictionary = GetDictionaryFromObj(obj);
        //        foreach (var key in dictionary.Keys)
        //        {
        //            if (dictionary[key] != null)
        //            {

        //                if (dictionary[key].Equals(BaladiyatDomainErrorCodesAdmin.CreateNotification))
        //                {
        //                    dictionary[key] = _localizer[BaladiyatDomainErrorCodesAdmin.CreateNotification, "#" + key + "#"];
        //                }
        //                else if (dictionary[key].Equals(BaladiyatDomainErrorCodesAdmin.UpdateNotification))
        //                {
        //                    dictionary[key] = _localizer[BaladiyatDomainErrorCodesAdmin.UpdateNotification, "#" + key + "#"];
        //                }

        //                text = text.Replace("#" + key + "#", dictionary[key].ToString());

        //            }
        //        }
        //        return text;
        //    }
        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.BadRequest);
        //    }
        //}

        //private Dictionary<string, object> GetDictionaryFromObj(object obj)
        //{
        //    try
        //    {
        //        var dic = obj.GetType()
        //                     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        //                     .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));

        //        return dic;
        //    }
        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.BadRequest);
        //    }
        //}

        //private async Task<Notification> GetNotificationsJsonFile(string name)
        //{
        //    try
        //    {
        //        string PathFileFromConfig = await GetFilePathByConfig();

        //        string pathFile = $"{PathFileFromConfig}";

        //        using (StreamReader readFromPath = new StreamReader(pathFile))
        //        {
        //            string jsonFile = readFromPath.ReadToEnd();

        //            var allItemasInJsonFile = JsonSerializer.Deserialize<Root>(jsonFile);

        //            var allItems = allItemasInJsonFile.notifications.FirstOrDefault(x => x.Name.Equals(name));

        //            return allItems;
        //        };
        //    }

        //    catch (Exception)
        //    {
        //        throw new BusinessException(BaladiyatDomainErrorCodesAdmin.PathFileNotFoundNotification);
        //    }
        //}

        //#endregion


    }
}

