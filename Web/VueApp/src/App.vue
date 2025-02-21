<template style="overflow: hidden !important">
  <router-view />
  <Loading v-if="IsLoading" />
</template>

<script setup>
import axios from "axios";
import { useAppStore } from "@/stores/app";
import Loading from "@/components/Loading.vue";
import { onErrorCaptured } from "vue";
import { useToast } from "vue-toastification";

const store = useAppStore();
const toast = useToast();
const router = useRouter();


let IsLoading = computed(() => {
  return store.IsLoading;
});

onErrorCaptured((error, component, info) => {
  let erroAxios = store.GetLastError;

  if (erroAxios != error.message) {
    let t = { error, component, info };
    let msg = `${error.message}` || "Erro interno do servidor.";
    toast.error(msg);
  }
  store.SetLastError("");
});

//AXIOS INTERCEPTORS PARA REQUEST
axios.interceptors.request.use(
  (config) => {
    store.SetIsLoading(true);
    return config;
  },
  (error) => {
    store.SetIsLoading(false);
    return Promise.reject(error);
  }
);

//AXIOS INTERCEPTORS PARA RESPONSE
axios.interceptors.response.use(
  (response) => {
    store.SetIsLoading(false);

    if (response.data.Sucesso && response.data.Mensagem) {
      toast.success(response.data.Mensagem);
    }

    return response;
  },
  (error) => {
    let msg = "";

    store.SetIsLoading(false);
    store.SetLastError(error.response.data?.Mensagem || error.message);

    switch (error.status) {
      case 401:
        router.push({ name: "/Logout" });
        break;

      case 403:
        router.push({ name: "/Negado" });
        break;

      case 500:
        msg = error.response.data?.Mensagem || "Erro interno do servidor (500).";
        router.push({ name: "/Erro/[msg]", params: { msg: msg } });
        break;

      case 400:
        msg = error.response.data?.Mensagem || "Erro interno do servidor (400).";
        toast.error(msg);
        break;

      case 404:
        msg = error.message || "Erro interno do servidor (404).";
        toast.error(msg);
        break;

      default:
        //msg = error.response.data?.Mensagem || "Erro interno do servidor.";
        //toast.error(msg);
        break;
    }
    return Promise.reject(error);
  }
);
</script>
