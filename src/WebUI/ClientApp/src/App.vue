<script setup lang="ts">
import {store} from "@/store";
import {useAuth0} from "@auth0/auth0-vue";
import {useAccessTokenBehavior} from "@/plugins/auth";
import {useRouter} from "vue-router";

useAccessTokenBehavior();
const {isLoading} = useAuth0();
const {back: _back} = useRouter();

const back = () => {
  _back();

  store.page.result = undefined; // reset
}
</script>

<template>
  <el-row
      id="root"
      justify="center"
      align="middle"
      v-loading="store.page.loading || isLoading">
    <el-col :span="23" :sm="16" :md="12">
      <el-result
          v-show="store.page.result"
          v-bind="store.page.result">
        <template #extra>
          <el-button
              type="primary"
              round
              @click="back">Back
          </el-button>
        </template>
      </el-result>

      <main v-show="!store.page.result">
        <router-view/>
      </main>
    </el-col>
  </el-row>
</template>

<style scoped>
#root {
  min-height: 100vh;
}
</style>
