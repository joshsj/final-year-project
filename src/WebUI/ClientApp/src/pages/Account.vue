<script setup lang="ts">
import PageTitle from "@/components/general/PageTitle.vue";
import { RemoveIndex } from "@/utilities/types";
import { User } from "@auth0/auth0-spa-js";
import { useAuth0 } from "@auth0/auth0-vue";
import { capitalize } from "lodash";
import { readonly } from "vue";

const { user } = useAuth0();

type Key = keyof RemoveIndex<User>;
const keys = readonly<Key[]>([
  "sub",
  "name",
  "email",
]);

const prettyKeys: { [K in Key]?: string } = {
  sub: "Id",
  nickname: "Username",
};
const prettyKey = (k: Key): string =>
  prettyKeys[k] ?? k.split("_").map(capitalize).join(" ");
</script>

<template>
  <page-title title="Account" />

  <p v-for="k in keys" :key="k">
    <b>{{ prettyKey(k) }}:</b> {{ user[k] }}
  </p>
</template>
