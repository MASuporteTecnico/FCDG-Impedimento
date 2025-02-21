<style scope>
html,
body {
  height: 100%;
  margin: 0;
}

.RowDisabled {
  background-color: #ffcdd2 !important;
  outline: thin solid rgb(var(--v-theme-error)) !important;
}

#nav_drawer {
  background-image: linear-gradient(to top, #124e74, #0d4971, #0b476f, #06456f, #024672);
  background-size: cover;
  color: white;
}

#tollbar_top {
  background-image: linear-gradient(to left, #124e74, #0d4971, #0b476f, #06456f, #024672);
  background-size: cover;
  color: white;
}

.appbar_top_color {
  background-image: linear-gradient(to left, #124e74, #0d4971, #0b476f, #06456f, #024672) !important;
  background-size: cover;
  color: white;
  border-bottom: #024672 2px solid !important;
}

.appbar_top_white {
  background-color: white;
  border-bottom: #024672 2px solid !important;
}
</style>
<template>
  <v-app>
    <v-navigation-drawer id="nav_drawer" permanent floating :rail="rail" v-if="UsrLogged && MenuLateralATivo" class="d-print-none">
      <v-list-item>
        <template v-slot:prepend>
          <v-avatar size="65">
            <v-img src="@/assets/malogo.png" />
          </v-avatar>
        </template>
        <v-list-item-title class="text-capitalize text-wrap text-center">
          <span>
            <b
              ><h3>{{ (UsrOpe.Empresa?.Nome || "-").toUpperCase() }}</h3>
            </b>
          </span>
        </v-list-item-title>
      </v-list-item>
      <v-divider></v-divider>

      <MenuSistemaLateral></MenuSistemaLateral>
    </v-navigation-drawer>

    <v-app-bar flat v-if="UsrLogged" fixed :color="MenuLateralATivo ? '' : 'primary'" :class="MenuLateralATivo ? 'appbar_top_white' : 'appbar_top_color'">
      <v-app-bar-nav-icon @click="rail = !rail" v-if="false"></v-app-bar-nav-icon>
      <template v-if="MenuLateralATivo">
        <v-app-bar-title>
          M&A SISTEMAS
          <v-icon>mdi-chevron-right</v-icon>
          {{ TituloTela }}
        </v-app-bar-title>
      </template>
      <template v-else>
        <div class="pa-3 text-h6">
          M&A SISTEMAS          
        </div>
      </template>

      <MenuSistemaTopo v-if="!MenuLateralATivo"></MenuSistemaTopo>
      <v-spacer></v-spacer>
      <template v-slot:append>
        <div class="mr-3">
          <!-- <AutoLogoffBKP :Seconds="TimeoutBaseTime" Enabled Page="/Logout"></AutoLogoffBKP -->
          <AutoLogoff :duration="TimeoutBaseTime" :reminders="[15]" @idle="onidle()"></AutoLogoff>          
          <Avisos></Avisos>
          <MenuUsuario :UsrOpe="UsrOpe"></MenuUsuario>
        </div>
      </template>
    </v-app-bar>
    <v-main>
      <v-container fluid class="pa-3 pt-5" :style="!UsrLogged ? 'height: 100vh;' : ''">
        <div style="border: #024672 2px solid !important">
          <router-view />
        </div>
      </v-container>
    </v-main>
    <AppFooter v-if="UsrLogged" />
  </v-app>
</template>

<script setup>
import { ref, computed } from "vue";
import { useAppStore } from "@/stores/app";
import Avisos from "@/components/Avisos.vue";
import AutoLogoff from "@/components/AutoLogoff.vue";
import MenuSistemaTopo from "@/components/MenuSistemaTopo.vue";

const store = useAppStore();
const rail = ref(false);
const TimeoutBaseTime = 600;

const UsrLogged = computed(() => {
  return store.getUsrLogged;
});

const UsrOpe = computed(() => {
  return store.getUsrOpe;
});

const MenuLateralATivo = computed(() => {
  return store.GetMenuLateralATivo;
});

function onMouseMove() {
  store.CountDownLogoffReset = !store.CountDownLogoffReset;
}

function onKeyPress() {
  store.CountDownLogoffReset = !store.CountDownLogoffReset;
}

const TituloTela = computed(() => {
  return store.GetTituloTela;
});

function onidle() {
  window.location.href = "/Logout";
}

</script>
