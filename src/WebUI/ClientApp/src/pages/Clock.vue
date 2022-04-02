<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import LocationStep from "@/components/clocks/LocationStep.vue";
import ConfirmationStep from "@/components/clocks/ConfirmationStep.vue";
import {PropType, ref} from "vue";
import {ClockClient, ClockType, Coordinates, SubmitClockCommand} from "@/api/clients";
import {store} from "@/store";

const props = defineProps({
  assignmentId: {type: String, required: true},
  type: {type: String as PropType<"0" | "1">, required: true,}
});

const coordinates = ref<Coordinates | undefined>(undefined);
const tokenId = ref<string | undefined>(undefined);

const typeText = ClockType[props.type]!;

const submit = async () => {
  if (!(coordinates.value && tokenId.value)) {
    return;
  }

  const request: SubmitClockCommand = {
    assignmentId: props.assignmentId,
    coordinates: coordinates.value,
    clockType: parseInt(props.type)
  }

  await store.page.load(() => new ClockClient().submit(request));

  store.page.result = {
    icon: "success",
    title: "Success",
    subTitle: `You're clocked ${typeText.toLowerCase()}.`
  }
};
</script>

<template>
  <page-title :title="`Clock ${typeText}`"/>

  <location-step v-model:coordinates="coordinates"/>
  <confirmation-step v-if="coordinates" v-model:token-id="tokenId"/>

  <el-button
      type="success"
      round
      v-show="coordinates && tokenId"
      @click="submit">
    Submit
  </el-button>
</template>

<style scoped>
.page-title {
  margin-bottom: 0;
}

.el-button {
  margin-top: 1rem;
}
</style>

