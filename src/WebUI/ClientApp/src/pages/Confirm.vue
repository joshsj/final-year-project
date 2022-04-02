<script setup lang="ts">
import {onMounted, ref, watch} from "vue";
import {store} from "@/store";
import {ClockClient, ConfirmationCodeDto} from "@/api/clients";
import PageTitle from "@/components/general/PageTitle.vue";
import {useRouter, onBeforeRouteLeave} from "vue-router";
import {useCounter} from "@/plugins/counter";
import {ElMessage} from "element-plus";

const props = defineProps({
  assignmentId: {type: String, required: true}
});
const {back} = useRouter();

const svgSource = ref("");
const countdown = useCounter();

const getConfirmationCode = async () => {
  const dto = await store.page.load(
      () => new ClockClient().getConfirmationCode(props.assignmentId));

  svgSource.value = dto.svgSource
  countdown.start(10);
};

onMounted(getConfirmationCode);

// ensure counter stops to destroy component
onBeforeRouteLeave(countdown.stop);
watch(countdown.current, (x) => {
    if (x > 0) {
        return;
    }

    ElMessage.info("Confirmation code expired.");
    back();
});
</script>

<template>
  <page-title title="Confirm">
    <template v-if="countdown.counting">
      <b>Time Remaining</b> ~{{ countdown.current }}s
    </template>
  </page-title>

  <div class="confirmation-code" v-html="svgSource"/>
</template>

<style scoped>
.confirmation-code {
  text-align: center;
}
</style>