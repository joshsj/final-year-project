<script setup lang="ts">
import {useGeoLocation} from "@/plugins/geoLocation";
import {onMounted, PropType, watch} from "vue";
import {Coordinates} from "@/api/clients";

defineProps({
  location: Object as PropType<Coordinates>
});

const emit = defineEmits(["update:location"])

const {position, getGeolocation, errorMessages} = useGeoLocation();

watch(position, (p) => emit(
    'update:location',
    p ? {latitude: p.coords.latitude, longitude: p.coords.longitude} : undefined));

onMounted(getGeolocation);
</script>

<template>
  <h2>Location</h2>

  <el-descriptions v-if="location">
    <el-descriptions-item label="Latitude">{{ location.latitude }}</el-descriptions-item>
    <el-descriptions-item label="Longitude">{{ location.longitude }}</el-descriptions-item>
  </el-descriptions>

  <el-alert v-if="errorMessages?.length" type="error" :closable="false">
    <p v-for="msg in errorMessages" :key="msg">{{ msg }}</p>

    <el-button
        round
        size="small"
        type="primary"
        @click="getGeolocation">
      Retry
    </el-button>
  </el-alert>
</template>


<style scoped>
.el-alert {
  margin-bottom: 1rem;
}
</style>
