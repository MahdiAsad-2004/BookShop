//using BookShop.Domain.Entities;
//using BookShop.Domain.Identity;
//using BookShop.Domain.IRepositories;
//using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

//namespace BookShop.Infrastructure.Identity
//{
//    internal sealed class UserStore : IUserStore<User>,
//                            IUserPasswordStore<User>,
//                            IUserSecurityStampStore<User>,
//                            IUserEmailStore<User>,
//                            IUserPhoneNumberStore<User>,
//                            IUserTwoFactorStore<User>,
//                            IUserLockoutStore<User>,
//                            IUserAuthenticationTokenStore<User>,
//                            IUserAuthenticatorKeyStore<User>,
//                            IUserTwoFactorRecoveryCodeStore<User>

//    {
//        #region contructor

//        private readonly ICurrentUser _currentUser;
//        private readonly IUserRepository _userRepository;
//        private readonly IRefreshTokenRepository _userTokenRepository;
//        public UserStore(IUserRepository userRepository, ICurrentUser currentUser, IRefreshTokenRepository userTokenRepository)
//        {
//            _userRepository = userRepository;
//            _currentUser = currentUser;
//            _userTokenRepository = userTokenRepository;
//        }


//        #endregion





//        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
//        {
//            user.PasswordHistories = new List<PasswordHistory>()
//            {
//                new PasswordHistory
//                {
//                    PasswordHash = user.PasswordHash,
//                    CreateDate = DateTime.UtcNow,
//                    LastModifiedDate = DateTime.UtcNow,
//                    CreateBy = _currentUser.GetId(),
//                    LastModifiedBy = _currentUser.GetId(),
//                },
//            };
//            await _userRepository.Add(user);
//            return IdentityResult.Success;
//        }

//        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
//        {
//            await _userRepository.Delete(user);
//            return IdentityResult.Success;
//        }

//        public void Dispose()
//        {
//        }

//        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
//        {
//            return  await _userRepository.GetByNormalizedEmail(normalizedEmail);
//        }

//        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
//        {
//            return await _userRepository.Get(userId);
//        }

//        public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
//        {
//            return await _userRepository.GetByNormalizedUsernameOrDefault(normalizedUserName);
//        }

//        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.AccessFailedCount);
//        }

//        public Task<string?> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.Email);
//        }

//        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.EmailConfirmed);
//        }

//        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.LockoutEnabled);
//        }

//        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.LockoutEnd);
//        }

//        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.NormalizedEmail);
//        }

//        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.NormalizedUsername);
//        }

//        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.PasswordHash);
//        }

//        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.PhoneNumber);
//        }

//        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.PhoneNumberConfirmed);
//        }

//        public Task<string?> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.SecurityStamp);
//        }

//        public Task<string?> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
//        {
//            string? tokenValue = user.UserTokens
//                .FirstOrDefault(a => a.TokenName == name && a.LoginProvider == loginProvider)?.TokenValue;
//            return Task.FromResult(tokenValue);
//        }

//        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.TwoFactorEnabled);
//        }

//        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.Id.ToString());
//        }

//        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(user.Username);
//        }

//        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
//        {
//            return Task.FromResult(string.IsNullOrEmpty(user.PasswordHash) == false);
//        }

//        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
//        {
//            user.AccessFailedCount++;
//            return Task.FromResult(user.AccessFailedCount);
//        }

//        public Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
//        {
//            RefreshToken? userToken = await _userTokenRepository.GetOrDefaultAsync(name , loginProvider);
            
//            if(userToken != null)
//            {
//                await _userTokenRepository.Delete(userToken);
//            }
//        }

//        public Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }

//        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
//        {
//            user.AccessFailedCount = 0;
//            return Task.CompletedTask;
//        }

//        public Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }

//        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
//        {
//            user.Email = email;
//            return Task.CompletedTask;
//        }

//        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
//        {
//            user.EmailConfirmed = confirmed;
//            return Task.CompletedTask;
//        }

//        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
//        {
//            user.LockoutEnabled = enabled;
//            return Task.CompletedTask;
//        }

//        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
//        {
//            user.LockoutEnd = lockoutEnd;
//            return Task.CompletedTask;
//        }

//        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
//        {
//            user.NormalizedEmail = normalizedEmail;
//            return Task.CompletedTask;
//        }

//        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
//        {
//            user.NormalizedUsername = normalizedName;
//            return Task.CompletedTask;
//        }

//        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
//        {
//            user.PasswordHash = passwordHash;
//            return Task.CompletedTask;
//        }

//        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
//        {
//            user.PhoneNumber = phoneNumber;
//            return Task.CompletedTask;
//        }

//        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
//        {
//            user.PhoneNumberConfirmed = confirmed;
//            return Task.CompletedTask;
//        }

//        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
//        {
//            user.SecurityStamp = stamp;
//            return Task.CompletedTask;
//        }

//        public async Task SetTokenAsync(User user, string loginProvider, string name, string? value, CancellationToken cancellationToken)
//        {
//            RefreshToken? userToken = await _userTokenRepository.GetOrDefaultAsync(name, loginProvider);
            
//            if(userToken == null)
//            {
//                await _userTokenRepository.Add(new RefreshToken
//                {
//                    LoginProvider = loginProvider,
//                    TokenName = name,
//                    TokenValue = value,
//                    UserId = user.Id,
//                });
//            }
//            else
//            {
//                userToken.TokenValue = value;
//                await _userTokenRepository.Update(userToken);  
//            }
//        }

//        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
//        {
//            user.TwoFactorEnabled = enabled;
//            return Task.CompletedTask;
//        }

//        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
//        {
//            user.Username = userName;
//            return Task.CompletedTask;
//        }

//        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
//        {
//            await _userRepository.Update(user);
//            return IdentityResult.Success;
//        }


//        public Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
//        {
//            throw new NotImplementedException();
//        }


//    }
//}
