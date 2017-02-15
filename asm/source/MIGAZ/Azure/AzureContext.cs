using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MIGAZ.Interface;
using System;
using System.Threading.Tasks;

namespace MIGAZ.Azure
{
    public enum AzureEnvironment
    {
        AzureCloud,
        AzureChinaCloud,
        AzureGermanCloud,
        AzureUSGovernment
    }

    public class AzureContext
    {
        private AzureEnvironment _AzureEnvironment = Azure.AzureEnvironment.AzureCloud;
        private AzureSubscription _AzureSubscription;
        private AzureRetriever _AzureRetriever;
        private AzureTokenProvider _TokenProvider;
        private ILogProvider _LogProvider;
        private IStatusProvider _StatusProvider;
        private ISettingsProvider _SettingsProvider;

        public delegate Task BeforeAzureSubscriptionChangedHandler(AzureContext sender);
        public event BeforeAzureSubscriptionChangedHandler BeforeAzureSubscriptionChange;

        public delegate Task AfterAzureSubscriptionChangedHandler(AzureContext sender);
        public event AfterAzureSubscriptionChangedHandler AfterAzureSubscriptionChange;

        public delegate Task AzureEnvironmentChangedHandler(AzureContext sender);
        public event AzureEnvironmentChangedHandler AzureEnvironmentChanged;

        public delegate Task UserAuthenticatedHandler(UserInfo userAuthenticated);
        public event UserAuthenticatedHandler UserAuthenticated;

        public delegate Task BeforeUserSignOutHandler();
        public event BeforeUserSignOutHandler BeforeUserSignOut;

        public delegate Task AfterUserSignOutHandler();
        public event AfterUserSignOutHandler AfterUserSignOut;

        #region Constructors

        public AzureContext(ILogProvider logProvider, IStatusProvider statusProvider, ISettingsProvider settingsProvider)
        {
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
            _SettingsProvider = settingsProvider;
        }

        #endregion

        #region Properties

        public Azure.AzureEnvironment AzureEnvironment
        {
            get { return _AzureEnvironment; }
            set
            {
                // Only allow value change when not authenticated
                if (_TokenProvider != null && _AzureEnvironment != value)
                    throw new ArgumentException("Azure Environment cannot be changed while authenticated.  Sign out before chaning environments.");
                else
                    _AzureEnvironment = value;

                AzureEnvironmentChanged?.Invoke(this);
            }
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public AzureRetriever AzureRetriever
        {
            get { return _AzureRetriever; }
            set { _AzureRetriever = value; }
        }

        public AzureTokenProvider TokenProvider
        {
            get { return _TokenProvider; }
            set
            {
                _TokenProvider = value;
                if (_TokenProvider != null)
                    UserAuthenticated?.Invoke(_TokenProvider.AuthenticationResult.UserInfo);
            }
        }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _StatusProvider; }
        }

        public ISettingsProvider SettingsProvider
        {
            get { return _SettingsProvider; }
        }

        #endregion

        #region Methods

        public async Task Login()
        {
            this.TokenProvider = await AzureTokenProvider.LoginAzureProvider(this.AzureEnvironment);
            this._AzureRetriever = new AzureRetriever(this);
        }

        public async Task SetSubscriptionContext(AzureSubscription azureSubscription)
        {
            if (BeforeAzureSubscriptionChange != null)
                await BeforeAzureSubscriptionChange?.Invoke(this);

            _AzureSubscription = azureSubscription;

            if (_AzureSubscription != null)
            {
                if (_TokenProvider != null)
                    await _TokenProvider.GetToken(_AzureSubscription);

                if (_AzureRetriever == null)
                    _AzureRetriever = new AzureRetriever(this);

                await _AzureRetriever.SetSubscriptionContext(_AzureSubscription);
            }

            if (AfterAzureSubscriptionChange != null)
                await AfterAzureSubscriptionChange?.Invoke(this);
        }

        //internal void SetAzureContext(AzureContext azureContext)
        //{
        //    this.AzureEnvironment = azureContext.AzureEnvironment;
        //    _AzureRetriever = azureContext.AzureRetriever;
        //    _TokenProvider = azureContext.TokenProvider;

        //    if (this.TokenProvider != null)
        //    {
        //        UserAuthenticated?.Invoke(this.TokenProvider.AuthenticationResult.UserInfo);
        //    }
        //}

        public async Task Logout()
        {
            if (BeforeUserSignOut != null)
                await BeforeUserSignOut.Invoke();

            await this.SetSubscriptionContext(null);
            _AzureRetriever = null;
            _TokenProvider = null;

            if (AfterUserSignOut != null)
                await AfterUserSignOut?.Invoke();
        }

        #endregion
    }
}
