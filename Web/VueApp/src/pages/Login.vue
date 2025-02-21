<template>
  <v-layout id="divTeste" class="fill-height align-center" style="display: flex; background-image: linear-gradient(to top, #3496d4, #2981bb, #1e6da2, #11598a, #024672)">
    <v-layout class="fill-height align-center">
      <v-row>
        <v-col>
          <v-card class="mx-auto" max-width="430">
            <v-layout>
              <v-app-bar elevation="0" color="grey-lighten-3">
                <v-row>
                  <v-col class="ma-3"> Sistema de Controle de Ativos de TI - 2024 </v-col>
                </v-row>
              </v-app-bar>
              <v-main>
                <v-container>
                  <v-form>
                    <v-row>
                      <v-col>
                        <v-img class="mb-4" height="217" src="@/assets/malogo.png" />
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col> </v-col>
                    </v-row>
                    <v-row>
                      <v-col> </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="text-right"> </v-col>
                    </v-row>
                  </v-form>
                </v-container>

                <v-footer color="grey-lighten-3">
                  <v-container class="ma-0 pa-0">
                    <v-row>
                      <v-col class="text-center">
                        <v-chip color="success">IP: 0.0.0.0</v-chip>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="text-center"> © 2024 - M&A Suporte Técnico - V2 </v-col>
                    </v-row>
                  </v-container>
                </v-footer>
              </v-main>
            </v-layout>
          </v-card>
        </v-col>
      </v-row>
    </v-layout>
    <v-layout class="v-layout fill-height align-center" style="display: flex; background-color: white; max-width: 500px">
      <v-row>
        <v-col>
          <v-card class="mx-auto" max-width="430" min-height="300" elevation="0">
            <v-layout>
              <v-main>
                <v-container>
                  <v-form>
                    <v-row>
                      <v-col>
                        <v-text-field name="txtLogin" label="E-Mail" id="txtLogin" ref="txtLogin" v-model="UsrLogin.EMail" prepend-inner-icon="mdi-account" @keypress.enter="TabSenha()"></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col>
                        <v-text-field label="Senha" name="txtSenha" ref="txtSenha" id="txtSenha" autocomplete="curr-pass" v-model="UsrLogin.Senha" :type="PassVisible ? 'text' : 'password'" prepend-inner-icon="mdi-lock" :append-inner-icon="PassVisible ? 'mdi-eye-off' : 'mdi-eye'" @click:append-inner="PassVisible = !PassVisible" @keypress.enter="Login()"></v-text-field>
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col >
                        <v-img height="55" class="ml-0" style="width:auto !important;" src="@/assets/captcha.png" />
                      </v-col>
                    </v-row>
                    <v-row>
                      <v-col class="text-right">
                        <v-btn variant="flat" color="primary" text="Login" @click="Login()"></v-btn>
                      </v-col>
                    </v-row>
                  </v-form>
                </v-container>
              </v-main>
            </v-layout>
          </v-card>
        </v-col>
      </v-row>
    </v-layout>
  </v-layout>
</template>
<route lang="yaml">
meta:
  layout: clean
</route>
<script setup>
definePage({
  meta: {
    title: "Login",
  },
});

import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";
import { useRouter } from "vue-router";

const router = useRouter();
const store = useAppStore();
const PassVisible = ref(false);
const api = inject("SistemaApis");

let txtSenha = ref();
let txtLogin = ref();

let UsrLogin = ref({
  EMail: "",
  Senha: "",
});

function TabSenha() {
  txtSenha.value.focus();
}

function TabEmail() {
  txtLogin.value.focus();
}


async function Login() {
  try {
    let response = await api.Usuario.Login(UsrLogin.value);
    store.SetUsrOpe(response.Dados);
    store.UsrLogon();
    router.push("/");
  } catch (error) {
    //throw new Error (error.message || "Erro interno do servidor.");
  }
}

onMounted(() => {
  store.UsrLogoff();
  store.SetUsrOpe({});
  store.SetIsLoading(false);
  TabEmail();
});
</script>
