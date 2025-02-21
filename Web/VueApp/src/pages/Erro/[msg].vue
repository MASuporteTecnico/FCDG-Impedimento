<template>
  <v-row class="mt-15">
    <v-col cols="6" offset="3">
      <v-alert variant="outlined" border="start" color="error" class="text-center">
        <H1>500 - Erro na Requisição</H1> <br />
        {{ $filters.replaceStr(mensagem,"\n",'&lt;br/&gt') }} <br />
        &nbsp;</v-alert
      >
    </v-col>
  </v-row>
</template>

<script setup>
import {ref} from "vue";

const route = useRoute();
let mensagem = ref("");

onMounted(() => {
   mensagem.value = route.params.msg ?? null;
});

</script>
