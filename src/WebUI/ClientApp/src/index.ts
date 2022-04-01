import "./styles.scss";
import ElementPlus, {ElMessage} from "element-plus";
import App from "@/App.vue";
import {createApp, createVNode} from "vue";
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
        [_: string]: string[]
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

        const messages = Object.values(errors).flatMap(x => x.flatMap(x => x));
        
        return `<ul>${messages.map(x => `<li>${x}</li>`)}</ul>`
    } catch(_) {
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



