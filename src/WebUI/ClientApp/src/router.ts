import {
  createRouter as createVueRouter,
  createWebHistory,
  RouteLocationRaw,
  RouteRecordRaw,
} from "vue-router";
import Home from "@/pages/Home.vue";
import Jobs from "@/pages/Jobs.vue";
import Account from "@/pages/Account.vue";
import { ComponentPublicInstance } from "vue";

declare module "vue-router" {
  interface RouteMeta {
    authenticated: boolean;
  }
}

type RouteRecordRawWithMeta = RouteRecordRaw &
  Required<Pick<RouteRecordRaw, "meta">>;

type RouteDef<T> = RouteRecordRawWithMeta & {
  helper: (arg: T) => RouteLocationRaw;
};

type Routes = typeof routes;
type RouteName = keyof Routes;

type HelperArg<T extends RouteName> = Parameters<
  Routes[T]["helper"]
>[0] extends void
  ? {}
  : Parameters<Routes[T]["helper"]>[0];

const route = <T extends object | void = void>(
  raw: RouteRecordRawWithMeta
): RouteDef<T> => {
  const helper = (arg: T) =>
    arg
      ? Object.entries(arg).reduce<string>(
          (path, [key, value]) => path.replace(`:${key}`, value),
          raw.path
        )
      : raw.path;

  return { ...raw, helper };
};

const routes = {
  home: route({
    path: "/",
    component: Home,
    meta: { authenticated: false },
  }),

  jobs: route({
    path: "/jobs",
    component: Jobs,
    meta: { authenticated: false },
  }),

  account: route({
    path: "/account",
    component: Account,
    meta: { authenticated: false },
  }),

  noPath: route({
    path: "/:noPath(.*)*",
    redirect: "/",
    meta: { authenticated: false },
  }),
};

const routeHelper = <T extends RouteName>(
  args: { name: T } & HelperArg<T>
): RouteLocationRaw => routes[args.name].helper(args as never);

const createRouter = (_app: () => ComponentPublicInstance | undefined) => {
  const router = createVueRouter({
    routes: Object.values(routes),
    history: createWebHistory(),
  });

  router.beforeEach(({ meta: { authenticated } }) => {
    const app = _app();

    // ensure homepage if app is unavailable
    return !authenticated || (app && app.$auth0.isAuthenticated.value)
      ? undefined
      : "/";
  });

  return router;
};

export { createRouter, routeHelper as route };
