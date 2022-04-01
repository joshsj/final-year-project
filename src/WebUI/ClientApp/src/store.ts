import { reactive, UnwrapNestedRefs } from "vue";
import {AssignmentDto, BriefJobDto, JobClient} from "@/api/clients";

type Job = BriefJobDto & {
    assignments?: AssignmentDto[]
}

type Store = {
    readonly page: {
        loading: boolean;
        readonly load: <T>(f: () => Promise<T>) => Promise<T>;
    };
    accessToken: string | undefined;
    readonly jobs: { 
        items: Job[]
        readonly fetch: () => Promise<void>
        readonly fetchAssignments: (jobId: string) => Promise<void>
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
        fetch: async () => {
            store.jobs.items = await store.page.load(() => new JobClient().get())  
        },
        fetchAssignments: async (jobId: string) => {
            const job = store.jobs.items.find(x => x.id === jobId);
            
            job && (job.assignments =  await store.page.load(() => new JobClient().getAssignments(jobId)));
        }
    }
});

export {store};
