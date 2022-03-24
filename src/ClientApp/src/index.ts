import "./styles.scss";
import ElementPlus from "element-plus";
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
      redirect_uri: location.origin,
    })
  )
  .mount("#app");

export { app };
