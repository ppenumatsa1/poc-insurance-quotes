export const msalConfig = {
    auth: {
        clientId: process.env.REACT_APP_CLIENT_ID,
        authority: process.env.REACT_APP_AUTHORITY,
        redirectUri: process.env.REACT_APP_REDIRECT_URI,
    },
    cache: {
        cacheLocation: 'sessionStorage',
        storeAuthStateInCookie: false,
    }
};

export const loginRequest = {
    scopes: [process.env.REACT_APP_API_SCOPE || 'api://your-api-client-id/access_as_user']
};