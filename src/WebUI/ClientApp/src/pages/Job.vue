<script lang="ts" setup>
import PageTitle from "@/components/general/PageTitle.vue";
import JobDetails from "@/components/jobs/JobDetails.vue";
import {onMounted, ref} from "vue";
import {store} from "@/store";
import {display} from "@/utilities/display";
import {AssignmentDto, ClockType} from "@/api/clients";
import {useRouter} from "vue-router";
import {route} from "@/router";
import {useAssignmentBusiness} from "@/plugins/business";

const props = defineProps({jobId: {type: String, required: true}});
const job = ref(store.jobs.items.find(x => x.id === props.jobId)!);

const {push} = useRouter();
const {
  isUserAssigment,
  userAssignment,
  canClockOut,
  canClockIn,
  canConfirm,
  requiresConfirmation
} = useAssignmentBusiness(job.value);

const clock = (type: ClockType) => {
  if (!(userAssignment.value && job.value.assignments)) {
    return;
  }

  push(route({
    name: requiresConfirmation(job.value.assignments) ? 'confirmedClock' : "unconfirmedClock",
    assignmentId: userAssignment.value.id,
    type
  }));
};

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
            label="Clock In"
            prop="clockIn.at"
            :formatter="({}, {}, d) => d && display.date(d, 'time')"/>

        <el-table-column
            label="Clock Out"
            prop="clockOut.at"
            :formatter="({}, {}, d) => d && display.date(d, 'time')"/>

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
