import { reactive, UnwrapNestedRefs } from "vue";

type Store = {
  page: {
    loading: boolean;
    load: <T>(f: () => Promise<T>) => Promise<T>;
  };
};

const store: UnwrapNestedRefs<Store> = reactive<Store>({
  page: {
    loading: false,
    load: (f) => {
      store.page.loading = true;

      return f().finally(() => (store.page.loading = false));
    },
  },
});

export { store };
