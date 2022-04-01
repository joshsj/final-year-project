import {ref} from "vue";

// https://vueuse.org/core/usegeolocation but better
const useGeoLocation = () => {
    const supported = !!navigator.geolocation;
    const position = ref<GeolocationPosition | undefined>(undefined);
    const error = ref<GeolocationPositionError | undefined>(undefined);

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

    return {supported, position, error, getGeolocation};
};

export {useGeoLocation}