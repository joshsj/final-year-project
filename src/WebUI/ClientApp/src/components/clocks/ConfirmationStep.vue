<script setup lang="ts">
import Step from "./Step.vue";
import {useUserMedia} from "@/plugins/userMedia";
import {useQrScanner} from "@/plugins/qrScanner";
import {onMounted, ref, watch} from "vue";
import {store} from "@/store";

defineProps({token: {type: String, required: true}});

const emit = defineEmits(["update:token"]);

const {getStream, stream, errorMessages} = useUserMedia({
  audio: false,
  video: {
    width: {ideal: 1600},
    height: {ideal: 1200},
    facingMode: "environment",
    aspectRatio: 4 / 3,
    frameRate: {ideal: 30}
  },
});

const {containerProvider, start, stop, data} = useQrScanner(stream, ref("fit"));

watch(stream, x => x && start());
watch(data, x => {
  if (!x) {
    return;
  }

  stop();
  stream.value?.getTracks().forEach(x => x.stop());
  emit("update:token", x);
});

onMounted(() => store.page.load(getStream));
</script>

<template>
  <step
      title="Confirmation"
      :state="!!token"
      :error-messages="errorMessages"
      @retry="getStream">
    <div :ref="containerProvider"/>

    <el-alert v-if="token" type="success" :closable="false">Scan successful.</el-alert>
  </step>
</template>

