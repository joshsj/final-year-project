<script lang="ts" setup>
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {onMounted, readonly} from "vue";
import {store} from "@/store";
import {display} from "@/utilities/display";
import {AssignmentDto, ClockType} from "@/api/clients";
import {useRouter} from "vue-router";
import {route} from "@/router";
import {useAssignmentBusiness} from "@/plugins/business";

const props = defineProps({jobId: {type: String, required: true}});
const job = readonly(store.jobs.items.find(x => x.id === props.jobId)!);

const {push} = useRouter();
const {
  isUserAssigment,
  userAssignment,
  canClockOut,
  canClockIn,
  canConfirm
} = useAssignmentBusiness(job);

const clock = (type: ClockType) => userAssignment.value && push(route({
  name: 'clock',
  assignmentId: userAssignment.value.id,
  type
}));

const confirm = ({id}: AssignmentDto) => push(route({
  name: 'confirm',
  assignmentId: id
}));

onMounted(() => store.jobs.fetchAssignments(props.jobId));
</script>

<template>
  <page-title title="Job"/>

  <job-details :job="job"/>

  <el-collapse v-if="job.assignmentCount">
    <el-collapse-item title="Staff">
      <el-table :data="job.assignments">
        <el-table-column
            #="{row: assignment}"
            label="Name">
          {{ isUserAssigment(assignment) ? "You" : assignment.employeeName }}
        </el-table-column>

        <el-table-column
            :formatter="({}, {}, value) => display.bool(value)"
            label="Clock In"
            prop="clockedIn"/>

        <el-table-column
            :formatter="({}, {}, value) => display.bool(value)"
            label="Clock Out"
            prop="clockedOut"/>

        <el-table-column
            label="Actions"
            #="{row: assignment}">
          <el-button
              v-if="canClockIn(assignment)"
              round
              size="small"
              type="success"
              @click="clock(ClockType.In)">
            Clock In
          </el-button>

          <el-button
              v-if="canClockOut(assignment)"
              round
              size="small"
              type="success"
              @click="clock(ClockType.Out)">
            Clock Out
          </el-button>

          <el-button
              v-if="canConfirm(assignment)"
              round
              size="small"
              type="primary"
              @click="confirm(assignment)">
            Confirm
          </el-button>
        </el-table-column>
      </el-table>
    </el-collapse-item>
  </el-collapse>
</template>
