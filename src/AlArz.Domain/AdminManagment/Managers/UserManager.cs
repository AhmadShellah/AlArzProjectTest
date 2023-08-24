using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AlArz.Managers
{
    public class UserManager : DomainService
    {
        public readonly IRepository<BlockedUserToken, Guid> _blockRepository;
        public readonly IRepository<RegisterFireBase, Guid> _registerFireBaseRepository;
        public UserManager(IRepository<BlockedUserToken, Guid> blockRepository, IRepository<RegisterFireBase, Guid> registerFireBaseRepository)
        {
            _blockRepository = blockRepository;
            _registerFireBaseRepository = registerFireBaseRepository;
        }
        public async Task CreateBlockedUserTokenAsync(BlockedUserToken blockedUserToken)
        {
            GuidGenerator.Create();
            await _blockRepository.InsertAsync(blockedUserToken);
        }

        public async Task RegisterFireBaseTokenAsync(string token)
        {
            GuidGenerator.Create();
            await _registerFireBaseRepository.InsertAsync(new RegisterFireBase { Token = token });
        }
        public async Task<List<RegisterFireBase>> GetActiveUserTokens(Guid userId)
        {
            return await _registerFireBaseRepository.GetListAsync(c => c.IsDeleted == false && c.CreatorId.HasValue && c.CreatorId.Value == userId);
        }

        public async Task UnRegisterFireBaseToken(string token)
        {
            var tokens = await _registerFireBaseRepository.GetListAsync(c => c.Token.Equals(token));
            tokens.ForEach(c => c.IsDeleted = true);
            await _registerFireBaseRepository.UpdateManyAsync(tokens);
        }

    }
}
