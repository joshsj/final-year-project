import "./styles.scss";
import ElementPlus, {ElMessage} from "element-plus";
import App from "@/App.vue";
import { createApp } from "vue";
import { createRouter } from "@/router";
import { createAuth0 } from "@auth0/auth0-vue";

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

app.config.errorHandler = () => ElMessage({
    type: "error",
    message: "Sorry, an unknown error has occured.",
    grouping: true,
    showClose: true
});

app.mount("#app");



