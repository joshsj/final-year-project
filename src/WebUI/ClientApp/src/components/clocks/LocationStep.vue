<script setup lang="ts">
import {useGeoLocation} from "@/plugins/geoLocation";
import {onMounted, PropType, watch} from "vue";
import {Coordinates} from "@/api/clients";
import Step from "./Step.vue";

defineProps({
  coordinates: Object as PropType<Coordinates>
});

const emit = defineEmits(["update:coordinates"])

const {position, getGeolocation, errorMessages} = useGeoLocation();

watch(position, (p) => emit(
    'update:coordinates',
    p ? {latitude: p.coords.latitude, longitude: p.coords.longitude} : undefined));

onMounted(getGeolocation);
</script>

<template>
  <step
      title="Location"
      :state="!!coordinates"
      :error-messages="errorMessages"
      @retry="getGeolocation">
    <el-alert v-if="coordinates" type="success" :closable="false">
      {{ coordinates.latitude }}, {{ coordinates.longitude }}
    </el-alert>
  </step>
</template>
