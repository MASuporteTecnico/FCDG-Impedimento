<template>
  <v-btn stacked>
    <v-badge color="warning" :content="MensagensNaoLidas">
      <v-icon>mdi-bell-outline</v-icon>
    </v-badge>

    <v-menu activator="parent" :close-on-content-click="false">
      <v-card min-width="350">
        <v-list density="compact">
          <v-list-item nav variant="plain">
            <v-list-item-title>
              <div class="ml-2">
                <v-btn color="primary" size="small" @click="EscreverMensagem()" block>Escrever Mensagem</v-btn>
              </div>
            </v-list-item-title>
          </v-list-item>
          <template v-for="Mensagem in Mensagens">
            <v-list-item :title="Mensagem.Titulo" :subtitle="'De: ' + Mensagem.De.Nome">
              <template v-slot:append>
                <v-btn color="indogo" icon="mdi-open-in-new" variant="text" @click="LerMensagem(Mensagem.Id)" size="small"></v-btn>
                <v-btn :color="!Mensagem.Caixa[0]?.Lida ? 'teal' : 'black'" :icon="Mensagem.Caixa[0]?.Lida ? 'mdi-message-outline' : 'mdi-message-text-outline'" variant="text" @click="SetarLidaNaoLida(Mensagem)"></v-btn>
                <v-btn color="error" icon="mdi-trash-can-outline" variant="text" @click.cancel="(MensagemSelecionada = Mensagem), (ShowConfirmaExcluir = true)" size="small"></v-btn>
              </template>
            </v-list-item>
          </template>
        </v-list>
      </v-card>
    </v-menu>
  </v-btn>

  <v-dialog v-model="DialogMensagem" :overlay="false" max-width="500px">
    <v-card>
      <v-card-title>{{ MensagemSelecionada.Titulo }}</v-card-title>
      <v-card-subtitle>
        <b>De:</b> {{ MensagemSelecionada.De.Nome }} <br />
        <b>Para:</b> {{ MensagemSelecionada.Para.map((Para) => Para.Usuario.Nome).join(", ") }}
      </v-card-subtitle>
      <v-card-text> <VuetifyViewer :value="MensagemSelecionada.Texto" /> </v-card-text>
    </v-card>
  </v-dialog>

  <v-dialog v-model="DialogEscreverMensagem" :overlay="false" max-width="600px">
    <v-card>
    <v-card-title>Nova Mensagem</v-card-title>
      <v-card-text>
        <v-row>
          <v-col> <v-text-field label="Título" v-model="MensagemSelecionada.Titulo"></v-text-field> </v-col>
        </v-row>
        <v-row>
          <v-col> <v-autocomplete :items="Usuarios" v-model="UsuariosSelecionados" label="Para:" multiple=""></v-autocomplete> </v-col>
        </v-row>
        <v-row>
          <v-col> <VuetifyTiptap v-model="MensagemSelecionada.Texto" dense outlined hideBubble removeDefaultWrapper label="Texto" /> </v-col
        ></v-row>
      </v-card-text>
      <v-card-actions>
        <v-btn color="warning" @click="DialogEscreverMensagem = false">Cancelar</v-btn>
        <v-btn color="primary" @click="EnviarMensagem()">Enviar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>

  <confirm Msg="Excluir mensagem?" :Show="ShowConfirmaExcluir" v-on:confirm="DeletarMensagem(), (ShowConfirmaExcluir = false)" v-on:cancel="ShowConfirmaExcluir = false"></confirm>
</template>

<script setup>
import { onMounted, inject } from "vue";
import { useInterval } from "../composables/useInterval";

const api = inject("SistemaApis");

let ShowConfirmaExcluir = ref(false);
let DialogMensagem = ref(false);
let DialogEscreverMensagem = ref(false);
let Mensagens = ref([]);
let Usuarios = ref([]);
let UsuariosSelecionados = ref([]);
let MensagemSelecionada = ref({
  De: {
    Nome: "",
  },
  Para: [],
  Caixa: [],
});

const MensagensNaoLidas = computed(() => {
  let count = 0;
  Mensagens.value.forEach((Mensagem) => {
    if (!Mensagem.Caixa[0].Lida) {
      count++;
    }
  });
  return count;
});

async function EscreverMensagem() {
  let response = await api.Mensagem.Edit(0);
  MensagemSelecionada.value = response.Dados;
  DialogEscreverMensagem.value = true;
}

async function EnviarMensagem() {
  MensagemSelecionada.value.Para = UsuariosSelecionados.value.map((Usuario) => {
    return {
      Usuario: Usuario,
    };
  });

  MensagemSelecionada.value.Para.forEach((Para) => {
    MensagemSelecionada.value.Caixa.push(Para);
  });

  await api.Mensagem.Save(MensagemSelecionada.value);
  DialogEscreverMensagem.value = false;
}

async function DeletarMensagem() {
  await api.Mensagem.Delete(MensagemSelecionada.value);
  await GetMensagens();
}

async function LerMensagem(id) {
  let response = await api.Mensagem.Edit(id);
  MensagemSelecionada.value = response.Dados;
  DialogMensagem.value = true;
}

async function SetarLidaNaoLida(Mensagem) {
  Mensagem.Lida = !Mensagem.Lida;
  await api.Mensagem.SetarLidaNaoLida(Mensagem);
  await GetMensagens();
}

async function GetMensagens() {
  let response = await api.Mensagem.Mensagens();
  Mensagens.value = response.Dados;
}

async function GetListas() {
  Usuarios.value = await api.Lista.Usuarios();
}

GetListas();
GetMensagens();

onMounted(async () => {
  useInterval(GetMensagens, 15000).start();
});
</script>
