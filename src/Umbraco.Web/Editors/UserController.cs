﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using AutoMapper;
using Umbraco.Core.Configuration;
using Umbraco.Web.Models.ContentEditing;
using Umbraco.Web.Models.Mapping;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using legacyUser = umbraco.BusinessLogic.User;
using System.Net.Http;
using System.Collections.Specialized;
using Constants = Umbraco.Core.Constants;


namespace Umbraco.Web.Editors
{
    /// <summary>
    /// Controller to back the User.Resource service, used for fetching user data when already authenticated. user.service is currently used for handling authentication
    /// </summary>
    [PluginController("UmbracoApi")]
    public class UserController : UmbracoAuthorizedJsonController
    {

        /// <summary>
        /// Returns the configuration for the backoffice user membership provider - used to configure the change password dialog
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetMembershipProviderConfig()
        {
            var provider = Membership.Providers[UmbracoConfig.For.UmbracoSettings().Providers.DefaultBackOfficeUserProvider];
            if (provider == null)
            {
                throw new InvalidOperationException("No back office membership provider found with the name " + UmbracoConfig.For.UmbracoSettings().Providers.DefaultBackOfficeUserProvider);
            }
            return provider.GetConfiguration();
        } 

        /// <summary>
        /// Returns a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDetail GetById(int id)
        {
            var user = Services.UserService.GetUserById(id);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<UserDetail>(user);
        }

        /// <summary>
        /// Changes the users password
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// If the password is being reset it will return the newly reset password, otherwise will return null;
        /// </returns>
        public string PostChangePassword(ChangingPasswordModel data)
        {
            var userProvider = Membership.Providers[UmbracoConfig.For.UmbracoSettings().Providers.DefaultBackOfficeUserProvider];
            if (userProvider == null)
            {
                throw new InvalidOperationException("No membership provider found with the name " + UmbracoConfig.For.UmbracoSettings().Providers.DefaultBackOfficeUserProvider);
            }

            //TODO: WE need to support this! - requires UI updates, etc...
            if (userProvider.RequiresQuestionAndAnswer)
            {
                throw new NotSupportedException("Currently the user editor does not support providers that have RequiresQuestionAndAnswer specified");
            }

            var passwordChangeResult = Security.ChangePassword(Security.CurrentUser.Username, data, userProvider);
            if (passwordChangeResult.Success)
            {
                //even if we weren't resetting this, it is the correct value (null), otherwise if we were resetting then it will contain the new pword
                return passwordChangeResult.Result.ResetPassword;
            }

            //it wasn't successful, so add the change error to the model state
            ModelState.AddPropertyError(
                passwordChangeResult.Result.ChangeError,
                string.Format("{0}password", Constants.PropertyEditors.InternalGenericPropertiesPrefix));

            throw new HttpResponseException(Request.CreateValidationErrorResponse(ModelState));
        }

        /// <summary>
        /// Returns all active users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserBasic> GetAll()
        {
            return Services.UserService.GetAllUsers().Where(x => x.IsLockedOut == false).Select(Mapper.Map<UserBasic>);
        }
    }
}