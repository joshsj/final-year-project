<script setup lang="ts">
import {Back, HomeFilled} from "@element-plus/icons-vue";
import {useRouter} from "vue-router";

const {back, push} = useRouter();

defineProps({
  title: {type: String, required: true},
});
</script>

<template>
  <header class="page-title">
    <el-row justify="space-between" align="middle">
      <el-row align="middle">
        <el-button
            id="back"
            type="primary"
            :icon="Back"
            circle
            @click="back"/>

        <h1>{{ title }}</h1>
      </el-row>

      <el-row justify="space-between" align="middle">
        <div>
          <slot/>
        </div>

        <el-button
            id="home"
            type="primary"
            :icon="HomeFilled"
            circle
            @click="push('/')"/>
      </el-row>
    </el-row>
  </header>
</template>

<style scoped>
.page-title {
  margin-bottom: 1rem;
}

h1 {
  margin: 0;
}

#back {
  margin-right: 1rem;
}

#home {
  margin-left: 1rem;
}
</style>
