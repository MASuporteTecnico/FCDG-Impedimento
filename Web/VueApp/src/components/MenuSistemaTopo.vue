<style scoped>
:deep(.v-list-group__items .v-list-item) {
  margin-left: 30px;
  padding-inline-start: 5px !important;
}

:deep(.v-list-group__items .v-list-item) {
  margin-left: 30px;
}

:deep(.v-list-group__items .v-list-item__spacer) {
  width: 8px !important;
}

:deep(.v-list-item__prepend .v-list-item__spacer) {
  width: 8px !important;
}

:deep(.v-list-group__items) {
  margin-bottom: min(4px);
}
</style>

<template>
  <template v-for="menu in Menu">
    <!-- GRUPO -->

    <v-menu open-on-hover v-if="TemSubMenu(menu.SubMenu)">
      <template v-slot:activator="{ props }">
        <v-divider opacity="100" class="ma-1" vertical> </v-divider>
        <v-btn v-bind="props">
          <v-icon :icon="menu.Icone"></v-icon>
          {{ menu.Nome }}
        </v-btn>
      </template>
      <v-list>
        <v-list-item v-for="subItem in menu.SubMenu" :to="subItem.Rota" :key="i" link density="compact">
          <v-list-item-title> {{ subItem.Nome }}</v-list-item-title>
          <template v-slot:prepend>
            <v-icon :icon="subItem.Icone"></v-icon>
          </template>
        </v-list-item>
      </v-list>
    </v-menu>
    <template v-else-if="!menu.Divisor">
      <v-divider opacity="100" class="ma-1" vertical> </v-divider>
      <v-btn :to="menu.Rota">
        <v-icon :icon="menu.Icone"></v-icon>
        {{ menu.Nome }}
      </v-btn>
    </template>
  </template>
</template>

<script setup>
import { onMounted, inject } from "vue";
import { useAppStore } from "@/stores/app";
import { useInterval } from "../composables/useInterval";

const store = useAppStore();
const api = inject("SistemaApis");

let Menu = ref([]);
let MenuResponse = ref([]);
let Rotas = ref([]);

function TemSubMenu(SubMenu) {
  if (SubMenu) {
    if (SubMenu.length > 0) return true;
  }
  return false;
}

async function GetMenu() {
  let response = await api.Usuario.Menu();

  MenuResponse.value = response.SubMenu;
  Rotas.value = response.Rotas;
  Menu.value = [];

  MenuResponse.value.forEach((value, index) => {
    Menu.value.push(value);
  });

  store.SetRotas(Rotas.value);
}

GetMenu();

onMounted(async () => {
  useInterval(GetMenu, 15000).start();
});
</script>
