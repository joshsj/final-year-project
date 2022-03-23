import {
  createRouter as createVueRouter,
  createWebHistory,
  RouteLocationRaw,
  RouteRecordRaw,
} from "vue-router";
import Home from "@/views/Home.vue";

type RouteDef<T> = RouteRecordRaw &
  Required<Pick<RouteRecordRaw, "meta">> & {
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
  raw: RouteRecordRaw & Required<Pick<RouteRecordRaw, "meta">>
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
    meta: { auth: "none" },
  }),

  noPath: route({
    path: "/:noPath(.*)*",
    redirect: "/",
    meta: { auth: "none" },
  }),
};

const routeHelper = <T extends RouteName>(
  args: { name: T } & HelperArg<T>
): RouteLocationRaw => routes[args.name].helper(args as never);

const createRouter = () => {
  const router = createVueRouter({
    routes: Object.values(routes),
    history: createWebHistory(),
  });

  return router;
};
export { createRouter, routeHelper as route };
