<style scoped>
:deep(.v-list-item__spacer) {
  width: 10px !important;
}

:deep(.v-list-group__items) {
  --indent-padding: 25px;
}
</style>

<template>
  <v-btn>
    <template v-slot:prepend>
      <v-icon color="success">mdi-account</v-icon>
    </template>
    {{ propsmenu.UsrOpe.Nome }}

    <v-menu activator="parent">
      <v-list nav :lines="false" density="compact">
        <v-list-item nav variant="plain">
          <v-list-item-title>
            <div class="ml-2">
              <v-switch density="compact" :inset="false" v-model="MenuLateralAtivo" @change="MenuLateralChange()" label="Menu Lateral"></v-switch>
            </div>
          </v-list-item-title>
        </v-list-item>

        <template v-for="menu in items">
          <!-- GRUPO -->
          <v-list-group v-if="TemSubMenu(menu.SubMenu)" :value="menu.ItemMenu">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" :title="menu.ItemMenu" :prepend-icon="menu.Icon"></v-list-item>
            </template>

            <!-- SUBGRUPO -->
            <v-list-item color="primary" nav variant="plain" v-for="submenu in menu.SubMenu" :key="submenu.Id" :to="submenu.Link">
              <template v-slot:prepend>
                <v-icon :icon="submenu.Icon"></v-icon>
              </template>
              <v-list-item-title>{{ submenu.ItemMenu }}</v-list-item-title>
            </v-list-item>
          </v-list-group>

          <!-- DIVIDER -->
          <v-divider v-else-if="menu.Divider"> </v-divider>

          <!-- MENU -->
          <v-list-item color="primary" nav variant="plain" v-else-if="!TemSubMenu(menu.SubMenu)" :to="menu.Link" :key="menu.Id">
            <template v-slot:prepend>
              <v-icon :icon="menu.Icon"></v-icon>
            </template>
            <v-list-item-title>{{ menu.ItemMenu }}</v-list-item-title>
          </v-list-item>
        </template>
      </v-list>
    </v-menu>
  </v-btn>
</template>
<script setup>
import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";

const store = useAppStore();
const api = inject("SistemaApis");
const propsmenu = defineProps(["UsrOpe"]);

let MenuLateralAtivo = ref(true);

const items = ref([
    {
    Id: 7,
    ItemMenu: "---",
    Divider: true,
    AllUsers: true,
    SubMenu: false,
  },
  {
    Id: 8,
    ItemMenu: "Trocar Senha",
    IdMenuPai: 0,
    Link: "/TrocarSenha",
    Icon: "mdi-lock",
    Seq: 1,
    Divider: 0,
    AllUsers: true,
    SubMenu: false,
  },
  {
    Id: 7,
    ItemMenu: "---",
    Divider: true,
    AllUsers: true,
    SubMenu: false,
  },
  {
    Id: 8,
    ItemMenu: "Sair",
    IdMenuPai: 0,
    Link: "/logout",
    Icon: "mdi-exit-to-app",
    Seq: 1,
    Divider: 0,
    AllUsers: true,
    SubMenu: false,
  },
]);

async function MenuLateralChange() {
  store.SetMenuLateralATivo(MenuLateralAtivo.value);
  await api.Usuario.MenuLateral(MenuLateralAtivo.value);
}

function TemSubMenu(SubMenu) {
  if (SubMenu) {
    return true;
  }
  return false;
}

MenuLateralAtivo.value = store.GetMenuLateralATivo;
</script>
