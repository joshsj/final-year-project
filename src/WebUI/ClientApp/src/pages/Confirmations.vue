<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import {store} from "@/store";
import {computed, readonly, ref} from "vue";
import {AssignmentDto, ClockDto, ClockType} from "@/api/clients";
import {VNetworkGraph, Nodes, Edges, defineConfigs, NodeConfig, EdgeConfig} from "v-network-graph";
import {display} from "@/utilities/display";

const props = defineProps({jobId: {type: String, required: true}});

const job = ref(store.jobs.items.find(x => x.id === props.jobId));
const clockType = ref<ClockType>(ClockType.In);

type ClockNode = ClockDto & Pick<AssignmentDto, "employeeName">;

const clocks = computed(() => {
  const field = clockType.value === ClockType.In ? "clockIn" : "clockOut";

  return job.value?.assignments?.reduce<ClockNode[]>(
          (clocks, ass) => {
            const clock = ass[field];
            clock && clocks.push({...clock, employeeName: ass.employeeName});
            return clocks;
          }, [])
      ?? [];
});

const nodes = computed(() =>
    clocks.value.reduce<Nodes>(
        (nodes, {id, at, employeeName}) => {
          nodes[id] = {
            name: `${employeeName}\n${display.date(at, "time")}`
          }

          return nodes;
        }, {}));

const edges = computed(() => clocks.value.reduce<Edges>((edges, {id, parentId}) => {
  parentId && (edges[`${id}-${parent}`] = {source: id, target: parentId});
  return edges;
}, {}));

const config = readonly(
    defineConfigs({
      view: {
        zoomEnabled: false,
        autoPanAndZoomOnLoad: "fit-content"
      },

      node: {
        draggable: false,
        normal: {
          color: "var(--el-color-success-light-3)"
        },
        hover: {
          radius: 20,
          color: "var(--el-color-success-light-3)"
        },
        label: {
          fontSize: 14
        }
      },
      edge: {
        normal: {
          color: "var(--el-color-success-light-5)"
        },
        hover: {
          color: "var(--el-color-success-light-5)"
        }
      }
    })
);
</script>

<template>
  <page-title title="Confirmations">
    <el-button-group>
      <el-button
          round
          :type="(clockType === ClockType.In) && 'primary'"
          @click="clockType = ClockType.In">In
      </el-button>
      <el-button
          round
          :type="(clockType === ClockType.Out) && 'primary'"
          @click="clockType = ClockType.Out">Out
      </el-button>
    </el-button-group>
  </page-title>

  <div id="container">
    <v-network-graph
        v-show="clocks.length"
        :nodes="nodes"
        :edges="edges"
        :configs="config"/>
  </div>

  <el-empty v-show="!clocks.length"/>
</template>


<style scoped>
#container {
    text-align: center;
}
</style>