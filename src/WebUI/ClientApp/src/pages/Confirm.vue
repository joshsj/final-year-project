<script setup lang="ts">
import {onMounted, ref, watch} from "vue";
import {store} from "@/store";
import {ClockClient, ConfirmationCodeDto} from "@/api/clients";
import PageTitle from "@/components/general/PageTitle.vue";
import {ElMessage} from "element-plus";
import {useRouter} from "vue-router";

const props = defineProps({
  assignmentId: {type: String, required: true}
});
const {back} = useRouter();

const codeHtml = ref<string>("");
const counter = ref({
  current: -1,
  type: "success",
  down: () => {
    counter.value.current -= 1;

    if (counter.value.current < 1) {
      ElMessage.info("Confirmation token expired.");
      back()
      return;
    }

    setTimeout(counter.value.down, 1000);
  }
});

const getConfirmationCode = async () => {
  const {svgSource, timeRemaining} = await store.page.load(
      () => new ClockClient().getConfirmationCode(props.assignmentId));

  codeHtml.value = svgSource;
  counter.value.current = timeRemaining;
  counter.value.down();
};

onMounted(getConfirmationCode);
</script>

<template>
  <page-title title="Confirm">
    <template v-if="counter.current > -1">
      <b>Time Remaining:</b> {{ counter.current }}
    </template>
  </page-title>

  <div class="confirmation-code" v-html="codeHtml"/>
</template>

<style scoped>
.confirmation-code {
  text-align: center;
}
</style>