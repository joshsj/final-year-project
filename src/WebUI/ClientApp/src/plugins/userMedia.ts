import {computed, ref} from "vue";


const useUserMedia = (constraints?: MediaStreamConstraints) => {
    const stream = ref<MediaStream | undefined>(undefined);
    const error = ref<DOMException | undefined>(undefined);

    // https://developer.mozilla.org/en-US/docs/Web/API/MediaDevices/getUserMedia
    const errorMessages = computed<string[] | undefined>(() => {
        if (!error.value) {
            return undefined;
        }

        return error.value.message === "NotAllowedError"
            ? [
                "You must allow access to your location.",
                "It can be enabled in the address bar of you browser."
            ]
            : [error.value.message];
    });

    const getStream = () =>
        navigator.mediaDevices.getUserMedia(constraints)
            .then(s => {
                stream.value = s;
                error.value = undefined;
            })
            .catch(err => {
                stream.value = undefined;
                error.value = err;
            });

    return {stream, error, errorMessages, getStream};
};

export {useUserMedia};