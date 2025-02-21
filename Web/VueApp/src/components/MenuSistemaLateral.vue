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

:deep(.v-list-group__items) {
  margin-bottom: min(4px);
}
</style>

<template>
  <v-list nav open-strategy="list" active-strategy="independent" variant="flat" base-color="rgba(255, 255, 255, 0.1)" :slim="true" theme="dark">
    <template v-for="menu in Menu">
      <!-- GRUPO -->
      <v-list-group v-if="TemSubMenu(menu.SubMenu)" :value="menu.Nome">
        <template v-slot:activator="{ props }">
          <v-list-item v-bind="props" :title="menu.Nome" :prepend-icon="menu.Icone" :key="menu.Id"></v-list-item>
        </template>

        <!-- SUBGRUPO -->
        <v-list-item color="primary" v-for="submenu in menu.SubMenu" :key="submenu.Id" :to="submenu.Rota">
          <template v-slot:prepend>
            <v-icon :icon="submenu.Icone"></v-icon>
          </template>
          <v-list-item-title>{{ submenu.Nome }}</v-list-item-title>
        </v-list-item>
      </v-list-group>

      <!-- DIVIDER -->
      <v-divider opacity="100" class="mb-1" v-else-if="menu.Divisor"> </v-divider>

      <!-- MENU -->
      <v-list-item color="primary" v-else :to="menu.Rota" :key="menu.Id">
        <template v-slot:prepend>
          <v-icon :icon="menu.Icone"></v-icon>
        </template>
        <v-list-item-title>{{ menu.Nome }}</v-list-item-title>
      </v-list-item>
    </template>
  </v-list>
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
