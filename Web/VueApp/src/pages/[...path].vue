<template>
  <v-row class="ma-15 pa-15">
    <v-col cols="6" offset="3" >
      <v-alert variant="outlined" border="start" color="error" class="text-center"> <H1>404</H1> <br> PÁGINA NÃO ENCONTRADA. <br> &nbsp;</v-alert>
    </v-col>
  </v-row>
</template>
<script setup>
definePage({
  meta: {
    title: "Página Não Encontrada",
  },
});
</script>
