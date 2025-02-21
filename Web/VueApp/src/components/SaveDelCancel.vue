<template>
  <div>
    <br />
    <v-row>
      <v-col cols="10" offset="1">
        <v-row>
          <v-col cols="2" v-if="!ReadOnly && !NoDelete"> <v-btn block color="error" @click="ShowConfirmaExcluir = true">Excluir</v-btn> </v-col>
          <v-spacer></v-spacer>
          <v-col cols="3" v-if="!NoCancel"><v-btn block color="warning" @click="(ReadOnly)? returnCancel() :ShowConfirmaCancelar = true">Cancelar</v-btn> </v-col>
          <v-col cols="3" v-if="!ReadOnly && !NoSave"> <v-btn block color="primary" @click="ShowConfirmaSalvar = true">Salvar</v-btn> </v-col>
        </v-row>
      </v-col>
    </v-row>

    <confirm :Msg="MsgDelete" :Show="ShowConfirmaExcluir" v-on:confirm="returnDelete(), (ShowConfirmaExcluir = false)" v-on:cancel="ShowConfirmaExcluir = false"></confirm>
    <confirm :Msg="MsgCancel" :Show="ShowConfirmaCancelar" v-on:confirm="returnCancel(), (ShowConfirmaCancelar = false)" v-on:cancel="ShowConfirmaCancelar = false"></confirm>
    <confirm :Msg="MsgSave" :Show="ShowConfirmaSalvar" v-on:confirm="returnSave(), (ShowConfirmaSalvar = false)" v-on:cancel="ShowConfirmaSalvar = false"></confirm>
  </div>
</template>

<script setup>
defineProps({
  MsgDelete: { type: String, default: "Excluir?" },
  MsgCancel: { type: String, default: "Cancelar?" },
  MsgSave: { type: String, default: "Salvar?" },
  ReadOnly: { type: Boolean, default: false },
  NoDelete: { type: Boolean, default: false },
  NoCancel: { type: Boolean, default: false },
  NoSave: { type: Boolean, default: false }
});

const emit = defineEmits(["cancel", "save", "delete"]);

let ShowConfirmaExcluir = ref(false);
let ShowConfirmaSalvar = ref(false);
let ShowConfirmaCancelar = ref(false);

function returnCancel() {
  emit("cancel");
}

function returnSave() {
  emit("save");
}

function returnDelete() {
  emit("delete");
}
</script>
