import {reactive, UnwrapNestedRefs} from "vue";
import {BriefJobDto, JobClient} from "@/api/clients";

type Store = {
    readonly page: {
        loading: boolean;
        readonly load: <T>(f: () => Promise<T>) => Promise<T>;
    };
    accessToken: string | undefined;
    readonly jobs: { 
        items: BriefJobDto[]
        readonly refresh: () => Promise<void>
    }
};

const store: UnwrapNestedRefs<Store> = reactive<Store>({
    page: {
        loading: false,
        load: (f) => {
            store.page.loading = true;

            return f().finally(() => (store.page.loading = false));
        },
    },

    accessToken: undefined,

    jobs: {
        items: [],
        refresh: async () => {
            store.jobs.items = await store.page.load(() => new JobClient().get())  
        }
    }
});

export {store};
