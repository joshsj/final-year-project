import {computed, ref} from "vue";

// https://developer.mozilla.org/en-US/docs/Web/API/GeolocationPositionError/code
const codeMessages: { [_: number]: string[] } = Object.freeze({
    1: [
        "You must allow access to your location.",
        "It can be enabled in the address bar of you browser."
    ],
    2: ["Failed to determine your location."],
    3: ["Failed to determine your location."]
});

// https://vueuse.org/core/usegeolocation but better
const useGeoLocation = () => {
    const supported = !!navigator.geolocation;
    const position = ref<GeolocationPosition | undefined>(undefined);
    const error = ref<GeolocationPositionError | undefined>(undefined);

    const errorMessages = computed((): string[] | undefined => {
        if (!supported) {
            return ["You cannot clock in with this device, as it does not have support for retrieving your location."]
        }

        if (error.value) {
            return codeMessages[error.value.code];
        }

        return undefined;
    });

    const getGeolocation = (options: PositionOptions = {enableHighAccuracy: true}) =>
        navigator.geolocation.getCurrentPosition(
            pos => {
                position.value = pos;
                error.value = undefined;
            },
            err => {
                error.value = err;
                position.value = undefined;
            },
            options
        );

    return {supported, position, error, errorMessages, getGeolocation};
};

export {useGeoLocation}