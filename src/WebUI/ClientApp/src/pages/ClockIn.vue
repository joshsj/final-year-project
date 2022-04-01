<script setup lang="ts">
import {computed, onMounted, readonly} from "vue";
import {store} from "@/store";
import PageTitle from "@/components/general/PageTitle.vue";
import {useGeoLocation} from "@/plugins/geoLocation";
import {ClockClient, ClockType, SubmitClockCommand} from "@/api/clients";
import {ElMessage} from "element-plus";

const props = defineProps({assignmentId: {type: String, required: true}});

const {supported, position, error, getGeolocation} = useGeoLocation();

const clockClient = readonly(new ClockClient());

// https://developer.mozilla.org/en-US/docs/Web/API/GeolocationPositionError/code
const codeMessages: { [_: number]: string[] } = {
  1: [
    "You must allow access to your location to clock in.",
    "It can be enabled in the address bar of you browser."
  ],
  2: ["Failed to determine your location."],
  3: ["Failed to determine your location."]
};

const errorMessages = computed((): string[] | undefined => {
  if (!supported) {
    return ["You cannot clock in with this device, as it does not have support for retrieving your location."]
  }

  if (error.value) {
    return codeMessages[error.value.code];
  }

  return undefined;
});

const confirm = async () => {
  if (!position.value) {
    return;
  }

  const {latitude, longitude} = position.value.coords;

  const request: SubmitClockCommand = {
    assignmentId: props.assignmentId,
    clockType: ClockType.In,
    coordinates: {latitude, longitude}
  }

  await store.page.load(() => clockClient.submit(request));
  
  ElMessage.success("Clock in successful!");
};

onMounted(getGeolocation);
</script>

<template>
  <page-title title="Clock In">
    <el-button
        v-if="errorMessages?.length"
        round
        type="primary"
        @click="getGeolocation">
      Retry
    </el-button>
  </page-title>

  <template v-if="position">
    <el-descriptions title="Location">
      <el-descriptions-item label="Latitude">{{ position.coords.latitude }}</el-descriptions-item>
      <el-descriptions-item label="Longitude">{{ position.coords.longitude }}</el-descriptions-item>
    </el-descriptions>

    <el-button type="success" round @click="confirm" d>Confirm</el-button>
  </template>

  <el-alert
      v-if="errorMessages?.length"
      type="error"
      :closable="false">
    <p v-for="msg in errorMessages" :key="msg">{{ msg }}</p>
  </el-alert>
</template>