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
const router = useRouter();
let dominio = store.GetDominio;

store.UsrLogoff();
store.SetUsrOpe({});
store.SetIsLoading(false);

if(dominio) {
  store.SetDominio(null);
  router.push('/Login?dominio=' + dominio);
}else{
  router.push('/Login');
}

</script>
