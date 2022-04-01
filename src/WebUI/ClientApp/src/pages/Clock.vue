<script setup lang="ts">
import {computed, onMounted, Prop, PropType, readonly, } from "vue";
import {store} from "@/store";
import PageTitle from "@/components/general/PageTitle.vue";
import {useGeoLocation} from "@/plugins/geoLocation";
import {ClockClient, ClockType, SubmitClockCommand} from "@/api/clients";

const props = defineProps({
  assignmentId: {type: String, required: true},
  type: {type: String as PropType<"0" | "1">, required: true,}
});

const {supported, position, error, getGeolocation, errorMessages} = useGeoLocation();
const typeText = ClockType[props.type]!;

const confirm = async () => {
  if (!position.value) {
    return;
  }

  const {latitude, longitude} = position.value.coords;

  const request: SubmitClockCommand = {
    assignmentId: props.assignmentId,
    coordinates: {latitude, longitude},
    clockType: parseInt(props.type)
  }

  await store.page.load(() => new ClockClient().submit(request));

  store.page.result = {
      icon: "success",
      title: "Success",
      subTitle: `You're clocked ${typeText.toLowerCase()}.`
  }
};

onMounted(getGeolocation);
</script>

<template>
  <page-title :title="`Clock ${typeText}`">
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