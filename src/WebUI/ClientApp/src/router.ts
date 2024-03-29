import {
    createRouter as createVueRouter,
    createWebHistory,
    RouteLocationRaw,
    RouteRecordRaw,
} from "vue-router";
import Home from "@/pages/Home.vue";
import Job from "@/pages/Job.vue";
import Jobs from "@/pages/Jobs.vue";
import ConfirmedClock from "@/pages/ConfirmedClock.vue";
import UnconfirmedClock from "@/pages/UnconfirmedClock.vue";
import Confirm from "@/pages/Confirm.vue";
import Confirmations from "@/pages/Confirmations.vue";
import Account from "@/pages/Account.vue";
import {store} from "@/store";
import {ClockType} from "@/api/clients";

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

type HelperArg<T extends RouteName> = Parameters<Routes[T]["helper"]>[0] extends void
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

    return {...raw, helper};
};

const routes = {
    home: route({
        path: "/",
        component: Home,
        meta: {authenticated: false},
    }),

    job: route<{ jobId: string }>({
        path: "/jobs/:jobId",
        component: Job,
        meta: {authenticated: true},
        props: true
    }),

    jobs: route({
        path: "/jobs",
        component: Jobs,
        meta: {authenticated: true},
    }),

    confirmedClock: route<{ assignmentId: string, type: ClockType }>(
    {
            path: "/confirmed-clock/:type/:assignmentId",
            component: ConfirmedClock,
            meta: {authenticated: true},
            props: true
        }
    ),

    unconfirmedClock: route<{ assignmentId: string, type: ClockType }>(
        {
            path: "/unconfirmed-clock/:type/:assignmentId",
            component: UnconfirmedClock,
            meta: {authenticated: true},
            props: true
        }
    ),

    confirm: route<{ assignmentId: string }>(
        {
            path: "/confirm/:assignmentId",
            component: Confirm,
            meta: {authenticated: true},
            props: true
        }
    ),

    confirmations: route<{ jobId: string }>(
        {
            path: "/confirmations/:jobId",
            component: Confirmations,
            meta: {authenticated: true},
            props: true
        }
    ),

    account: route({
        path: "/account",
        component: Account,
        meta: {authenticated: true},
    }),

    noPath: route({
        path: "/:noPath(.*)*",
        redirect: "/",
        meta: {authenticated: false},
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

    // ensure homepage if app is unavailable
    router.beforeEach(({meta: {authenticated}}) =>
        !authenticated || store.accessToken ? undefined : "/");

    return router;
};

export {createRouter, routeHelper as route};
