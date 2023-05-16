import Oidc from 'oidc-client';
function getOidcSettings() {
    const { protocol, hostname, port } = window.location;
    // const { protocol, hostname, port } = idassEnvOptions[env].authority;
    const currentHost = `${protocol}//${hostname}${port ? `:${port}` : ''}`;
    const settings = {
        authority: import.meta.env.VITE_APP_OIDC_API,
        client_id: 'Admin_App',
        redirect_uri: currentHost + '/callback',
        post_logout_redirect_uri: currentHost,
        response_type: `code`,
        client_secret: '',
        scope: 'openid profile Admin',
        //silent_redirect_uri: currentHost + '/oidc-silent-renew',
        automaticSilentRenew: true, // If true oidc-client will try to renew your token when it is about to expire
        automaticSilentSignin: true, // If true vuex-oidc will try to silently signin unauthenticated users on public routes. Defaults to true
    };
    return settings;
}

const settings = getOidcSettings();
// console.log(settings)
const mgr = new Oidc.UserManager(settings);
mgr.events.addAccessTokenExpiring(function () {
    console.log('token expiring');
});

// token过期事件
mgr.events.addAccessTokenExpired(function () {
    console.log('token expired');
});

// 授权失败事件
mgr.events.addSilentRenewError(function (e) {
    console.log('silent renew error', e.message);
});

// 用户加载事件 
mgr.events.addUserLoaded(function (user) {
    console.log('user loaded', user);
    // 获取用户信息
    mgr.getUser().then(function () {
        console.log('getUser loaded user after userLoaded event fired');
    });
});

// 用户卸载事件
mgr.events.addUserUnloaded(function () {
    console.log('user unloaded');
});

export function useOidcLogin() {
    // 登陆事件
    return mgr.signinRedirect();
}

export function useOidcLogout() {
    //登出事件
    return mgr.signoutRedirect();
}

export function callBack() {
    // 登陆成功回调事件
    return mgr.signinRedirectCallback();
}

export function refresh() {
    return mgr.signinSilentCallback();
}
