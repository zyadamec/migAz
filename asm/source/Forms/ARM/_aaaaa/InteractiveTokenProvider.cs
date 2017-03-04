//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MigAz.Generator
//{
//    class InteractiveTokenProvider : ITokenProvider
//    {
//        public string GetToken(string tenantId)
//        {
//            AuthenticationContext context = new AuthenticationContext(ServiceUrls.GetLoginUrl(app.Default.AzureEnvironment) + tenantId);

//            AuthenticationResult result = null;
//            result = context.AcquireToken(ServiceUrls.GetServiceManagementUrl(app.Default.AzureEnvironment), app.Default.ClientId, new Uri(app.Default.ReturnURL), PromptBehavior.Auto);
//            if (result == null)
//            {
//                throw new InvalidOperationException("Failed to obtain the token");
//            }


//            return result.AccessToken;

//        }
//    }
//}

//private string GetToken(string tenantId, PromptBehavior promptBehavior, bool updateUI = false)
//{
//    //"d94647e7-c4ff-4a93-bbe0-d993badcc5b8"
//    AuthenticationContext context = new AuthenticationContext(ServiceUrls.GetLoginUrl(app.Default.AzureAuth) + tenantId);

//    AuthenticationResult result = null;
//    try
//    {
//        result = context.AcquireToken(ServiceUrls.GetServiceManagementUrl(app.Default.AzureAuth), app.Default.ClientId, new Uri(app.Default.ReturnURL), promptBehavior);
//        if (result == null)
//        {
//            throw new InvalidOperationException("Failed to obtain the token");
//        }
//        if (updateUI)
//        {
//            // lblSignInText.Text = $"Signed in as {result.UserInfo.DisplayableId}";
//        }

//        return result.AccessToken;
//    }
//    catch (Exception exception)
//    {
//        DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        return null;
//    }
//}



//private string GetToken(string tenantId, PromptBehavior promptBehavior, bool updateUI = false, string AuthType = "AzureAuth")
//{

//    lblStatus.Text = "BUSY: Authenticating...";
//    //"d94647e7-c4ff-4a93-bbe0-d993badcc5b8"
//    AuthenticationContext context = new AuthenticationContext(ServiceUrls.GetLoginUrl(app.Default.AzureAuth) + tenantId);

//    AuthenticationResult result = null;
//    string AuthUri = null;

//    if (AuthType == "AzureAuth")
//    {
//        AuthUri = ServiceUrls.GetServiceManagementUrl(app.Default.AzureAuth);
//    }
//    else
//    {
//        AuthUri = ServiceUrls.GetServiceManagementUrl(app.Default.GraphAuth);
//    }

//    try
//    {
//        if (_userId == null)
//        {
//            result = context.AcquireToken(AuthUri, app.Default.ClientId, new Uri(app.Default.ReturnURL), promptBehavior);
//            _userId = new UserIdentifier(result.UserInfo.DisplayableId, UserIdentifierType.RequiredDisplayableId);
//        }
//        else
//        {
//            result = context.AcquireToken(AuthUri, app.Default.ClientId, new Uri(app.Default.ReturnURL), PromptBehavior.Never, _userId);
//            // result = context.AcquireTokenSilent(ServiceUrls.GetServiceManagementUrl(app.Default.GraphAuth), app.Default.ClientId, _userId)
//        }

//        if (result == null)
//        {
//            throw new InvalidOperationException("Failed to obtain the token");
//        }
//        if (updateUI)
//        {
//            lblSignInText.Text = $"Signed in as {result.UserInfo.DisplayableId}";
//        }

//        return result.AccessToken;
//    }
//    catch (Exception exception)
//    {
//        DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        return null;
//    }
//}
