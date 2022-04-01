import {store} from "@/store";
import {useAuth0} from "@auth0/auth0-vue";
import {watch} from "vue";

const useAuth = () => {
    const { loginWithPopup, logout: _logout } = useAuth0();

    const login = () =>
        store.page
            .load(loginWithPopup)
            .catch(() => void 0);

    const logout = () => {
        store.page.loading = true;
        _logout({returnTo: location.origin});
    };

    return { login, logout }
};

/** Saves an access token to the store whenever Auth0 authenticates */
const useAccessTokenBehavior = () => {
    const { isAuthenticated, getAccessTokenSilently } = useAuth0();
    
    watch(isAuthenticated, async (auth) => {
        auth && (store.accessToken = await getAccessTokenSilently());
    });
};

export { useAuth, useAccessTokenBehavior }