import "./styles.scss";
import ElementPlus from "element-plus";
import App from "@/App.vue";
import { ComponentPublicInstance, createApp } from "vue";
import { createRouter } from "@/router";
import { createAuth0 } from "@auth0/auth0-vue";

const app: ComponentPublicInstance = createApp(App)
  .use(ElementPlus)
  .use(createRouter(() => app))
  .use(
    createAuth0({
      domain: import.meta.env.VITE_AUTH0_DOMAIN,
      client_id: import.meta.env.VITE_AUTH0_CLIENT_ID,
      audience: import.meta.env.VITE_AUTH0_AUDIENCE,
      redirect_uri: location.origin,
    })
  )
  .mount("#app");

console.log(import.meta.env);
