<script setup lang="ts">
import RvPageTitle from "@/components/general/PageTitle.vue";
import { RemoveIndex } from "@/utilities/types";
import { User } from "@auth0/auth0-spa-js";
import { useAuth0 } from "@auth0/auth0-vue";
import { capitalize } from "lodash";
import { readonly } from "vue";

const { user } = useAuth0();

type Key = keyof RemoveIndex<User>;
const keys = readonly<Key[]>([
  "sub",
  "nickname",
  "name",
  "email",
  "email_verified",
]);

const prettyKey = (k: Key) => k.split("_").map(capitalize).join(" ");
</script>

<template>
  <rv-page-title title="Account" />

  <p v-for="k in keys" :key="k">
    <b>{{ prettyKey(k) }}</b> {{ user[k] }}
  </p>
</template>
