import "./styles.scss";
import ElementPlus, {ElMessage} from "element-plus";
import {VNetworkGraph} from "v-network-graph";
import App from "@/App.vue";
import {createApp} from "vue";
import {createRouter} from "@/router";
import {createAuth0} from "@auth0/auth0-vue";
import {ApiException} from "@/api/clients";

const app = createApp(App)
    .use(ElementPlus)
    .use(createRouter())
    .use(
        createAuth0({
            domain: import.meta.env.VITE_AUTH0_DOMAIN,
            client_id: import.meta.env.VITE_AUTH0_CLIENT_ID,
            audience: import.meta.env.VITE_AUTH0_AUDIENCE,
            redirect_uri: location.origin,
        })
    )

type Errors = {
    errors?: {
        [field: string]: string[]
    }
}

// TODO tidy
const getErrorMessage = (err: any): string => {
    const defaultMessage = "Sorry, an unknown error has occured.";

    if (!(err instanceof ApiException)) {
        return defaultMessage;
    }

    try {
        const {errors} = JSON.parse(err.response) as Errors;

        if (!errors) {
            return defaultMessage;
        }

        return Object.values(errors)
            .flatMap(x => x.flatMap(x => `<p>${x}</p>`))
            .join("");
    } catch (_) {
    }

    return defaultMessage;
}

app.config.errorHandler = (err) => {
    ElMessage({
        type: "error",
        message: getErrorMessage(err),
        grouping: false,
        showClose: true,
        dangerouslyUseHTMLString: true
    });

    import.meta.env.DEV && console.dir(err);
}
app.mount("#app");



