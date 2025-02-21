<template></template>
<route lang="yaml">
  meta:
    layout: clean
</route>
<script setup>
definePage({
  meta: {
    title: "Logout",
  },
});

import { useAppStore } from "@/stores/app";
import { useRouter } from 'vue-router';

const store = useAppStore();
store.UsrLogoff();
store.SetUsrOpe({});
store.SetIsLoading(false);

const router = useRouter();
router.push('/Login')
</script>
