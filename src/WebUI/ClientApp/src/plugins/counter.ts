import {Ref, ref} from "vue";

const useCounter = (current: Ref<number> = ref(0)) => {
    const counting = ref(false);

    const count = (to: number, step: number) => {
        if (!counting.value || current.value === to) {
            stop();
            return;
        }
        
        current.value += step;
        setTimeout(() => count(to, step), 1000);
    }

    const start = (from: number, to = 0, step = -1) => {
        counting.value = true;
        current.value = from;

        count(to, step);
    }
    
    const stop = () => {
        counting.value = false;
    }

    return {start, stop, current, counting};
};


export {useCounter};