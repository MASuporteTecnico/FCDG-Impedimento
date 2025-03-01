<template>
  <v-row class="ma-15 pa-15">
    <v-col cols="10" offset="1">
      <v-alert variant="outlined" border="start" color="error" class="text-center">
        <H1>404 - Página nã oencontrada</H1> <br />
        A página requisitada não existe. <br />
        &nbsp;</v-alert>
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
