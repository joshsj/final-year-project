<script setup lang="ts">
import {PropType, ref} from "vue";
import {ClockType, Coordinates} from "@/api/clients";
import PageTitle from "@/components/general/PageTitle.vue";
import LocationStep from "@/components/clocks/LocationStep.vue";

const props = defineProps({
  assignmentId: {type: String, required: true},
  type: {type: String as PropType<"0" | "1">, required: true,}
});

const location = ref<Coordinates | undefined>(undefined);

const typeText = ClockType[props.type]!;

const confirm = async () => {
  // if (!position.value) {
  //   return;
  // }
  //
  // const {latitude, longitude} = position.value.coords;
  //
  // const request: SubmitClockCommand = {
  //   assignmentId: props.assignmentId,
  //   coordinates: {latitude, longitude},
  //   clockType: parseInt(props.type)
  // }
  //
  // await store.page.load(() => new ClockClient().submit(request));
  //
  // store.page.result = {
  //   icon: "success",
  //   title: "Success",
  //   subTitle: `You're clocked ${typeText.toLowerCase()}.`
  // }
};
</script>

<template>
  <page-title :title="`Clock ${typeText}`"/>

  <location-step v-model:location="location"/>

  <el-button
      type="success"
      round
      :disabled="!location"
      @click="confirm">
    Confirm
  </el-button>
</template>

