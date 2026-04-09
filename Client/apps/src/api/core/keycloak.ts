import Keycloak from 'keycloak-js';

interface KeycloakConfig {
    url: string;
    realm: string;
    clientId: string;
}

const resolveConfig = (): KeycloakConfig => {
    const url = 'http://localhost:8080'; // Keycloak server URL
    const realm = 'BookStoreRealm';
    const clientId = 'bookstore-client';

    return { url, realm, clientId };
};


let _instance: Keycloak | null = null;

const getInstance = (): Keycloak => {
    if (!_instance)
        _instance = new Keycloak(resolveConfig());
    return _instance;
}


/**
 * Gọi 1 lần duy nhất ở entry point (main.tsx / _app.tsx)
 * @param onLoad 'login-required' redirect thẳng | 'check-sso' silent check
 */
export const initKeycloak = async (onLoad: 'login-required' | 'check-sso' = 'login-required'): Promise<boolean> => {

    const kc = getInstance();

    const authenticated = await kc.init({
        onLoad,
        checkLoginIframe: false,
        pkceMethod: 'S256'
    })

    kc.onTokenExpired = () => {
        kc.updateToken(30).catch(() => {
            console.warn('[Keycloak] Failed to refresh token, logging out');

            logout();
        })
    }

    return authenticated;
}


export const getAccessToken = async (): Promise<string> => {
    const kc = getInstance();

    if (!kc.authenticated) {
        throw new Error('[Keycloak] User not authenticated');

    }
    await kc.updateToken(30);

    if (!kc.token) {
        throw new Error('[Keycloak] No token available');
    }

    return kc.token;
}


export const logout = (redirectUri?: string): void => {
    getInstance().logout({ redirectUri });

}

export const getKeycloak = (): Keycloak => getInstance();
